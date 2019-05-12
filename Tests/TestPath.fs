namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPath =

    [<Fact>]
    let ``create empty`` () =
        Assert.Equal("<path d=\"\"/>", Path.empty |> Path.toString)

    [<Fact>]
    let ``create move to`` () =
        Assert.Equal("<path d=\"M0 0\"/>", Path.empty |> Path.addAbsolute PathType.MoveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create relative move to`` () =
        Assert.Equal("<path d=\"m0 0\"/>", Path.empty |> Path.addRelative PathType.MoveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create line to`` () =
        Assert.Equal("<path d=\"L0 0\"/>", Path.empty |> Path.addAbsolute PathType.LineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create line to with close`` () =
        Assert.Equal("<path d=\"L0 0 Z\"/>", Path.empty |> Path.addAbsolute PathType.LineTo Point.origin |> Path.addClosePath |> Path.toString)

    [<Fact>]
    let ``create horizontal line to`` () =
        Assert.Equal("<path d=\"H0 0\"/>", Path.empty |> Path.addAbsolute PathType.HorizontalLineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create vertical line to`` () =
        Assert.Equal("<path d=\"V0 0\"/>", Path.empty |> Path.addAbsolute PathType.VerticalLineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create curve to`` () =
        Assert.Equal("<path d=\"C0 0\"/>", Path.empty |> Path.addAbsolute PathType.CurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth curve to`` () =
        Assert.Equal("<path d=\"S0 0\"/>", Path.empty |> Path.addAbsolute PathType.SmoothCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"Q0 0\"/>", Path.empty |> Path.addAbsolute PathType.QuadraticBezierCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"T0 0\"/>", Path.empty |> Path.addAbsolute PathType.SmoothQuadraticBezierCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create ellipticalArc`` () =
        Assert.Equal("<path d=\"A0 0\"/>", Path.empty |> Path.addAbsolute PathType.EllipticalArc Point.origin |> Path.toString)

    [<Fact>]
    let ``create path by addPathString`` () =
        Assert.Equal("<path d=\"M0 0 a20 40 0 1 1 0 40\"/>", 
                     Path.empty 
                     |> Path.addAbsolute PathType.MoveTo Point.origin 
                     |> Path.addRelative PathType.EllipticalArc (Point.ofFloats (20.0, 40.0))
                     |> Path.addPathString "0 1 1 0 40"
                     |> Path.toString)

