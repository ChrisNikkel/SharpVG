
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
module SharpVG
  open System
  
  type Colors =
    | None = 0
    | AliceBlue = 1
    | AntiqueWhite = 2
    | Aqua = 3
    | Aquamarine = 4
    | Azure = 5
    | Beige = 6
    | Bisque = 7
    | Black = 8
    | BlanchedAlmond = 9
    | Blue = 10
    | BlueViolet = 11
    | Brown = 12
    | BurlyWood = 13
    | CadetBlue = 14
    | Chartreuse = 15
    | Chocolate = 16
    | Coral = 17
    | CornflowerBlue = 18
    | CornSilk = 19
    | Crimson = 20
    | Cyan = 21
    | DarkBlue = 22
    | DarkCyan = 23
    | DarkGoldenRod = 24
    | DarkGray = 25
    | DarkGreen = 26
    | DarkGrey = 27
    | DarkKhaki = 28
    | DarkMagenta = 29
    | DarkOliveGreen = 30
    | DarkOrange = 31
    | DarkOrchid = 32
    | DarkRed = 33
    | DarkSalmon = 34
    | DarkSeaGreen = 35
    | DarkSlateBlue = 36
    | DarkSlateGray = 37
    | DarkSlateGrey = 38
    | DarkTurquoise = 39
    | DarkViolet = 40
    | DeepPink = 41
    | DeepSkyBlue = 42
    | DimGray = 43
    | DimGrey = 44
    | DodgerBlue = 45
    | FireBrick = 46
    | FloralWhite = 47
    | ForestGreen = 48
    | Fuchsia = 49
    | Gainsboro = 50
    | GhostWhite = 51
    | Gold = 52
    | GoldenRod = 53
    | Gray = 54
    | Grey = 55
    | Green = 56
    | GreenYellow = 57
    | HoneyDew = 58
    | HotPink = 59
    | IndianRed = 60
    | Indigo = 61
    | Ivory = 62
    | Khaki = 63
    | Lavender = 64
    | LavenderBlush = 65
    | LawnGreen = 66
    | LemonChiffon = 67
    | LightBlue = 68
    | LightCoral = 69
    | LightCyan = 70
    | LightGoldenRodYellow = 71
    | LightGray = 72
    | LightGreen = 73
    | LightGrey = 74
    | LightPink = 75
    | LightSalmon = 76
    | LightSeaGreen = 77
    | LightSkyBlue = 78
    | LightSlateGray = 79
    | LightSlateGrey = 80
    | LightSteelBlue = 81
    | LightYellow = 82
    | Lime = 83
    | LimeGreen = 84
    | Linen = 85
    | Magenta = 86
    | Maroon = 87
    | MediumAquamarine = 88
    | MediumBlue = 89
    | MediumOrchid = 90
    | MediumPurple = 91
    | MediumSeaGreen = 92
    | MediumSlateBlue = 93
    | MediumSpringGreen = 94
    | MediumTurquoise = 95
    | MediumVioletRed = 96
    | MidnightBlue = 97
    | MintCream = 98
    | MistyRose = 99
    | Moccasin = 100
    | NavajoWhite = 101
    | Navy = 102
    | OldLace = 103
    | Olive = 104
    | OliveDrab = 105
    | Orange = 106
    | OrangeRed = 107
    | Orchid = 108
    | PaleGoldenRod = 109
    | PaleGreen = 110
    | PaleTurquoise = 111
    | PaleVioletRed = 112
    | PapayaWhip = 113
    | PeachPuff = 114
    | Peru = 115
    | Pink = 116
    | Plum = 117
    | PowderBlue = 118
    | Purple = 119
    | Red = 120
    | RosyBrown = 121
    | RoyalBlue = 122
    | SaddleBrown = 123
    | Salmon = 124
    | SandyBrown = 125
    | SeaGreen = 126
    | SeaShell = 127
    | Sienna = 128
    | Silver = 129
    | SkyBlue = 130
    | SlateBlue = 131
    | SlateGray = 132
    | SlateGrey = 133
    | Snow = 134
    | SpringGreen = 135
    | SteelBlue = 136
    | Tan = 137
    | Teal = 138
    | Thistle = 139
    | Tomato = 140
    | Turquoise = 141
    | Violet = 142
    | Wheat = 143
    | White = 144
    | WhiteSmoke = 145
    | Yellow = 146
    | YellowGreen = 147
  
  type Color =
    | Name of Colors
    | SmallHex of int16  // #rgb
    | Hex of int  // #rrggbb
    | Values of byte * byte * byte // 0 - 255
    | Percents of double * double * double // 0.0% - 100.0%
  
  type Size =
    | Pixels of int
    | Ems of double
    | Percent of int
  type Style = { Fill : Color; Stroke : Color; StrokeWidth : Size }
  type Point = { X : Size; Y : Size; }
  type Area = { Width : Size; Height : Size; }

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