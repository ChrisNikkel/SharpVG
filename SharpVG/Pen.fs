namespace SharpVG

type Pen =
    {
        Color: Color;
        Opacity: float;
        Width: Length;
    }

module Pen =
    let private defaultWidth = Length.one
    let private defaultOpacity = 1.0

    let create color =
        { Color = color; Opacity = defaultOpacity; Width = defaultWidth }

    let createWithOpacity color opacity =
        { Color = color; Opacity = opacity; Width = defaultWidth }

    let createWithWidth color width =
        { Color = color; Opacity = defaultOpacity; Width = width }

    let createWithOpacityAndWidth color opacity width =
        { Color = color; Opacity = opacity; Width = width }

    let withOpacity opacity pen =
        { pen with Opacity = opacity }

    let withWidth width (pen : Pen) =
        { pen with Width = width }

    let aliceBlue = Color.ofName AliceBlue |> create

    let antiqueWhite = Color.ofName AntiqueWhite |> create

    let aqua = Color.ofName Aqua |> create

    let aquamarine = Color.ofName Aquamarine |> create

    let azure = Color.ofName Azure |> create

    let beige = Color.ofName Beige |> create

    let bisque = Color.ofName Bisque |> create

    let black = Color.ofName Black |> create

    let blanchedAlmond = Color.ofName BlanchedAlmond |> create

    let blue = Color.ofName Blue |> create

    let blueViolet = Color.ofName BlueViolet |> create

    let brown = Color.ofName Brown |> create

    let burlyWood = Color.ofName BurlyWood |> create

    let cadetBlue = Color.ofName CadetBlue |> create

    let chartreuse = Color.ofName Chartreuse |> create

    let chocolate = Color.ofName Chocolate |> create

    let coral = Color.ofName Coral |> create

    let cornflowerBlue = Color.ofName CornflowerBlue |> create

    let cornSilk = Color.ofName CornSilk |> create

    let crimson = Color.ofName Crimson |> create

    let cyan = Color.ofName Cyan |> create

    let darkBlue = Color.ofName DarkBlue |> create

    let darkCyan = Color.ofName DarkCyan |> create

    let darkGoldenRod = Color.ofName DarkGoldenRod |> create

    let darkGray = Color.ofName DarkGray |> create

    let darkGreen = Color.ofName DarkGreen |> create

    let darkGrey = Color.ofName DarkGrey |> create

    let darkKhaki = Color.ofName DarkKhaki |> create

    let darkMagenta = Color.ofName DarkMagenta |> create

    let darkOliveGreen = Color.ofName DarkOliveGreen |> create

    let darkOrange = Color.ofName DarkOrange |> create

    let darkOrchid = Color.ofName DarkOrchid |> create

    let darkRed = Color.ofName DarkRed |> create

    let darkSalmon = Color.ofName DarkSalmon |> create

    let darkSeaGreen = Color.ofName DarkSeaGreen |> create

    let darkSlateBlue = Color.ofName DarkSlateBlue |> create

    let darkSlateGray = Color.ofName DarkSlateGray |> create

    let darkSlateGrey = Color.ofName DarkSlateGrey |> create

    let darkTurquoise = Color.ofName DarkTurquoise |> create

    let darkViolet = Color.ofName DarkViolet |> create

    let deepPink = Color.ofName DeepPink |> create

    let deepSkyBlue = Color.ofName DeepSkyBlue |> create

    let dimGray = Color.ofName DimGray |> create

    let dimGrey = Color.ofName DimGrey |> create

    let dodgerBlue = Color.ofName DodgerBlue |> create

    let fireBrick = Color.ofName FireBrick |> create

    let floralWhite = Color.ofName FloralWhite |> create

    let forestGreen = Color.ofName ForestGreen |> create

    let fuchsia = Color.ofName Fuchsia |> create

    let gainsboro = Color.ofName Gainsboro |> create

    let ghostWhite = Color.ofName GhostWhite |> create

    let gold = Color.ofName Gold |> create

    let goldenRod = Color.ofName GoldenRod |> create

    let gray = Color.ofName Gray |> create

    let grey = Color.ofName Grey |> create

    let green = Color.ofName Green |> create

    let greenYellow = Color.ofName GreenYellow |> create

    let honeyDew = Color.ofName HoneyDew |> create

    let hotPink = Color.ofName HotPink |> create

    let indianRed = Color.ofName IndianRed |> create

    let indigo = Color.ofName Indigo |> create

    let ivory = Color.ofName Ivory |> create

    let khaki = Color.ofName Khaki |> create

    let lavender = Color.ofName Lavender |> create

    let lavenderBlush = Color.ofName LavenderBlush |> create

    let lawnGreen = Color.ofName LawnGreen |> create

    let lemonChiffon = Color.ofName LemonChiffon |> create

    let lightBlue = Color.ofName LightBlue |> create

    let lightCoral = Color.ofName LightCoral |> create

    let lightCyan = Color.ofName LightCyan |> create

    let lightGoldenRodYellow = Color.ofName LightGoldenRodYellow |> create

    let lightGray = Color.ofName LightGray |> create

    let lightGreen = Color.ofName LightGreen |> create

    let lightGrey = Color.ofName LightGrey |> create

    let lightPink = Color.ofName LightPink |> create

    let lightSalmon = Color.ofName LightSalmon |> create

    let lightSeaGreen = Color.ofName LightSeaGreen |> create

    let lightSkyBlue = Color.ofName LightSkyBlue |> create

    let lightSlateGray = Color.ofName LightSlateGray |> create

    let lightSlateGrey = Color.ofName LightSlateGrey |> create

    let lightSteelBlue = Color.ofName LightSteelBlue |> create

    let lightYellow = Color.ofName LightYellow |> create

    let lime = Color.ofName Lime |> create

    let limeGreen = Color.ofName LimeGreen |> create

    let linen = Color.ofName Linen |> create

    let magenta = Color.ofName Magenta |> create

    let maroon = Color.ofName Maroon |> create

    let mediumAquamarine = Color.ofName MediumAquamarine |> create

    let mediumBlue = Color.ofName MediumBlue |> create

    let mediumOrchid = Color.ofName MediumOrchid |> create

    let mediumPurple = Color.ofName MediumPurple |> create

    let mediumSeaGreen = Color.ofName MediumSeaGreen |> create

    let mediumSlateBlue = Color.ofName MediumSlateBlue |> create

    let mediumSpringGreen = Color.ofName MediumSpringGreen |> create

    let mediumTurquoise = Color.ofName MediumTurquoise |> create

    let mediumVioletRed = Color.ofName MediumVioletRed |> create

    let midnightBlue = Color.ofName MidnightBlue |> create

    let mintCream = Color.ofName MintCream |> create

    let mistyRose = Color.ofName MistyRose |> create

    let moccasin = Color.ofName Moccasin |> create

    let navajoWhite = Color.ofName NavajoWhite |> create

    let navy = Color.ofName Navy |> create

    let oldLace = Color.ofName OldLace |> create

    let olive = Color.ofName Olive |> create

    let oliveDrab = Color.ofName OliveDrab |> create

    let orange = Color.ofName Orange |> create

    let orangeRed = Color.ofName OrangeRed |> create

    let orchid = Color.ofName Orchid |> create

    let paleGoldenRod = Color.ofName PaleGoldenRod |> create

    let paleGreen = Color.ofName PaleGreen |> create

    let paleTurquoise = Color.ofName PaleTurquoise |> create

    let paleVioletRed = Color.ofName PaleVioletRed |> create

    let papayaWhip = Color.ofName PapayaWhip |> create

    let peachPuff = Color.ofName PeachPuff |> create

    let peru = Color.ofName Peru |> create

    let pink = Color.ofName Pink |> create

    let plum = Color.ofName Plum |> create

    let powderBlue = Color.ofName PowderBlue |> create

    let purple = Color.ofName Purple |> create

    let red = Color.ofName Red |> create

    let rosyBrown = Color.ofName RosyBrown |> create

    let royalBlue = Color.ofName RoyalBlue |> create

    let saddleBrown = Color.ofName SaddleBrown |> create

    let salmon = Color.ofName Salmon |> create

    let sandyBrown = Color.ofName SandyBrown |> create

    let seaGreen = Color.ofName SeaGreen |> create

    let seaShell = Color.ofName SeaShell |> create

    let sienna = Color.ofName Sienna |> create

    let silver = Color.ofName Silver |> create

    let skyBlue = Color.ofName SkyBlue |> create

    let slateBlue = Color.ofName SlateBlue |> create

    let slateGray = Color.ofName SlateGray |> create

    let slateGrey = Color.ofName SlateGrey |> create

    let snow = Color.ofName Snow |> create

    let springGreen = Color.ofName SpringGreen |> create

    let steelBlue = Color.ofName SteelBlue |> create

    let tan = Color.ofName Tan |> create

    let teal = Color.ofName Teal |> create

    let thistle = Color.ofName Thistle |> create

    let tomato = Color.ofName Tomato |> create

    let turquoise = Color.ofName Turquoise |> create

    let violet = Color.ofName Violet |> create

    let wheat = Color.ofName Wheat |> create

    let white = Color.ofName White |> create

    let whiteSmoke = Color.ofName WhiteSmoke |> create

    let yellow = Color.ofName Yellow |> create

    let yellowGreen = Color.ofName YellowGreen |> create
