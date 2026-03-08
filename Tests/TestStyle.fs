namespace SharpVG.Tests

open SharpVG
open Xunit

module TestStyle =

    [<Fact>]
    let ``create style with fill`` () =
        Assert.Equal("fill:white", Style.empty |> Style.withFill (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke`` () =
        Assert.Equal("stroke:white", Style.empty |> Style.withStroke (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke width`` () =
        Assert.Equal("stroke:white;stroke-width:0", Style.empty |> Style.withStroke (Name Colors.White) |> Style.withStrokeWidth Length.empty |> Style.toString)

    [<Fact>]
    let ``create style with opacity`` () =
        Assert.Equal("opacity:0.5", Style.empty |> Style.withOpacity 0.5 |> Style.toString)

    [<Fact>]
    let ``create style with fill opacity`` () =
        Assert.Equal("fill-opacity:0.8", Style.empty |> Style.withFillOpacity 0.8 |> Style.toString)

    [<Fact>]
    let ``create style with pen and fill pen`` () =
        let strokeColor = Color.ofName Colors.Blue
        let fillColor = Color.ofName Colors.Cyan
        let penWidth = Length.ofInt 3
        let strokePen = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
        let fillPen = Pen.create fillColor
        let style = Style.createWithPen strokePen |> Style.withFillPen fillPen
        let result = style |> Style.toString
        Assert.Contains("stroke:blue", result)
        Assert.Contains("fill:cyan", result)
        Assert.Contains("stroke-width:3", result)

    [<Fact>]
    let ``named style isNamed returns true`` () =
        let style = Style.createWithName "myStyle"
        Assert.True(Style.isNamed style)

    [<Fact>]
    let ``unnamed style isNamed returns false`` () =
        Assert.False(Style.isNamed Style.empty)

    [<Fact>]
    let ``named style toCssString`` () =
        let fillColor = Color.ofName Colors.Red
        let style = Style.createWithFill fillColor |> Style.withName "highlight"
        let result = style |> Style.toCssString
        Assert.Contains(".highlight", result)
        Assert.Contains("fill:red", result)

    [<Fact>]
    let ``style toAttribute produces inline style`` () =
        let fillColor = Color.ofName Colors.Green
        let result = Style.createWithFill fillColor |> Style.toAttribute |> Attribute.toString
        Assert.Equal("style=\"fill:green\"", result)

    [<Fact>]
    let ``named style applied to element uses class not inline attributes`` () =
        let strokeColor = Color.ofName Colors.Black
        let style = Style.createWithStroke strokeColor |> Style.withName "outlined"
        let result = Rect.create Point.origin Area.full |> Element.createWithStyle style |> Element.toString
        Assert.Contains("class=\"outlined\"", result)
        Assert.DoesNotContain("stroke=", result)

