namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPolygon =

    [<Fact>]
    let ``create polygon`` () =
        Assert.Equal("<polygon points=\"0,0 0,0 0,0 0,0\"/>", seq { yield Point.origin; yield Point.origin; yield Point.origin; yield Point.origin } |> Polygon.ofSeq |> Polygon.toString)