namespace SharpVG.Tests

open SharpVG
open Xunit

module TestGraphic =

    [<Fact>]
    let ``create graphic`` () =
        let ellipse = Ellipse.create Point.origin Point.origin
        let graphic = Graphic.ofEllipse ellipse
        Assert.Equal("<ellipse cx=\"0\" cy=\"0\" rx=\"0\" ry=\"0\"/>",  graphic |> Graphic.toElement |> Element.toString)
