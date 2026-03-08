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
        Assert.Contains("writing-mode=\"vertical-left\"", result)