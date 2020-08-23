namespace SharpVG

type Pen =
    {
        Color: Color;
        Opacity: float option;
        Width: Length option;
    }

module Pen =
    let create color =
        { Color = color; Opacity = None; Width = None }

    let createWithOpacity color opacity =
        { Color = color; Opacity = Some(opacity); Width = None }

    let createWithWidth color width =
        { Color = color; Opacity = None; Width = Some(width) }

    let createWithOpacityAndWidth color opacity width =
        { Color = color; Opacity = Some(opacity); Width = Some(width) }

    let withOpacity opacity pen =
        { pen with Opacity = Some(opacity) }

    let withWidth width (pen : Pen) =
        { pen with Width = Some(width) }

    let aliceBlue = Color.ofName AliceBlue

    let antiqueWhite = Color.ofName AntiqueWhite

    let aqua = Color.ofName Aqua

    let aquamarine = Color.ofName Aquamarine

    let azure = Color.ofName Azure

    let beige = Color.ofName Beige

    let bisque = Color.ofName Bisque

    let black = Color.ofName Black

    let blanchedAlmond = Color.ofName BlanchedAlmond

    let blue = Color.ofName Blue

    let blueViolet = Color.ofName BlueViolet

    let brown = Color.ofName Brown

    let burlyWood = Color.ofName BurlyWood

    let cadetBlue = Color.ofName CadetBlue

    let chartreuse = Color.ofName Chartreuse

    let chocolate = Color.ofName Chocolate

    let coral = Color.ofName Coral

    let cornflowerBlue = Color.ofName CornflowerBlue

    let cornSilk = Color.ofName CornSilk

    let crimson = Color.ofName Crimson

    let cyan = Color.ofName Cyan

    let darkBlue = Color.ofName DarkBlue

    let darkCyan = Color.ofName DarkCyan

    let darkGoldenRod = Color.ofName DarkGoldenRod

    let darkGray = Color.ofName DarkGray

    let darkGreen = Color.ofName DarkGreen

    let darkGrey = Color.ofName DarkGrey

    let darkKhaki = Color.ofName DarkKhaki

    let darkMagenta = Color.ofName DarkMagenta

    let darkOliveGreen = Color.ofName DarkOliveGreen

    let darkOrange = Color.ofName DarkOrange

    let darkOrchid = Color.ofName DarkOrchid

    let darkRed = Color.ofName DarkRed

    let darkSalmon = Color.ofName DarkSalmon

    let darkSeaGreen = Color.ofName DarkSeaGreen

    let darkSlateBlue = Color.ofName DarkSlateBlue

    let darkSlateGray = Color.ofName DarkSlateGray

    let darkSlateGrey = Color.ofName DarkSlateGrey

    let darkTurquoise = Color.ofName DarkTurquoise

    let darkViolet = Color.ofName DarkViolet

    let deepPink = Color.ofName DeepPink

    let deepSkyBlue = Color.ofName DeepSkyBlue

    let dimGray = Color.ofName DimGray

    let dimGrey = Color.ofName DimGrey

    let dodgerBlue = Color.ofName DodgerBlue

    let fireBrick = Color.ofName FireBrick

    let floralWhite = Color.ofName FloralWhite

    let forestGreen = Color.ofName ForestGreen

    let fuchsia = Color.ofName Fuchsia

    let gainsboro = Color.ofName Gainsboro

    let ghostWhite = Color.ofName GhostWhite

    let gold = Color.ofName Gold

    let goldenRod = Color.ofName GoldenRod

    let gray = Color.ofName Gray

    let grey = Color.ofName Grey

    let green = Color.ofName Green

    let greenYellow = Color.ofName GreenYellow

    let honeyDew = Color.ofName HoneyDew

    let hotPink = Color.ofName HotPink

    let indianRed = Color.ofName IndianRed

    let indigo = Color.ofName Indigo

    let ivory = Color.ofName Ivory

    let khaki = Color.ofName Khaki

    let lavender = Color.ofName Lavender

    let lavenderBlush = Color.ofName LavenderBlush

    let lawnGreen = Color.ofName LawnGreen

    let lemonChiffon = Color.ofName LemonChiffon

    let lightBlue = Color.ofName LightBlue

    let lightCoral = Color.ofName LightCoral

    let lightCyan = Color.ofName LightCyan

    let lightGoldenRodYellow = Color.ofName LightGoldenRodYellow

    let lightGray = Color.ofName LightGray

    let lightGreen = Color.ofName LightGreen

    let lightGrey = Color.ofName LightGrey

    let lightPink = Color.ofName LightPink

    let lightSalmon = Color.ofName LightSalmon

    let lightSeaGreen = Color.ofName LightSeaGreen

    let lightSkyBlue = Color.ofName LightSkyBlue

    let lightSlateGray = Color.ofName LightSlateGray

    let lightSlateGrey = Color.ofName LightSlateGrey

    let lightSteelBlue = Color.ofName LightSteelBlue

    let lightYellow = Color.ofName LightYellow

    let lime = Color.ofName Lime

    let limeGreen = Color.ofName LimeGreen

    let linen = Color.ofName Linen

    let magenta = Color.ofName Magenta

    let maroon = Color.ofName Maroon

    let mediumAquamarine = Color.ofName MediumAquamarine

    let mediumBlue = Color.ofName MediumBlue

    let mediumOrchid = Color.ofName MediumOrchid

    let mediumPurple = Color.ofName MediumPurple

    let mediumSeaGreen = Color.ofName MediumSeaGreen

    let mediumSlateBlue = Color.ofName MediumSlateBlue

    let mediumSpringGreen = Color.ofName MediumSpringGreen

    let mediumTurquoise = Color.ofName MediumTurquoise

    let mediumVioletRed = Color.ofName MediumVioletRed

    let midnightBlue = Color.ofName MidnightBlue

    let mintCream = Color.ofName MintCream

    let mistyRose = Color.ofName MistyRose

    let moccasin = Color.ofName Moccasin

    let navajoWhite = Color.ofName NavajoWhite

    let navy = Color.ofName Navy

    let oldLace = Color.ofName OldLace

    let olive = Color.ofName Olive

    let oliveDrab = Color.ofName OliveDrab

    let orange = Color.ofName Orange

    let orangeRed = Color.ofName OrangeRed

    let orchid = Color.ofName Orchid

    let paleGoldenRod = Color.ofName PaleGoldenRod

    let paleGreen = Color.ofName PaleGreen

    let paleTurquoise = Color.ofName PaleTurquoise

    let paleVioletRed = Color.ofName PaleVioletRed

    let papayaWhip = Color.ofName PapayaWhip

    let peachPuff = Color.ofName PeachPuff

    let peru = Color.ofName Peru

    let pink = Color.ofName Pink

    let plum = Color.ofName Plum

    let powderBlue = Color.ofName PowderBlue

    let purple = Color.ofName Purple

    let red = Color.ofName Red

    let rosyBrown = Color.ofName RosyBrown

    let royalBlue = Color.ofName RoyalBlue

    let saddleBrown = Color.ofName SaddleBrown

    let salmon = Color.ofName Salmon

    let sandyBrown = Color.ofName SandyBrown

    let seaGreen = Color.ofName SeaGreen

    let seaShell = Color.ofName SeaShell

    let sienna = Color.ofName Sienna

    let silver = Color.ofName Silver

    let skyBlue = Color.ofName SkyBlue

    let slateBlue = Color.ofName SlateBlue

    let slateGray = Color.ofName SlateGray

    let slateGrey = Color.ofName SlateGrey

    let snow = Color.ofName Snow

    let springGreen = Color.ofName SpringGreen

    let steelBlue = Color.ofName SteelBlue

    let tan = Color.ofName Tan

    let teal = Color.ofName Teal

    let thistle = Color.ofName Thistle

    let tomato = Color.ofName Tomato

    let turquoise = Color.ofName Turquoise

    let violet = Color.ofName Violet

    let wheat = Color.ofName Wheat

    let white = Color.ofName White

    let whiteSmoke = Color.ofName WhiteSmoke

    let yellow = Color.ofName Yellow

    let yellowGreen = Color.ofName YellowGreen
