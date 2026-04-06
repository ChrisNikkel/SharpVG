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

    // Wiki: Rect page — styled rect example
    [<Fact>]
    let ``Rect wiki - styled rect with position, size, and stroke`` () =
        let position = Point.ofInts (55, 45)
        let area = Area.ofInts (25, 15)
        let style = Style.create (Color.ofName Colors.Red) (Color.ofName Colors.Green) (Length.ofInt 3) 1.0 1.0
        let result = Rect.create position area |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<rect", result)
        Assert.Contains("x=\"55\"", result)
        Assert.Contains("y=\"45\"", result)
        Assert.Contains("width=\"25\"", result)
        Assert.Contains("height=\"15\"", result)
        Assert.Contains("fill=\"red\"", result)
        Assert.Contains("stroke=\"green\"", result)
        Assert.Contains("stroke-width=\"3\"", result)
