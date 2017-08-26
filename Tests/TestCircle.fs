namespace SharpVG.Tests

open SharpVG
open Xunit

module TestCircle =

    [<Fact>]
    let ``create circle`` () =
        Assert.Equal("<circle r=\"0\" cx=\"0\" cy=\"0\"/>", Circle.create Point.origin Length.empty |> Circle.toString)