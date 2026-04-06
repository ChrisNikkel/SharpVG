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

    // Wiki: Polygon page — styled polygon example
    [<Fact>]
    let ``Polygon wiki - four-point styled polygon`` () =
        let points =
            seq {
                yield Point.ofInts (55, 45)
                yield Point.ofInts (45, 45)
                yield Point.ofInts (45, 15)
                yield Point.ofInts (10, 5)
            }
        let style = Style.create (Color.ofName Colors.Yellow) (Color.ofName Colors.Red) (Length.ofInt 3) 1.0 1.0
        let result = points |> Polygon.ofSeq |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<polygon", result)
        Assert.Contains("points=\"55,45 45,45 45,15 10,5\"", result)
        Assert.Contains("fill=\"yellow\"", result)
        Assert.Contains("stroke=\"red\"", result)