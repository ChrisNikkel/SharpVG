namespace SharpVG.Tests

open SharpVG
open Xunit

module TestLine =

    [<Fact>]
    let ``create line`` () =
        Assert.Equal("<line x1=\"0\" y1=\"0\" x2=\"0\" y2=\"0\"/>", Line.create Point.origin Point.origin |> Line.toString)