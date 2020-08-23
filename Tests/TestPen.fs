namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPen =

    [<Fact>]
    let ``create pen`` () =
        let pen = Pen.createWithOpacityAndWidth (Color.ofName Colors.Black) 1.0 (Length.ofInt 1)
        let style = Style.createWithPen pen
        Assert.Equal("stroke:black;stroke-width:1;opacity:1",  style |> Style.toString)
