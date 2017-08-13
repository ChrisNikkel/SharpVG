namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPolygon =

    [<Fact>]
    let ``create polygon`` () =
        Assert.Equal(seq { yield Point.origin; yield Point.origin; yield Point.origin; yield Point.origin } |> Polygon.toString, "<polygon points=\"0,0 0,0 0,0 0,0\"/>")