namespace SharpVG.Tests

open SharpVG
open Xunit

module TestText =

    [<Fact>]
    let ``create text`` () =
        Assert.Equal("<text x=\"0\" y=\"0\">Hello World!</text>", Text.create Point.origin "Hello World!" |> Text.toString)

    [<Fact>]
    let ``text with font family and size`` () =
        let position = Point.ofInts (5, 5)
        let result = Text.create position "Hello" |> Text.withFont "Arial" 12.0 |> Text.toString
        Assert.Contains("font-family=\"Arial\"", result)
        Assert.Contains("font-size=\"12\"", result)
        Assert.Contains(">Hello</text>", result)

    [<Fact>]
    let ``text withFontFamily only`` () =
        let position = Point.ofInts (0, 20)
        let result = Text.create position "Hi" |> Text.withFontFamily "Helvetica" |> Text.toString
        Assert.Contains("font-family=\"Helvetica\"", result)

    [<Fact>]
    let ``text withFontSize only`` () =
        let position = Point.ofInts (0, 20)
        let result = Text.create position "Hi" |> Text.withFontSize 16.0 |> Text.toString
        Assert.Contains("font-size=\"16\"", result)

    [<Fact>]
    let ``text anchor start`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "Left" |> Text.withAnchor Start |> Text.toString
        Assert.Contains("text-anchor=\"start\"", result)

    [<Fact>]
    let ``text anchor middle`` () =
        let position = Point.ofInts (50, 50)
        let result = Text.create position "Center" |> Text.withAnchor Middle |> Text.toString
        Assert.Contains("text-anchor=\"middle\"", result)

    [<Fact>]
    let ``text anchor end`` () =
        let position = Point.ofInts (100, 50)
        let result = Text.create position "Right" |> Text.withAnchor End |> Text.toString
        Assert.Contains("text-anchor=\"end\"", result)

    [<Fact>]
    let ``text anchor inherit`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "Inherited" |> Text.withAnchor Inherit |> Text.toString
        Assert.Contains("text-anchor=\"inherit\"", result)

    [<Fact>]
    let ``text decoration underline`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "Underlined" |> Text.withDecoration Underline |> Text.toString
        Assert.Contains("text-decoration=\"underline\"", result)

    [<Fact>]
    let ``text decoration strikethrough`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "Strike" |> Text.withDecoration StrikeThrough |> Text.toString
        Assert.Contains("text-decoration=\"line-through\"", result)

    [<Fact>]
    let ``text with letter spacing`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "Spaced" |> Text.withLetterSpacing 2.5 |> Text.toString
        Assert.Contains("letter-spacing=\"2.5\"", result)

    [<Fact>]
    let ``text writing mode horizontal`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "H" |> Text.withWritingMode HorizontalTopToBottom |> Text.toString
        Assert.Contains("writing-mode=\"horizontal-tb\"", result)

    [<Fact>]
    let ``text writing mode vertical right to left`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "V" |> Text.withWritingMode VerticalRightToLeft |> Text.toString
        Assert.Contains("writing-mode=\"vertical-rl\"", result)

    [<Fact>]
    let ``text writing mode vertical left to right`` () =
        let position = Point.ofInts (10, 10)
        let result = Text.create position "V" |> Text.withWritingMode VerticalLeftToRight |> Text.toString
        Assert.Contains("writing-mode=\"vertical-lr\"", result)

    [<Fact>]
    let ``text withFontWeight normal`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontWeight NormalWeight |> Text.toString
        Assert.Contains("font-weight=\"normal\"", result)

    [<Fact>]
    let ``text withFontWeight bold`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontWeight BoldWeight |> Text.toString
        Assert.Contains("font-weight=\"bold\"", result)

    [<Fact>]
    let ``text withFontWeight bolder`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontWeight BolderWeight |> Text.toString
        Assert.Contains("font-weight=\"bolder\"", result)

    [<Fact>]
    let ``text withFontWeight lighter`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontWeight LighterWeight |> Text.toString
        Assert.Contains("font-weight=\"lighter\"", result)

    [<Fact>]
    let ``text withFontWeight numeric 700`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontWeight (NumericWeight 700) |> Text.toString
        Assert.Contains("font-weight=\"700\"", result)

    [<Fact>]
    let ``text withFontStyle normal`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontStyle NormalStyle |> Text.toString
        Assert.Contains("font-style=\"normal\"", result)

    [<Fact>]
    let ``text withFontStyle italic`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontStyle ItalicStyle |> Text.toString
        Assert.Contains("font-style=\"italic\"", result)

    [<Fact>]
    let ``text withFontStyle oblique`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontStyle ObliqueStyle |> Text.toString
        Assert.Contains("font-style=\"oblique\"", result)

    [<Fact>]
    let ``text withFontVariant normal`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontVariant NormalVariant |> Text.toString
        Assert.Contains("font-variant=\"normal\"", result)

    [<Fact>]
    let ``text withFontVariant small-caps`` () =
        let result = Text.create Point.origin "Hi" |> Text.withFontVariant SmallCaps |> Text.toString
        Assert.Contains("font-variant=\"small-caps\"", result)

    [<Fact>]
    let ``text withBaseline auto`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline AutoBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"auto\"", result)

    [<Fact>]
    let ``text withBaseline middle`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline MiddleBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"middle\"", result)

    [<Fact>]
    let ``text withAlignmentBaseline hanging`` () =
        let result = Text.create Point.origin "Hi" |> Text.withAlignmentBaseline HangingBaseline |> Text.toString
        Assert.Contains("alignment-baseline=\"hanging\"", result)

    [<Fact>]
    let ``text withBaseline central`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline CentralBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"central\"", result)

    [<Fact>]
    let ``text withBaseline alphabetic`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline AlphabeticBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"alphabetic\"", result)

    [<Fact>]
    let ``text withBaseline ideographic`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline IdeographicBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"ideographic\"", result)

    [<Fact>]
    let ``text withBaseline text-bottom`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline TextBottomBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"text-bottom\"", result)

    [<Fact>]
    let ``text withBaseline text-top`` () =
        let result = Text.create Point.origin "Hi" |> Text.withBaseline TextTopBaseline |> Text.toString
        Assert.Contains("dominant-baseline=\"text-top\"", result)

    [<Fact>]
    let ``text withWordSpacing`` () =
        let result = Text.create Point.origin "Hi" |> Text.withWordSpacing 4.0 |> Text.toString
        Assert.Contains("word-spacing=\"4\"", result)

    [<Fact>]
    let ``text withTextLength`` () =
        let result = Text.create Point.origin "Hi" |> Text.withTextLength (Length.ofInt 100) |> Text.toString
        Assert.Contains("textLength=\"100\"", result)

    [<Fact>]
    let ``text withLengthAdjust spacing`` () =
        let result = Text.create Point.origin "Hi" |> Text.withLengthAdjust Spacing |> Text.toString
        Assert.Contains("lengthAdjust=\"spacing\"", result)

    [<Fact>]
    let ``text withLengthAdjust spacingAndGlyphs`` () =
        let result = Text.create Point.origin "Hi" |> Text.withLengthAdjust SpacingAndGlyphs |> Text.toString
        Assert.Contains("lengthAdjust=\"spacingAndGlyphs\"", result)

    [<Fact>]
    let ``TSpan create basic`` () =
        let result = TSpan.create "hello" |> TSpan.toString
        Assert.Equal("<tspan>hello</tspan>", result)

    [<Fact>]
    let ``TSpan withPosition`` () =
        let result = TSpan.create "hello" |> TSpan.withPosition (Point.ofInts (10, 20)) |> TSpan.toString
        Assert.Contains("x=\"10\"", result)
        Assert.Contains("y=\"20\"", result)

    [<Fact>]
    let ``TSpan withOffset`` () =
        let result = TSpan.create "hello" |> TSpan.withOffset (Point.ofInts (5, 0)) |> TSpan.toString
        Assert.Contains("dx=\"5\"", result)
        Assert.Contains("dy=\"0\"", result)

    [<Fact>]
    let ``TSpan withFontWeight bold`` () =
        let result = TSpan.create "hello" |> TSpan.withFontWeight BoldWeight |> TSpan.toString
        Assert.Contains("font-weight=\"bold\"", result)

    [<Fact>]
    let ``TSpan withFontStyle italic`` () =
        let result = TSpan.create "hello" |> TSpan.withFontStyle ItalicStyle |> TSpan.toString
        Assert.Contains("font-style=\"italic\"", result)

    [<Fact>]
    let ``TSpan withFontVariant small-caps`` () =
        let result = TSpan.create "hello" |> TSpan.withFontVariant SmallCaps |> TSpan.toString
        Assert.Contains("font-variant=\"small-caps\"", result)

    [<Fact>]
    let ``TSpan withFontFamily`` () =
        let result = TSpan.create "hello" |> TSpan.withFontFamily "Arial" |> TSpan.toString
        Assert.Contains("font-family=\"Arial\"", result)

    [<Fact>]
    let ``TSpan withFontSize`` () =
        let result = TSpan.create "hello" |> TSpan.withFontSize 14.0 |> TSpan.toString
        Assert.Contains("font-size=\"14\"", result)

    [<Fact>]
    let ``TSpan withBaseline middle`` () =
        let result = TSpan.create "hello" |> TSpan.withBaseline MiddleBaseline |> TSpan.toString
        Assert.Contains("dominant-baseline=\"middle\"", result)

    [<Fact>]
    let ``Text addSpan appends tspan to text`` () =
        let span = TSpan.create "world"
        let result = Text.create Point.origin "Hello " |> Text.addSpan span |> Text.toString
        Assert.Contains("<tspan>world</tspan>", result)
        Assert.Contains("Hello ", result)

    [<Fact>]
    let ``Text withSpans replaces spans`` () =
        let span1 = TSpan.create "first"
        let span2 = TSpan.create "second"
        let result = Text.create Point.origin "" |> Text.withSpans [span1; span2] |> Text.toString
        Assert.Contains("<tspan>first</tspan>", result)
        Assert.Contains("<tspan>second</tspan>", result)

    [<Fact>]
    let ``create textPath basic`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.toString
        Assert.Equal("<textPath href=\"#myPath\">Hello</textPath>", result)

    [<Fact>]
    let ``textPath withStartOffset percentage`` () =
        let startOffset = Length.ofPercent 50.0
        let result = TextPath.create "myPath" "Hello" |> TextPath.withStartOffset startOffset |> TextPath.toString
        Assert.Contains("startOffset=\"50%\"", result)

    [<Fact>]
    let ``textPath withStartOffset length`` () =
        let startOffset = Length.ofInt 20
        let result = TextPath.create "myPath" "Hello" |> TextPath.withStartOffset startOffset |> TextPath.toString
        Assert.Contains("startOffset=\"20\"", result)

    [<Fact>]
    let ``textPath withMethod align`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withMethod AlignMethod |> TextPath.toString
        Assert.Contains("method=\"align\"", result)

    [<Fact>]
    let ``textPath withMethod stretch`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withMethod StretchMethod |> TextPath.toString
        Assert.Contains("method=\"stretch\"", result)

    [<Fact>]
    let ``textPath withSpacing auto`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withSpacing AutoSpacing |> TextPath.toString
        Assert.Contains("spacing=\"auto\"", result)

    [<Fact>]
    let ``textPath withSpacing exact`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withSpacing ExactSpacing |> TextPath.toString
        Assert.Contains("spacing=\"exact\"", result)

    [<Fact>]
    let ``textPath withTextLength`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withTextLength (Length.ofInt 200) |> TextPath.toString
        Assert.Contains("textLength=\"200\"", result)

    [<Fact>]
    let ``textPath withLengthAdjust spacing`` () =
        let result = TextPath.create "myPath" "Hello" |> TextPath.withLengthAdjust Spacing |> TextPath.toString
        Assert.Contains("lengthAdjust=\"spacing\"", result)

    [<Fact>]
    let ``Text addTextPath`` () =
        let textPath = TextPath.create "curvePath" "Curved text"
        let result = Text.create Point.origin "" |> Text.addTextPath textPath |> Text.toString
        Assert.Contains("<textPath href=\"#curvePath\">Curved text</textPath>", result)

    [<Fact>]
    let ``Text withTextPaths`` () =
        let textPath = TextPath.create "myPath" "Along a path"
        let result = Text.create Point.origin "" |> Text.withTextPaths [textPath] |> Text.toString
        Assert.Contains("<textPath href=\"#myPath\">Along a path</textPath>", result)

    // Wiki: Text page — basic styled text example
    [<Fact>]
    let ``Text wiki - styled text with bold and italic`` () =
        let position = Point.ofInts (55, 45)
        let style = Style.create (Color.ofName Colors.White) (Color.ofName Colors.Black) (Length.ofInt 1) 1.0 1.0
        let result =
            Text.create position "Hello World!"
            |> Text.withFontWeight BoldWeight
            |> Text.withFontStyle ItalicStyle
            |> Element.createWithStyle style
            |> Element.toString
        Assert.Contains("<text", result)
        Assert.Contains("x=\"55\"", result)
        Assert.Contains("y=\"45\"", result)
        Assert.Contains("Hello World!", result)
        Assert.Contains("font-weight=\"bold\"", result)
        Assert.Contains("font-style=\"italic\"", result)

    // Wiki: Text page — mixed font styles with TSpan
    [<Fact>]
    let ``Text wiki - mixed styles with tspan`` () =
        let result =
            Text.create (Point.ofInts (10, 40)) "Hello "
            |> Text.addSpan (TSpan.create "world" |> TSpan.withFontWeight BoldWeight)
            |> Text.addSpan (TSpan.create "!" |> TSpan.withFontStyle ItalicStyle)
            |> Element.create
            |> Element.toString
        Assert.Contains("<text x=\"10\" y=\"40\">", result)
        Assert.Contains("Hello ", result)
        Assert.Contains("<tspan font-weight=\"bold\">world</tspan>", result)
        Assert.Contains("<tspan font-style=\"italic\">!</tspan>", result)