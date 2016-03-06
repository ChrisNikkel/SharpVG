
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
open TransformHelpers

type Element =
    | Line of Line
    | Text of Text
    | Image of Image
    | Circle of Circle
    | Ellipse of Ellipse
    | Rect of Rect
    | Polygon of Polygon
    | Polyline of Polyline
    
    member __.asBase =
        match __ with
                | Line l -> (l :> ElementBase)
                | Text t -> (t :> ElementBase)
                | Image i -> (i :> ElementBase)
                | Circle c -> (c :> ElementBase)
                | Ellipse e -> (e :> ElementBase)
                | Rect r -> (r :> ElementBase)
                | Polygon p -> (p :> ElementBase)
                | Polyline p -> (p :> ElementBase)
                
    member __.toString =
        let name = __.asBase.name
        
        let attributes =
            let elementAttributes =
                match __ with
                    | Line { Point1 = point1; Point2 = point2 } ->
                        pointModifierToDescriptiveString point1 "" "1" + " " +
                        pointModifierToDescriptiveString point2 "" "2"

                    | Text { UpperLeft = upperLeft; } ->
                        pointToDescriptiveString upperLeft

                    | Image { UpperLeft = upperLeft; Size = size; Source = source } ->
                    "xlink:href=" +
                        quote source + " " +
                        pointToDescriptiveString upperLeft + " " +
                        areaToString size

                    | Circle  { Center = center; Radius = radius } ->
                        pointModifierToDescriptiveString center "c" "" +
                        " r=" + quote (sizeToString radius)

                    | Ellipse { Center = center; Radius = radius } ->
                        pointModifierToDescriptiveString center "c" "" +
                        " r=" + quote (pointToString radius)

                    | Rect { UpperLeft = upperLeft; Size = size; } ->
                        pointToDescriptiveString upperLeft + " " +
                        areaToString size

                    | Polygon { Points = points } | Polyline { Points = points } ->
                        "points=" + quote (pointsToString points)

            match __ with
                | Line { Style = Some(style) } | Text { Style = Some(style) } | Circle { Style = Some(style) } | Ellipse { Style = Some(style) } | Rect { Style = Some(style) } | Polygon { Style = Some(style) } | Polyline { Style = Some(style) } ->
                    elementAttributes + " " + style.toString
                | _ -> elementAttributes

        let body =
            match __ with
                | Text { Body = body } -> Some(body)
                | _ -> None
                
        match body with
            | Some(body) -> "<" + name + " " + attributes + ">" + body + "</" + name + ">"
            | None -> "<" + name + " " + attributes + ">"
    

    override __.ToString() = __.toString

    //member this.withLabel source with Source = Some(label);

    //member this.toString =
    //    "<" + this.name + " " +
    //    this.attributes +
    //    "/>"

    //override this.ToString() = this.toString

and ElementSvgStringifier(element : Element) =
    member __.name =
        match element with
            | Line _ -> "line"
            | Text _ -> "text"
            | Image _ -> "image"
            | Circle _ -> "circle"
            | Ellipse _ -> "ellipse"
            | Rect _ -> "rect"
            | Polygon _ -> "polygon"
            | Polyline _ -> "polyline"

    member __.attributes =
        let elementAttributes =
            match element with
                | Line { Point1 = point1; Point2 = point2 } ->
                    pointModifierToDescriptiveString point1 "" "1" + " " +
                    pointModifierToDescriptiveString point2 "" "2"

                | Text { UpperLeft = upperLeft; } ->
                    pointToDescriptiveString upperLeft

                | Image { UpperLeft = upperLeft; Size = size; Source = source } ->
                "xlink:href=" +
                    quote source + " " +
                    pointToDescriptiveString upperLeft + " " +
                    areaToString size

                | Circle  { Center = center; Radius = radius } ->
                    pointModifierToDescriptiveString center "c" "" +
                    " r=" + quote (sizeToString radius)

                | Ellipse { Center = center; Radius = radius } ->
                    pointModifierToDescriptiveString center "c" "" +
                    " r=" + quote (pointToString radius)

                | Rect { UpperLeft = upperLeft; Size = size; } ->
                    pointToDescriptiveString upperLeft + " " +
                    areaToString size

                | Polygon { Points = points } | Polyline { Points = points } ->
                    "points=" + quote (pointsToString points)

        match element with
            | Line { Style = Some(style) } | Text { Style = Some(style) } | Circle { Style = Some(style) } | Ellipse { Style = Some(style) } | Rect { Style = Some(style) } | Polygon { Style = Some(style) } | Polyline { Style = Some(style) } ->
                elementAttributes + " " + style.toString
            | _ -> elementAttributes

    member __.body =
        match element with
            | Text { Body = body } -> Some(body)
            | _ -> None

    member __.toString =
        match __.body with
            | Some(body) -> "<" + __.name + " " + __.attributes + ">" + body + "</" + __.name + ">"
            | None -> "<" + __.name + " " + __.attributes + ">"

    override __.ToString() = __.toString

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
    | Element

and Body =
    seq<BodyElement>



module Core =
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