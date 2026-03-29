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

    [<Fact>]
    let ``create partial triangle using absolutes`` () =
        let paritalTriangle =
            Path.empty
                |> Path.addMoveTo Absolute (Point.ofFloats (10.0, 10.0))
                |> Path.addLineTo Absolute (Point.ofFloats (90.0, 90.0))
                |> Path.addVerticalLineTo Absolute (Length.ofFloat 10.0)
                |> Path.addHorizontalLineTo Absolute (Length.ofFloat 50.0)
        Assert.Equal("<path d=\"M10,10 L90,90 V10 H50\"/>", paritalTriangle |> Path.toString)

    [<Fact>]
    let ``create partial triangle using relatives`` () =
        let paritalTriangle =
            Path.empty
                |> Path.addMoveTo Absolute (Point.ofFloats (110.0, 10.0))
                |> Path.addLineTo Relative (Point.ofFloats (80.0, 80.0))
                |> Path.addVerticalLineTo Relative (Length.ofFloat -80.0)
                |> Path.addHorizontalLineTo Relative (Length.ofFloat -40.0)
        Assert.Equal("<path d=\"M110,10 l80,80 v-80 h-40\"/>", paritalTriangle |> Path.toString)

    [<Fact>]
    let ``addMovesTo multiple points`` () =
        let result = Path.empty |> Path.addMovesTo Absolute (seq { Point.ofInts (0, 0); Point.ofInts (10, 20) }) |> Path.toString
        Assert.Equal("<path d=\"M0,0 10,20\"/>", result)

    [<Fact>]
    let ``addHorizontalLinesTo multiple lengths`` () =
        let result = Path.empty |> Path.addHorizontalLinesTo Absolute (seq { Length.ofInt 10; Length.ofInt 20 }) |> Path.toString
        Assert.Equal("<path d=\"H10 20\"/>", result)

    [<Fact>]
    let ``addVerticalLinesTo multiple lengths`` () =
        let result = Path.empty |> Path.addVerticalLinesTo Relative (seq { Length.ofInt 5; Length.ofInt 15 }) |> Path.toString
        Assert.Equal("<path d=\"v5 15\"/>", result)

    [<Fact>]
    let ``addCubicBezierCurvesTo multiple triplets`` () =
        let origin = Point.origin
        let result = Path.empty |> Path.addCubicBezierCurvesTo Absolute (seq { origin, origin, origin; origin, origin, origin }) |> Path.toString
        Assert.Equal("<path d=\"C0,0 0,0 0,0 0,0 0,0 0,0\"/>", result)

    [<Fact>]
    let ``addSmoothCubicBezierCurvesTo multiple pairs`` () =
        let origin = Point.origin
        let result = Path.empty |> Path.addSmoothCubicBezierCurvesTo Absolute (seq { origin, origin; origin, origin }) |> Path.toString
        Assert.Equal("<path d=\"S0,0 0,0 0,0 0,0\"/>", result)

    [<Fact>]
    let ``addQuadraticBezierCurvesTo multiple pairs`` () =
        let origin = Point.origin
        let result = Path.empty |> Path.addQuadraticBezierCurvesTo Absolute (seq { origin, origin; origin, origin }) |> Path.toString
        Assert.Equal("<path d=\"Q0,0 0,0 0,0 0,0\"/>", result)

    [<Fact>]
    let ``addSmoothQuadraticBezierCurvesTo multiple points`` () =
        let result = Path.empty |> Path.addSmoothQuadraticBezierCurvesTo Absolute (seq { Point.ofInts (10, 20); Point.ofInts (30, 40) }) |> Path.toString
        Assert.Equal("<path d=\"T10,20 30,40\"/>", result)

    [<Fact>]
    let ``addEllipticalArcCurvesTo multiple params`` () =
        let arc = { Radius = Point.origin; RotationXAxis = 0.0; LargeArc = false; Sweep = false; Point = Point.origin }
        let result = Path.empty |> Path.addEllipticalArcCurvesTo Absolute (seq { arc; arc }) |> Path.toString
        Assert.Equal("<path d=\"A0,0 0 0,0 0,0 0,0 0 0,0 0,0\"/>", result)
