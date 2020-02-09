namespace SharpVG.Tests

open SharpVG
open Xunit

module TestPath =

    [<Fact>]
    let ``create empty`` () =
        Assert.Equal("<path d=\"\"/>", Path.empty |> Path.toString)

    [<Fact>]
    let ``create move to`` () =
        Assert.Equal("<path d=\"M0,0\"/>", Path.empty |> Path.addMoveTo Absolute Point.origin |> Path.toString)

    [<Fact>]
    let ``create relative move to`` () =
        Assert.Equal("<path d=\"m0,0\"/>", Path.empty |> Path.addMoveTo Relative Point.origin |> Path.toString)

    [<Fact>]
    let ``create line to`` () =
        Assert.Equal("<path d=\"L0,0\"/>", Path.empty |> Path.addLineTo Absolute Point.origin |> Path.toString)

    [<Fact>]
    let ``create lines to`` () =
        Assert.Equal("<path d=\"L0,0 0,0\"/>", Path.empty |> Path.addLinesTo Absolute (seq {Point.origin; Point.origin}) |> Path.toString)

    [<Fact>]
    let ``create line to with close`` () =
        Assert.Equal("<path d=\"L0,0 Z\"/>", Path.empty |> Path.addLineTo Absolute Point.origin |> Path.addClosePath |> Path.toString)

    [<Fact>]
    let ``create horizontal line to`` () =
        Assert.Equal("<path d=\"H0\"/>", Path.empty |> Path.addHorizontalLineTo Absolute Length.empty |> Path.toString)

    [<Fact>]
    let ``create vertical line to`` () =
        Assert.Equal("<path d=\"V0\"/>", Path.empty |> Path.addVerticalLineTo Absolute Length.empty |> Path.toString)

    [<Fact>]
    let ``create cubic bezier curve to`` () =
        Assert.Equal("<path d=\"C0,0 0,0 0,0\"/>", Path.empty |> Path.addCubicBezierCurveTo Absolute Point.origin Point.origin Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth cubic bezier curve to`` () =
        Assert.Equal("<path d=\"S0,0 0,0\"/>", Path.empty |> Path.addSmoothCubicBezierCurveTo Absolute Point.origin Point.origin |> Path.toString)

    [<Fact>]
    let ``create quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"Q0,0 0,0\"/>", Path.empty |> Path.addQuadraticBezierCurveTo Absolute Point.origin Point.origin |> Path.toString)

    [<Fact>]
    let ``create smooth quadratic bezier curve to`` () =
        Assert.Equal("<path d=\"T0,0\"/>", Path.empty |> Path.addSmoothQuadraticBezierCurveTo Absolute Point.origin |> Path.toString)

    [<Fact>]
    let ``create ellipticalArc`` () =
        Assert.Equal("<path d=\"A0,0 -90 0,0 0,0\"/>", Path.empty |> Path.addEllipticalArcCurveTo Absolute Point.origin -90.0 false false Point.origin |> Path.toString)

