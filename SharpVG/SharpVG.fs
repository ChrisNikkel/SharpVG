
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

type Style =
    {
        Fill : Color;
        Stroke : Color;
        StrokeWidth : Size;
    }


type Line =
    {
        Point1: Point
        Point2: Point
    }

type Text =
    {
        UpperLeft: Point
        Body: string
    }

type Image =
    {
        UpperLeft: Point
        Size: Area
        Source: string
    }

type Circle =
    {
        Center: Point
        Radius: Size
    }

type Ellipse =
    {
        Center: Point
        Radius: Point
    }

type Rect =
    {
        UpperLeft: Point
        Size: Area
    }

type Points = seq<Point>

type Script = Body of string

type BaseElement =
    | Line of Line
    | Text of Text
    | Image of Image
    | Circle of Circle
    | Ellipse of Ellipse
    | Rect of Rect
    | Polygon of Points
    | Polyline of Points

// TODO: Re-add Style
type Element =
    | StyledElement of BaseElement * Style
    | PlainElement of BaseElement

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