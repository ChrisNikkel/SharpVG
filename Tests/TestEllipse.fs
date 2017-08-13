namespace SharpVG.Tests

open SharpVG
open Xunit

module TestEllipse =

    [<Fact>]
    let ``create ellipse`` () =
        Assert.Equal(Ellipse.create Point.origin Point.origin |> Ellipse.toString, "<ellipse cx=\"0\" cy=\"0\" rx=\"0\" ry=\"0\"/>")