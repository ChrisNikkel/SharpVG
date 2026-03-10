namespace SharpVG.Tests

open SharpVG
open Xunit

module TestGradient =

    [<Fact>]
    let ``GradientStop create basic stop`` () =
        let stop = GradientStop.create 0.0 (Color.ofName Colors.Blue)
        let result = stop |> GradientStop.toString
        Assert.Contains("offset=\"0\"", result)
        Assert.Contains("stop-color=\"blue\"", result)

    [<Fact>]
    let ``GradientStop create with offset 1`` () =
        let stop = GradientStop.create 1.0 (Color.ofName Colors.Red)
        let result = stop |> GradientStop.toString
        Assert.Contains("offset=\"1\"", result)
        Assert.Contains("stop-color=\"red\"", result)

    [<Fact>]
    let ``GradientStop createWithOpacity`` () =
        let stop = GradientStop.createWithOpacity 0.5 (Color.ofName Colors.Green) 0.8
        let result = stop |> GradientStop.toString
        Assert.Contains("offset=\"0.5\"", result)
        Assert.Contains("stop-color=\"green\"", result)
        Assert.Contains("stop-opacity=\"0.8\"", result)

    [<Fact>]
    let ``GradientStop withOpacity`` () =
        let stop = GradientStop.create 0.0 (Color.ofName Colors.Blue) |> GradientStop.withOpacity 0.5
        let result = stop |> GradientStop.toString
        Assert.Contains("stop-opacity=\"0.5\"", result)

    [<Fact>]
    let ``GradientStop renders as stop tag`` () =
        let stop = GradientStop.create 0.0 (Color.ofName Colors.Blue)
        let result = stop |> GradientStop.toString
        Assert.StartsWith("<stop", result)

    [<Fact>]
    let ``LinearGradient create basic`` () =
        let stops = [ GradientStop.create 0.0 (Color.ofName Colors.Blue); GradientStop.create 1.0 (Color.ofName Colors.Red) ]
        let gradient = LinearGradient.create "myGrad" Point.origin (Point.ofInts (1, 0)) stops
        let result = gradient |> LinearGradient.toString
        Assert.Contains("<linearGradient", result)
        Assert.Contains("id=\"myGrad\"", result)
        Assert.Contains("x1=\"0\"", result)
        Assert.Contains("y1=\"0\"", result)
        Assert.Contains("x2=\"1\"", result)
        Assert.Contains("y2=\"0\"", result)

    [<Fact>]
    let ``LinearGradient contains stops`` () =
        let stops = [ GradientStop.create 0.0 (Color.ofName Colors.Blue); GradientStop.create 1.0 (Color.ofName Colors.Red) ]
        let gradient = LinearGradient.create "myGrad" Point.origin (Point.ofInts (1, 0)) stops
        let result = gradient |> LinearGradient.toString
        Assert.Contains("stop-color=\"blue\"", result)
        Assert.Contains("stop-color=\"red\"", result)

    [<Fact>]
    let ``LinearGradient withGradientUnits userSpaceOnUse`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withGradientUnits UserSpaceOnUse
        let result = gradient |> LinearGradient.toString
        Assert.Contains("gradientUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``LinearGradient withGradientUnits objectBoundingBox`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withGradientUnits ObjectBoundingBox
        let result = gradient |> LinearGradient.toString
        Assert.Contains("gradientUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``LinearGradient withSpreadMethod pad`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withSpreadMethod Pad
        let result = gradient |> LinearGradient.toString
        Assert.Contains("spreadMethod=\"pad\"", result)

    [<Fact>]
    let ``LinearGradient withSpreadMethod reflect`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withSpreadMethod Reflect
        let result = gradient |> LinearGradient.toString
        Assert.Contains("spreadMethod=\"reflect\"", result)

    [<Fact>]
    let ``LinearGradient withSpreadMethod repeat`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withSpreadMethod Repeat
        let result = gradient |> LinearGradient.toString
        Assert.Contains("spreadMethod=\"repeat\"", result)

    [<Fact>]
    let ``LinearGradient withGradientTransform`` () =
        let transform = Transform.createRotate 45.0 Length.empty Length.empty
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withGradientTransform transform
        let result = gradient |> LinearGradient.toString
        Assert.Contains("gradientTransform=", result)

    [<Fact>]
    let ``LinearGradient withHref`` () =
        let gradient = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
                       |> LinearGradient.withHref "baseGrad"
        let result = gradient |> LinearGradient.toString
        Assert.Contains("href=\"#baseGrad\"", result)

    [<Fact>]
    let ``RadialGradient create basic`` () =
        let stops = [ GradientStop.create 0.0 (Color.ofName Colors.Blue) ]
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) stops
        let result = gradient |> RadialGradient.toString
        Assert.Contains("<radialGradient", result)
        Assert.Contains("id=\"rGrad\"", result)
        Assert.Contains("cx=\"50\"", result)
        Assert.Contains("cy=\"50\"", result)
        Assert.Contains("r=\"50\"", result)

    [<Fact>]
    let ``RadialGradient contains stops`` () =
        let stops = [ GradientStop.create 0.0 (Color.ofName Colors.Blue); GradientStop.create 1.0 (Color.ofName Colors.White) ]
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) stops
        let result = gradient |> RadialGradient.toString
        Assert.Contains("stop-color=\"blue\"", result)
        Assert.Contains("stop-color=\"white\"", result)

    [<Fact>]
    let ``RadialGradient withFocal`` () =
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
                       |> RadialGradient.withFocal (Point.ofInts (30, 30))
        let result = gradient |> RadialGradient.toString
        Assert.Contains("fx=\"30\"", result)
        Assert.Contains("fy=\"30\"", result)

    [<Fact>]
    let ``RadialGradient withGradientUnits`` () =
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
                       |> RadialGradient.withGradientUnits UserSpaceOnUse
        let result = gradient |> RadialGradient.toString
        Assert.Contains("gradientUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``RadialGradient withSpreadMethod`` () =
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
                       |> RadialGradient.withSpreadMethod Reflect
        let result = gradient |> RadialGradient.toString
        Assert.Contains("spreadMethod=\"reflect\"", result)

    [<Fact>]
    let ``RadialGradient withGradientTransform`` () =
        let transform = Transform.createRotate 30.0 Length.empty Length.empty
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
                       |> RadialGradient.withGradientTransform transform
        let result = gradient |> RadialGradient.toString
        Assert.Contains("gradientTransform=", result)

    [<Fact>]
    let ``RadialGradient withHref`` () =
        let gradient = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
                       |> RadialGradient.withHref "baseGrad"
        let result = gradient |> RadialGradient.toString
        Assert.Contains("href=\"#baseGrad\"", result)

    [<Fact>]
    let ``Gradient ofLinear wraps LinearGradient`` () =
        let lg = LinearGradient.create "g" Point.origin (Point.ofInts (1, 0)) []
        let gradient = Gradient.ofLinear lg
        let result = gradient |> Gradient.toString
        Assert.Contains("<linearGradient", result)

    [<Fact>]
    let ``Gradient ofRadial wraps RadialGradient`` () =
        let rg = RadialGradient.create "rGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) []
        let gradient = Gradient.ofRadial rg
        let result = gradient |> Gradient.toString
        Assert.Contains("<radialGradient", result)

    [<Fact>]
    let ``SpreadMethod Pad toString`` () =
        Assert.Equal("pad", Pad.ToString())

    [<Fact>]
    let ``SpreadMethod Reflect toString`` () =
        Assert.Equal("reflect", Reflect.ToString())

    [<Fact>]
    let ``SpreadMethod Repeat toString`` () =
        Assert.Equal("repeat", Repeat.ToString())
