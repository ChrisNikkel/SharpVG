namespace SharpVG.Tests

open SharpVG
open Xunit

module TestCircle =

    [<Fact>]
    let ``create circle`` () =
        Assert.Equal(Circle.create Point.origin Length.empty |> Circle.toString, "<circle r=\"0\" cx=\"0\" cy=\"0\"/>")