namespace SharpVG

type colors =
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

type color =
    | Name of colors
    | SmallHex of int16  // #rgb
    | Hex of int  // #rrggbb
    | Values of byte * byte * byte // 0 - 255
    | Percents of double * double * double // 0.0% - 100.0%

module Color =
    open System

    let initWithName =
        Name

    let initWithSmallHex =
        SmallHex

    let initWithHex =
        Hex

    let initWithValues =
        Values

    let initWithPercents =
        Percents

    let toString color =
        match color with
            | Name n -> Enum.GetName(typeof<colors>, n).ToLower()
            | SmallHex sh -> String.Format("0x{0:x}", sh)
            | Hex h -> String.Format("0x{0:x}", h)
            | Values (r, g, b) -> "(" + string r + ", " + string g + ", " + string b + ")"
            | Percents (r, g, b) -> "(" + string r + "%, " + string g + "%, " + string b + "%)"
