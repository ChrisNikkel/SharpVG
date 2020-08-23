namespace SharpVG

open System

type Colors =
    | AliceBlue
    | AntiqueWhite
    | Aqua
    | Aquamarine
    | Azure
    | Beige
    | Bisque
    | Black
    | BlanchedAlmond
    | Blue
    | BlueViolet
    | Brown
    | BurlyWood
    | CadetBlue
    | Chartreuse
    | Chocolate
    | Coral
    | CornflowerBlue
    | CornSilk
    | Crimson
    | Cyan
    | DarkBlue
    | DarkCyan
    | DarkGoldenRod
    | DarkGray
    | DarkGreen
    | DarkGrey
    | DarkKhaki
    | DarkMagenta
    | DarkOliveGreen
    | DarkOrange
    | DarkOrchid
    | DarkRed
    | DarkSalmon
    | DarkSeaGreen
    | DarkSlateBlue
    | DarkSlateGray
    | DarkSlateGrey
    | DarkTurquoise
    | DarkViolet
    | DeepPink
    | DeepSkyBlue
    | DimGray
    | DimGrey
    | DodgerBlue
    | FireBrick
    | FloralWhite
    | ForestGreen
    | Fuchsia
    | Gainsboro
    | GhostWhite
    | Gold
    | GoldenRod
    | Gray
    | Grey
    | Green
    | GreenYellow
    | HoneyDew
    | HotPink
    | IndianRed
    | Indigo
    | Ivory
    | Khaki
    | Lavender
    | LavenderBlush
    | LawnGreen
    | LemonChiffon
    | LightBlue
    | LightCoral
    | LightCyan
    | LightGoldenRodYellow
    | LightGray
    | LightGreen
    | LightGrey
    | LightPink
    | LightSalmon
    | LightSeaGreen
    | LightSkyBlue
    | LightSlateGray
    | LightSlateGrey
    | LightSteelBlue
    | LightYellow
    | Lime
    | LimeGreen
    | Linen
    | Magenta
    | Maroon
    | MediumAquamarine
    | MediumBlue
    | MediumOrchid
    | MediumPurple
    | MediumSeaGreen
    | MediumSlateBlue
    | MediumSpringGreen
    | MediumTurquoise
    | MediumVioletRed
    | MidnightBlue
    | MintCream
    | MistyRose
    | Moccasin
    | NavajoWhite
    | Navy
    | OldLace
    | Olive
    | OliveDrab
    | Orange
    | OrangeRed
    | Orchid
    | PaleGoldenRod
    | PaleGreen
    | PaleTurquoise
    | PaleVioletRed
    | PapayaWhip
    | PeachPuff
    | Peru
    | Pink
    | Plum
    | PowderBlue
    | Purple
    | Red
    | RosyBrown
    | RoyalBlue
    | SaddleBrown
    | Salmon
    | SandyBrown
    | SeaGreen
    | SeaShell
    | Sienna
    | Silver
    | SkyBlue
    | SlateBlue
    | SlateGray
    | SlateGrey
    | Snow
    | SpringGreen
    | SteelBlue
    | Tan
    | Teal
    | Thistle
    | Tomato
    | Turquoise
    | Violet
    | Wheat
    | White
    | WhiteSmoke
    | Yellow
    | YellowGreen
    override this.ToString() =
        match this with
        | AliceBlue -> "AliceBlue"
        | AntiqueWhite -> "AntiqueWhite"
        | Aqua -> "Aqua"
        | Aquamarine -> "Aquamarine"
        | Azure -> "Azure"
        | Beige -> "Beige"
        | Bisque -> "Bisque"
        | Black -> "Black"
        | BlanchedAlmond -> "BlanchedAlmond"
        | Blue -> "Blue"
        | BlueViolet -> "BlueViolet"
        | Brown -> "Brown"
        | BurlyWood -> "BurlyWood"
        | CadetBlue -> "CadetBlue"
        | Chartreuse -> "Chartreus"
        | Chocolate -> "Chocolate"
        | Coral -> "Coral"
        | CornflowerBlue -> "CornflowerBlue"
        | CornSilk -> "CornSilk"
        | Crimson -> "Crimson"
        | Cyan -> "Cyan"
        | DarkBlue -> "DarkBlue"
        | DarkCyan -> "DarkCyan"
        | DarkGoldenRod -> "DarkGoldenRod"
        | DarkGray -> "DarkGray"
        | DarkGreen -> "DarkGreen"
        | DarkGrey -> "DarkGrey"
        | DarkKhaki -> "DarkKhaki"
        | DarkMagenta -> "DarkMagenta"
        | DarkOliveGreen -> "DarkOliveGreen"
        | DarkOrange -> "DarkOrange"
        | DarkOrchid -> "DarkOrchid"
        | DarkRed -> "DarkRed"
        | DarkSalmon -> "DarkSalmon"
        | DarkSeaGreen -> "DarkSeaGreen"
        | DarkSlateBlue -> "DarkSlateBlue"
        | DarkSlateGray -> "DarkSlateGray"
        | DarkSlateGrey -> "DarkSlateGrey"
        | DarkTurquoise -> "DarkTurquoise"
        | DarkViolet -> "DarkViolet"
        | DeepPink -> "DeepPink"
        | DeepSkyBlue -> "DeepSkyBlue"
        | DimGray -> "DimGray"
        | DimGrey -> "DimGrey"
        | DodgerBlue -> "DodgerBlue"
        | FireBrick -> "FireBrick"
        | FloralWhite -> "FloralWhite"
        | ForestGreen -> "ForestGreen"
        | Fuchsia -> "Fuchsia"
        | Gainsboro -> "Gainsboro"
        | GhostWhite -> "GhostWhite"
        | Gold -> "Gold"
        | GoldenRod -> "GoldenRod"
        | Gray -> "Gray"
        | Grey -> "Grey"
        | Green -> "Green"
        | GreenYellow -> "GreenYellow"
        | HoneyDew -> "HoneyDew"
        | HotPink -> "HotPink"
        | IndianRed -> "IndianRed"
        | Indigo -> "Indigo"
        | Ivory -> "Ivory"
        | Khaki -> "Khaki"
        | Lavender -> "Lavender"
        | LavenderBlush -> "LavenderBlush"
        | LawnGreen -> "LawnGreen"
        | LemonChiffon -> "LemonChiffon"
        | LightBlue -> "LightBlue"
        | LightCoral -> "LightCoral"
        | LightCyan -> "LightCyan"
        | LightGoldenRodYellow -> "LightGoldenRodYellow"
        | LightGray -> "LightGray"
        | LightGreen -> "LightGreen"
        | LightGrey -> "LightGrey"
        | LightPink -> "LightPink"
        | LightSalmon -> "LightSalmon"
        | LightSeaGreen -> "LightSeaGreen"
        | LightSkyBlue -> "LightSkyBlue"
        | LightSlateGray -> "LightSlateGray"
        | LightSlateGrey -> "LightSlateGrey"
        | LightSteelBlue -> "LightSteelBlue"
        | LightYellow -> "LightYellow"
        | Lime -> "Lime"
        | LimeGreen -> "LimeGreen"
        | Linen -> "Linen"
        | Magenta -> "Magenta"
        | Maroon -> "Maroon"
        | MediumAquamarine -> "MediumAquamarine"
        | MediumBlue -> "MediumBlue"
        | MediumOrchid -> "MediumOrchid"
        | MediumPurple -> "MediumPurple"
        | MediumSeaGreen -> "MediumSeaGreen"
        | MediumSlateBlue -> "MediumSlateBlue"
        | MediumSpringGreen -> "MediumSpringGreen"
        | MediumTurquoise -> "MediumTurquoise"
        | MediumVioletRed -> "MediumVioletRed"
        | MidnightBlue -> "MidnightBlue"
        | MintCream -> "MintCream"
        | MistyRose -> "MistyRose"
        | Moccasin -> "Moccasin"
        | NavajoWhite -> "NavajoWhite"
        | Navy -> "Navy"
        | OldLace -> "OldLace"
        | Olive -> "Olive"
        | OliveDrab -> "OliveDrab"
        | Orange -> "Orange"
        | OrangeRed -> "OrangeRed"
        | Orchid -> "Orchid"
        | PaleGoldenRod -> "PaleGoldenRod"
        | PaleGreen -> "PaleGreen"
        | PaleTurquoise -> "PaleTurquoise"
        | PaleVioletRed -> "PaleVioletRed"
        | PapayaWhip -> "PapayaWhip"
        | PeachPuff -> "PeachPuff"
        | Peru -> "Peru"
        | Pink -> "Pink"
        | Plum -> "Plum"
        | PowderBlue -> "PowderBlue"
        | Purple -> "Purple"
        | Red -> "Red"
        | RosyBrown -> "RosyBrown"
        | RoyalBlue -> "RoyalBlue"
        | SaddleBrown -> "SaddleBrown"
        | Salmon -> "Salmon"
        | SandyBrown -> "SandyBrown"
        | SeaGreen -> "SeaGreen"
        | SeaShell -> "SeaShell"
        | Sienna -> "Sienna"
        | Silver -> "Silver"
        | SkyBlue -> "SkyBlue"
        | SlateBlue -> "SlateBlue"
        | SlateGray -> "SlateGray"
        | SlateGrey -> "SlateGrey"
        | Snow -> "Snow"
        | SpringGreen -> "SpringGreen"
        | SteelBlue -> "SteelBlue"
        | Tan -> "Tan"
        | Teal -> "Teal"
        | Thistle -> "Thistle"
        | Tomato -> "Tomato"
        | Turquoise -> "Turquoise"
        | Violet -> "Violet"
        | Wheat -> "Wheat"
        | White -> "White"
        | WhiteSmoke -> "WhiteSmoke"
        | Yellow -> "Yellow"
        | YellowGreen -> "YellowGreen"

type Color =
    | Name of Colors
    | SmallHex of int16  // #rgb
    | Hex of int  // #rrggbb
    | Values of byte * byte * byte // 0 - 255
    | Percents of float * float * float // 0.0% - 100.0%
with
    override this.ToString() =
        match this with
            | Name n -> n.ToString().ToLower()
            | SmallHex sh -> String.Format("0x{0:x}", sh)
            | Hex h -> String.Format("0x{0:x}", h)
            | Values (r, g, b) -> "rgb(" + string r + "," + string g + "," + string b + ")"
            | Percents (r, g, b) -> "rgb(" + string r + "%," + string g + "%," + string b + "%)"

module Color =

    let ofName =
        Name

    let ofSmallHex =
        SmallHex

    let ofHex =
        Hex

    let ofValues =
        Values

    let ofPercents =
        Percents

    let toString (color :Color) =
        color.ToString()
