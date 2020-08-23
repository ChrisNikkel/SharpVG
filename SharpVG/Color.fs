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

    let aliceBlue = ofName AliceBlue

    let antiqueWhite = ofName AntiqueWhite

    let aqua = ofName Aqua

    let aquamarine = ofName Aquamarine

    let azure = ofName Azure

    let beige = ofName Beige

    let bisque = ofName Bisque

    let black = ofName Black

    let blanchedAlmond = ofName BlanchedAlmond

    let blue = ofName Blue

    let blueViolet = ofName BlueViolet

    let brown = ofName Brown

    let burlyWood = ofName BurlyWood

    let cadetBlue = ofName CadetBlue

    let chartreuse = ofName Chartreuse

    let chocolate = ofName Chocolate

    let coral = ofName Coral

    let cornflowerBlue = ofName CornflowerBlue

    let cornSilk = ofName CornSilk

    let crimson = ofName Crimson

    let cyan = ofName Cyan

    let darkBlue = ofName DarkBlue

    let darkCyan = ofName DarkCyan

    let darkGoldenRod = ofName DarkGoldenRod

    let darkGray = ofName DarkGray

    let darkGreen = ofName DarkGreen

    let darkGrey = ofName DarkGrey

    let darkKhaki = ofName DarkKhaki

    let darkMagenta = ofName DarkMagenta

    let darkOliveGreen = ofName DarkOliveGreen

    let darkOrange = ofName DarkOrange

    let darkOrchid = ofName DarkOrchid

    let darkRed = ofName DarkRed

    let darkSalmon = ofName DarkSalmon

    let darkSeaGreen = ofName DarkSeaGreen

    let darkSlateBlue = ofName DarkSlateBlue

    let darkSlateGray = ofName DarkSlateGray

    let darkSlateGrey = ofName DarkSlateGrey

    let darkTurquoise = ofName DarkTurquoise

    let darkViolet = ofName DarkViolet

    let deepPink = ofName DeepPink

    let deepSkyBlue = ofName DeepSkyBlue

    let dimGray = ofName DimGray

    let dimGrey = ofName DimGrey

    let dodgerBlue = ofName DodgerBlue

    let fireBrick = ofName FireBrick

    let floralWhite = ofName FloralWhite

    let forestGreen = ofName ForestGreen

    let fuchsia = ofName Fuchsia

    let gainsboro = ofName Gainsboro

    let ghostWhite = ofName GhostWhite

    let gold = ofName Gold

    let goldenRod = ofName GoldenRod

    let gray = ofName Gray

    let grey = ofName Grey

    let green = ofName Green

    let greenYellow = ofName GreenYellow

    let honeyDew = ofName HoneyDew

    let hotPink = ofName HotPink

    let indianRed = ofName IndianRed

    let indigo = ofName Indigo

    let ivory = ofName Ivory

    let khaki = ofName Khaki

    let lavender = ofName Lavender

    let lavenderBlush = ofName LavenderBlush

    let lawnGreen = ofName LawnGreen

    let lemonChiffon = ofName LemonChiffon

    let lightBlue = ofName LightBlue

    let lightCoral = ofName LightCoral

    let lightCyan = ofName LightCyan

    let lightGoldenRodYellow = ofName LightGoldenRodYellow

    let lightGray = ofName LightGray

    let lightGreen = ofName LightGreen

    let lightGrey = ofName LightGrey

    let lightPink = ofName LightPink

    let lightSalmon = ofName LightSalmon

    let lightSeaGreen = ofName LightSeaGreen

    let lightSkyBlue = ofName LightSkyBlue

    let lightSlateGray = ofName LightSlateGray

    let lightSlateGrey = ofName LightSlateGrey

    let lightSteelBlue = ofName LightSteelBlue

    let lightYellow = ofName LightYellow

    let lime = ofName Lime

    let limeGreen = ofName LimeGreen

    let linen = ofName Linen

    let magenta = ofName Magenta

    let maroon = ofName Maroon

    let mediumAquamarine = ofName MediumAquamarine

    let mediumBlue = ofName MediumBlue

    let mediumOrchid = ofName MediumOrchid

    let mediumPurple = ofName MediumPurple

    let mediumSeaGreen = ofName MediumSeaGreen

    let mediumSlateBlue = ofName MediumSlateBlue

    let mediumSpringGreen = ofName MediumSpringGreen

    let mediumTurquoise = ofName MediumTurquoise

    let mediumVioletRed = ofName MediumVioletRed

    let midnightBlue = ofName MidnightBlue

    let mintCream = ofName MintCream

    let mistyRose = ofName MistyRose

    let moccasin = ofName Moccasin

    let navajoWhite = ofName NavajoWhite

    let navy = ofName Navy

    let oldLace = ofName OldLace

    let olive = ofName Olive

    let oliveDrab = ofName OliveDrab

    let orange = ofName Orange

    let orangeRed = ofName OrangeRed

    let orchid = ofName Orchid

    let paleGoldenRod = ofName PaleGoldenRod

    let paleGreen = ofName PaleGreen

    let paleTurquoise = ofName PaleTurquoise

    let paleVioletRed = ofName PaleVioletRed

    let papayaWhip = ofName PapayaWhip

    let peachPuff = ofName PeachPuff

    let peru = ofName Peru

    let pink = ofName Pink

    let plum = ofName Plum

    let powderBlue = ofName PowderBlue

    let purple = ofName Purple

    let red = ofName Red

    let rosyBrown = ofName RosyBrown

    let royalBlue = ofName RoyalBlue

    let saddleBrown = ofName SaddleBrown

    let salmon = ofName Salmon

    let sandyBrown = ofName SandyBrown

    let seaGreen = ofName SeaGreen

    let seaShell = ofName SeaShell

    let sienna = ofName Sienna

    let silver = ofName Silver

    let skyBlue = ofName SkyBlue

    let slateBlue = ofName SlateBlue

    let slateGray = ofName SlateGray

    let slateGrey = ofName SlateGrey

    let snow = ofName Snow

    let springGreen = ofName SpringGreen

    let steelBlue = ofName SteelBlue

    let tan = ofName Tan

    let teal = ofName Teal

    let thistle = ofName Thistle

    let tomato = ofName Tomato

    let turquoise = ofName Turquoise

    let violet = ofName Violet

    let wheat = ofName Wheat

    let white = ofName White

    let whiteSmoke = ofName WhiteSmoke

    let yellow = ofName Yellow

    let yellowGreen = ofName YellowGreen
