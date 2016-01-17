
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

type Size =
    | Pixels of int
    | Ems of double
    | Percent of int
type Style = { Fill : Color; Stroke : Color; StrokeWidth : Size }
type Point = { X : Size; Y : Size; }
type Area = { Width : Size; Height : Size; }

module Core =

    open System

    // Helpers
    let quoter = "\""
    let inline quote i =
        quoter + string i + quoter

    let addSpace needsSpace =
        (if needsSpace then " " else "")

    let sizeToString size =
        match size with
        | Pixels p -> string p
        | Ems e -> string e + "em"
        | Percent p -> string p + "%"

    let pointModifierToDescriptiveString point pre post =
        pre + "x" + post + "=" + quote (sizeToString point.X) + " " +
        pre + "y" + post + "=" + quote (sizeToString point.Y)

    let pointToDescriptiveString point =
        pointModifierToDescriptiveString point "" ""

    let pointToString point =
        sizeToString point.X + "," + sizeToString point.Y

    let pointsToString pointsToString =
        pointsToString
        |> Seq.fold (
            fun acc point ->
            acc + addSpace (acc <> "") + pointToString point
            ) ""

    let areaToString area =
        "height=" + quote (sizeToString area.Height) + " " +
        "width=" + quote (sizeToString area.Width)

    let colorToString color =
        match color with
        | Name n -> Enum.GetName(typeof<Colors>, n).ToLower()
        | SmallHex sh -> String.Format("0x{0:x}", sh)
        | Hex h -> String.Format("0x{0:x}", h)
        | Values (r, g, b) -> "(" + string r + ", " + string g + ", " + string b + ")"
        | Percents (r, g, b) -> "(" + string r + "%, " + string g + "%, " + string b + "%)"

    let styleToString style =
        "stroke=" + quote (colorToString style.Stroke) + " " +
        "stroke-width=" + quote (sizeToString style.StrokeWidth) + " " +
        "fill=" + quote (colorToString style.Fill)

    // Public
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
        pointModifierToDescriptiveString center "r" "" + " " +
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