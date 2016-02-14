
// For styles I should use css http://tutorials.jenkov.com/svg/svg-and-css.html#css-attributes
// SVG reference http://www.cheat-sheets.org/own/svg/index.xhtml
// SVG Style and animation reference http://www.smashingmagazine.com/2014/11/styling-and-animating-svgs-with-css/
// SVG animation http://blogs.adobe.com/dreamweaver/2015/06/the-state-of-svg-animation.html
// TODO: Add styles
// TODO: Add anchor, stroke, font, decorator, spacing, and writing mode to text
// TODO: Add id to everything
// TODO: Add def and use to make reusable elements
//
//<?xml version="1.0"?>
//<svg viewBox="0 0 120 120" version="1.1"
//  xmlns="http://www.w3.org/2000/svg">
//  <circle cx="60" cy="60" r="50"/>
//  <line x1="0" y1="0" x2="200" y2="200" style="stroke:rgb(255,0,0);stroke-width:2" />
//</svg>
//
// style attributes fill, stroke and stroke-width
//<style type="text/css">
//div {font-family: Arial; font-size: 14px; margin-left: 30px;}
//p   {border-left: 1px dotted gray;}
//circle {stroke: #006600; fill: #00cc00;}
//</style>
namespace SharpVG

open Helpers
open ColorHelpers
open SizeHelpers
open PointHelpers
open AreaHelpers
open StyleHelpers
open TransformHelpers


type Line = {
    Point1: Point
    Point2: Point
}

type Text = {
    UpperLeft: Point
    Body: Text
}

type Image = {
    UpperLeft: Point
    Size: Area
    Label: string option
}

type Circle = {
    Center: Point
    Radius: Size
}

type Ellipse = {
    Center: Point
    Radius: Point
}

type Rect = {
    UpperLeft: Point
    Size: Area
}

type Polygon = Points of seq<Point>


type Polyline = Points of seq<Point>

type Script = Body of string

type Tag =
    | Line
    | Text
    | Image
    | Circle
    | Ellipse
    | Rect
    | Polygon
    | Polyline

type StyledElement = Tag * Style

type Svg = {
    Body: Body
    UpperLeft: Point option
}

and Group = {
    Body: Body
    UpperLeft: Point
    Transform: Transform option
}

and BodyElement =
    | Group
    | Svg
    | Script
    | Tag
    | StyledElement

and Body =
    seq<BodyElement>



module Core =
    // Public
    let emptyTagToString name attribute = "<" + name + " " + attribute + "/>"

    let tagToString name attribute body = "<" + name + " " + attribute + "/" + body + ">"

    let addSpaces strings = (strings |> Seq.reduce (fun acc str -> acc + " " + str)).TrimStart()

    let imageAttributesToString image =
        seq {
            yield (quote image.Label);
            yield (pointToDescriptiveString image.UpperLeft);
            yield (areaToString image.Size)
        } |> addSpaces

    let textAttributesToString text =
        seq {
            yield (quote text.Body);
            yield (pointToDescriptiveString text.UpperLeft);
        } |> addSpaces

    let lineAttributesToString line =
        seq {
            yield pointModifierToDescriptiveString line.Point1 "" "1";
            yield pointModifierToDescriptiveString line.Point2 "" "2";
        } |> addSpaces

    let circleAttributesToString circle =
        seq {
            yield pointModifierToDescriptiveString circle.Center "c" "";
            yield "r=" + quote (pointToString circle.Radius);
        } |> addSpaces


    let ellipseAttributesToString ellipse =
        seq {
            yield pointModifierToDescriptiveString ellipse.Center "c" "";
            yield pointModifierToDescriptiveString ellipse.Radius "r" "";
        } |> addSpaces

    let rectAttributesToString (rect: Rect) =
        seq {
            yield pointToDescriptiveString rect.UpperLeft;
            yield areaToString rect.Size;
        } |> addSpaces

    let polygonAttributesToString polygon = "points=" + quote (pointsToString polygon)

    //TODO: Remove duplicate from polygon
    let polylineAttributesToString polyline = "points=" + quote (pointsToString polyline)

    //TODO: Add tag to string

    let html title body =
        "<!DOCTYPE html>\n<html>\n<head>\n  <title>" +
        title +
        "</title>\n</head>\n<body>\n" +
        body +
        "</body>\n</html>\n"

    let style =
        "<style>
        circle {
        stroke: #006600;
        fill  : #00cc00;
        }
        circle.allBlack {
        stroke: #000000;
        fill  : #000000;
        }\n" +
        "</style>\n"

    let svg size body =
        "<svg " + areaToString size + ">\n  " +
        body +
        "\n</svg>\n"

    let group id transform point body =
        "<g id=" + quote id +
        transformToString transform +
        pointToString point + ">" +
        body +
        "</g>"

    let image upperLeft size imageName =
        "<image xlink:href=" +
        quote imageName + " " +
        pointToDescriptiveString upperLeft + " " +
        areaToString size +
        "/>"

    let text style upperLeft text =
        "<text " +
        pointToDescriptiveString upperLeft + " " +
        styleToString style + " " +
        ">" +
        text +
        "</text>"

    let line style point1 point2 =
        "<line " +
        pointModifierToDescriptiveString point1 "" "1" + " " +
        pointModifierToDescriptiveString point2 "" "2" + " " +
        styleToString style +
        "/>"

    let circle style center radius =
        "<circle " +
        pointModifierToDescriptiveString center "c" "" +
        " r=" + quote (sizeToString radius) + " " +
        styleToString style +
        "/>"

    let ellipse style center radius =
        "<ellipse " +
        pointModifierToDescriptiveString center "c" "" + " " +
        pointModifierToDescriptiveString radius "r" "" + " " +
        styleToString style +
        "/>"

    let rect style upperLeft size =
        "<rect " +
        pointToDescriptiveString upperLeft + " " +
        areaToString size + " " +
        styleToString style +
        "/>"

    let polygon style points =
        "<polygon points=" +
        quote (pointsToString points) + " " +
        styleToString style +
        "/>"

    let polyline style points =
        "<polyline points=" +
        quote (pointsToString points) + " " +
        styleToString style +
        "/>"

    let script body =
        "<script type=\"application/ecmascript\"><![CDATA[" +
        body +
        "]]></script>"

// Playground:

// TODO: Make tag a base class and create derived classes that implement .WithAttributes() etc.
    let line2 line =
        let (style, lineSegment) = line
        let (point1, point2) = lineSegment
        "<line " +
        pointModifierToDescriptiveString point1 "" "1" + " " +
        pointModifierToDescriptiveString point2 "" "2" + " " +
        (match style with Some(style) -> styleToString style | None -> "") +
        "/>"

    let withStyle element style = (element, style)
    let createLineSegment point1 point2 = (point1, point2)
    let createPointFromPixels x y = { X = Size.Pixels(x); Y = Size.Pixels(y) }

    let test =
        let s = {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Color.Name(Colors.Red); }
        let a = line2 (Some(s),({X = Size.Pixels(10); Y = Size.Pixels(10)}, {X = Size.Pixels(10); Y = Size.Pixels(10)}))
        let b = line2 (Some(s), createLineSegment (createPointFromPixels 1 1) (createPointFromPixels 10 10))
        0