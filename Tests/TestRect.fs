namespace SharpVG.Tests

open SharpVG
open Xunit

module TestRect =

    [<Fact>]
    let ``create rect`` () =
        Assert.Equal("<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\"/>", Rect.create Point.origin Area.full |> Rect.toString)

    [<Fact>]
    let ``create rect with rounded corners`` () =
        Assert.Equal("<rect x=\"0\" y=\"0\" width=\"100%\" height=\"100%\" rx=\"0\" ry=\"0\"/>", Rect.create Point.origin Area.full |> Rect.withCornerRadius Point.origin |> Rect.toString)
