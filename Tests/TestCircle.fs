namespace SharpVG.Tests

open SharpVG
open Xunit

module TestCircle =

    [<Fact>]
    let ``create circle`` () =
        Assert.Equal("<circle r=\"0\" cx=\"0\" cy=\"0\"/>", Circle.create Point.origin Length.empty |> Circle.toString)

    // Wiki: Circle page — styled circle example
    [<Fact>]
    let ``Circle wiki - styled circle with center, radius, and stroke`` () =
        let center = Point.ofInts (15, 15)
        let radius = Length.ofInt 5
        let style = Style.create (Color.ofName Colors.Cyan) (Color.ofName Colors.Blue) (Length.ofInt 3) 1.0 1.0
        let result = Circle.create center radius |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<circle", result)
        Assert.Contains("r=\"5\"", result)
        Assert.Contains("cx=\"15\"", result)
        Assert.Contains("cy=\"15\"", result)
        Assert.Contains("stroke=\"blue\"", result)
        Assert.Contains("fill=\"cyan\"", result)
        Assert.Contains("stroke-width=\"3\"", result)