namespace SharpVG.Tests

open SharpVG
open Xunit

module TestRect =

    [<Fact>]
    let ``create rect`` () =
        Assert.Equal(Rect.create Point.origin Area.full |> Rect.toString, "<rect x=\"0\" y=\"0\" height=\"100%\" width=\"100%\"/>")