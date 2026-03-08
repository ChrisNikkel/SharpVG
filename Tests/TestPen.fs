namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPen =

    [<Fact>]
    let ``create pen`` () =
        let pen = Pen.createWithOpacityAndWidth (Color.ofName Colors.Black) 1.0 (Length.ofInt 1)
        let style = Style.createWithPen pen
        Assert.Equal("stroke:black;stroke-width:1;opacity:1",  style |> Style.toString)

    [<Fact>]
    let ``pen create uses default opacity and width`` () =
        let strokeColor = Color.ofName Colors.Red
        let pen = Pen.create strokeColor
        let style = Style.createWithPen pen
        let result = style |> Style.toString
        Assert.Contains("stroke:red", result)
        Assert.Contains("opacity:1", result)
        Assert.Contains("stroke-width:1", result)

    [<Fact>]
    let ``pen createWithOpacity`` () =
        let strokeColor = Color.ofName Colors.Green
        let pen = Pen.createWithOpacity strokeColor 0.5
        let result = Style.createWithPen pen |> Style.toString
        Assert.Contains("opacity:0.5", result)
        Assert.Contains("stroke:green", result)

    [<Fact>]
    let ``pen createWithWidth`` () =
        let strokeColor = Color.ofName Colors.Blue
        let penWidth = Length.ofInt 4
        let pen = Pen.createWithWidth strokeColor penWidth
        let result = Style.createWithPen pen |> Style.toString
        Assert.Contains("stroke-width:4", result)
        Assert.Contains("stroke:blue", result)

    [<Fact>]
    let ``pen withOpacity`` () =
        let strokeColor = Color.ofName Colors.Black
        let pen = Pen.create strokeColor |> Pen.withOpacity 0.3
        let result = Style.createWithPen pen |> Style.toString
        Assert.Contains("opacity:0.3", result)

    [<Fact>]
    let ``pen withWidth`` () =
        let strokeColor = Color.ofName Colors.White
        let penWidth = Length.ofInt 6
        let pen = Pen.create strokeColor |> Pen.withWidth penWidth
        let result = Style.createWithPen pen |> Style.toString
        Assert.Contains("stroke-width:6", result)
