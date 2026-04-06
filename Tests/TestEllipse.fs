namespace SharpVG.Tests

open SharpVG
open Xunit

module TestEllipse =

    [<Fact>]
    let ``create ellipse`` () =
        Assert.Equal("<ellipse cx=\"0\" cy=\"0\" rx=\"0\" ry=\"0\"/>", Ellipse.create Point.origin Point.origin |> Ellipse.toString)

    // Wiki: Ellipse page — styled ellipse example
    [<Fact>]
    let ``Ellipse wiki - styled ellipse with center and radii`` () =
        let center = Point.ofInts (55, 45)
        let radius = Point.ofInts (5, 1)
        let style = Style.create (Color.ofName Colors.Red) (Color.ofName Colors.Green) (Length.ofInt 3) 1.0 1.0
        let result = Ellipse.create center radius |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<ellipse", result)
        Assert.Contains("rx=\"5\"", result)
        Assert.Contains("ry=\"1\"", result)
        Assert.Contains("cx=\"55\"", result)
        Assert.Contains("cy=\"45\"", result)
        Assert.Contains("stroke=\"green\"", result)
        Assert.Contains("fill=\"red\"", result)