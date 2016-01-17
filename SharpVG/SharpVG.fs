
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


module Core =
    open Helpers
    open ColorHelpers
    open SizeHelpers
    open PointHelpers
    open AreaHelpers
    open StyleHelpers

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