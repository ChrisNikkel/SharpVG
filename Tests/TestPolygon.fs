namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPolygon =

    [<Fact>]
    let ``create polygon`` () =
        Assert.Equal("<polygon points=\"0,0 0,0 0,0 0,0\"/>", seq { yield Point.origin; yield Point.origin; yield Point.origin; yield Point.origin } |> Polygon.ofSeq |> Polygon.toString)

    [<Fact>]
    let ``ofList`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (10, 0); Point.ofInts (5, 10) ]
        let result = points |> Polygon.ofList |> Polygon.toString
        Assert.Contains("polygon", result)
        Assert.Contains("0,0", result)
        Assert.Contains("10,0", result)
        Assert.Contains("5,10", result)

    [<Fact>]
    let ``ofArray`` () =
        let points = [| Point.ofInts (0, 0); Point.ofInts (10, 0); Point.ofInts (5, 10) |]
        let result = points |> Polygon.ofArray |> Polygon.toString
        Assert.Contains("polygon", result)
        Assert.Contains("5,10", result)