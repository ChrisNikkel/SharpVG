namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPath =

    [<Fact>]
    let ``create empty`` () =
        Assert.Equal(Path.empty |> Path.toString, "<path d=\"\"/>")

    [<Fact>]
    let ``create move to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.MoveTo Point.origin |> Path.toString, "<path d=\"M0 0\"/>")

    [<Fact>]
    let ``create relative move to`` () =
        Assert.Equal(Path.empty |> Path.addRelative PathType.MoveTo Point.origin |> Path.toString, "<path d=\"m0 0\"/>")

    [<Fact>]
    let ``create line to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.LineTo Point.origin |> Path.toString, "<path d=\"L0 0\"/>")

    [<Fact>]
    let ``create line to with close`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.LineTo Point.origin |> Path.addClosePath |> Path.toString, "<path d=\"L0 0 Z\"/>")

    [<Fact>]
    let ``create horizontal line to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.HorizontalLineTo Point.origin |> Path.toString, "<path d=\"H0 0\"/>")

    [<Fact>]
    let ``create vertical line to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.VerticalLineTo Point.origin |> Path.toString, "<path d=\"V0 0\"/>")

    [<Fact>]
    let ``create curve to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.CurveTo Point.origin |> Path.toString, "<path d=\"C0 0\"/>")

    [<Fact>]
    let ``create smooth curve to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.SmoothCurveTo Point.origin |> Path.toString, "<path d=\"S0 0\"/>")

    [<Fact>]
    let ``create quadratic bezier curve to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.QuadraticBezierCurveTo Point.origin |> Path.toString,"<path d=\"Q0 0\"/>")

    [<Fact>]
    let ``create smooth quadratic bezier curve to`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.SmoothQuadraticBezierCurveTo Point.origin |> Path.toString, "<path d=\"T0 0\"/>")

    [<Fact>]
    let ``create ellipticalArc`` () =
        Assert.Equal(Path.empty |> Path.addAbsolute PathType.EllipticalArc Point.origin |> Path.toString, "<path d=\"A0 0\"/>")

