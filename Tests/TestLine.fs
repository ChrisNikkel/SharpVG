namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit

module TestLine =

    [<Fact>]
    let ``create line with origin points`` () =
        Assert.Equal("<line x1=\"0\" y1=\"0\" x2=\"0\" y2=\"0\"/>", Line.create Point.origin Point.origin |> Line.toString)

    [<Fact>]
    let ``create line with float points`` () =
        let point1 = Point.create (Length.ofFloat 10.0) (Length.ofFloat 20.0)
        let point2 = Point.create (Length.ofFloat 30.0) (Length.ofFloat 40.0)
        let expected = "<line x1=\"10\" y1=\"20\" x2=\"30\" y2=\"40\"/>"
        Assert.Equal(expected, Line.create point1 point2 |> Line.toString)

    [<Fact>]
    let ``line with decimal coordinates`` () =
        let point1 = Point.create (Length.ofFloat 10.5) (Length.ofFloat 20.75)
        let point2 = Point.create (Length.ofFloat 30.25) (Length.ofFloat 40.5)
        let expected = "<line x1=\"10.5\" y1=\"20.75\" x2=\"30.25\" y2=\"40.5\"/>"
        Assert.Equal(expected, Line.create point1 point2 |> Line.toString)

    // Wiki: Line page — styled line example
    [<Fact>]
    let ``Line wiki - styled line with start, end, and stroke`` () =
        let startPoint = Point.ofInts (5, 0)
        let endPoint = Point.ofInts (60, 9)
        let style = Style.create (Color.ofName Colors.Cyan) (Color.ofName Colors.Blue) (Length.ofInt 3) 1.0 1.0
        let result = Line.create startPoint endPoint |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<line", result)
        Assert.Contains("x1=\"5\"", result)
        Assert.Contains("y1=\"0\"", result)
        Assert.Contains("x2=\"60\"", result)
        Assert.Contains("y2=\"9\"", result)
        Assert.Contains("stroke=\"blue\"", result)
        Assert.Contains("fill=\"cyan\"", result)
