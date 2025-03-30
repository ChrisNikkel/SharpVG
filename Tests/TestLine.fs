namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit

module TestLine =

    [<Fact>]
    let ``create line with origin points`` () =
        Assert.Equal("<line x1=\"0\" y1=\"0\" x2=\"0\" y2=\"0\"/>", Line.create Point.origin Point.origin |> Line.toString)

    [<Property>]
    let ``create line with float points`` (x1: float, y1: float, x2: float, y2: float) =
        let point1 = Point.create (Length.ofFloat 10.0) (Length.ofFloat 20.0)
        let point2 = Point.create (Length.ofFloat 30.0) (Length.ofFloat 40.0)
        let expected = "<line x1=\"10\" y1=\"20\" x2=\"30\" y2=\"40\"/>"
        Assert.Equal(expected, Line.create point1 point2 |> Line.toString)

    [<Property>]
    let ``line creation preserves point coordinates`` (x1: float, y1: float, x2: float, y2: float) =
        let point1 = Point.create (Length.ofFloat x1) (Length.ofFloat y1)
        let point2 = Point.create (Length.ofFloat x2) (Length.ofFloat y2)
        let line = Line.create point1 point2
        line.Point1 = point1 && line.Point2 = point2

    [<Fact>]
    let ``line with decimal coordinates`` () =
        let point1 = Point.create (Length.ofFloat 10.5) (Length.ofFloat 20.75)
        let point2 = Point.create (Length.ofFloat 30.25) (Length.ofFloat 40.5)
        let expected = "<line x1=\"10.5\" y1=\"20.75\" x2=\"30.25\" y2=\"40.5\"/>"
        Assert.Equal(expected, Line.create point1 point2 |> Line.toString)