namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPath =

    [<Fact>]
    let ``create empty`` () =
        Assert.Equal("<path d=\"\"/>", Path.empty |> Path.toString)

    [<Fact>]
    let ``create move to`` () =
        Assert.Equal("<path d=\"M0 0\"/>", Path.empty |> Path.addAbsolute MoveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create relative move to`` () =
        Assert.Equal("<path d=\"m0 0\"/>", Path.empty |> Path.addRelative MoveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create line to`` () =
        Assert.Equal("<path d=\"L0 0\"/>", Path.empty |> Path.addAbsolute LineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create line to with close`` () =
        Assert.Equal("<path d=\"L0 0 Z\"/>", Path.empty |> Path.addAbsolute LineTo Point.origin |> Path.addClosePath |> Path.toString)

    [<Fact>]
    let ``create horizontal line to`` () =
        Assert.Equal("<path d=\"H0 0\"/>", Path.empty |> Path.addAbsolute HorizontalLineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create vertical line to`` () =
        Assert.Equal("<path d=\"V0 0\"/>", Path.empty |> Path.addAbsolute VerticalLineTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create curve to`` () =
        Assert.Equal("<path d=\"C0 0\"/>", Path.empty |> Path.addAbsolute CurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth curve to`` () =
        Assert.Equal("<path d=\"S0 0\"/>", Path.empty |> Path.addAbsolute SmoothCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"Q0 0\"/>", Path.empty |> Path.addAbsolute QuadraticBezierCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"T0 0\"/>", Path.empty |> Path.addAbsolute SmoothQuadraticBezierCurveTo Point.origin |> Path.toString)

    [<Fact>]
    let ``create ellipticalArc`` () =
        Assert.Equal("<path d=\"A0,0 -90 0,0 0,0\"/>", Path.empty |> Path.addEllipticalArc Absolute Point.origin -90 false false Point.origin |> Path.toString)

