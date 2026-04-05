namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestSymbol =

    [<Fact>]
    let ``create symbol`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "name"

        Assert.Equal("<symbol id=\"name\" viewBox=\"0,0 100%,100%\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></symbol>", symbolElement |> Element.toString)

    [<Fact>]
    let ``symbol withSize`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let size = Area.ofInts (50, 50)
        let result = Symbol.create viewBox |> Symbol.withSize (Some size) |> Symbol.toString
        Assert.Contains("width=\"50\"", result)
        Assert.Contains("height=\"50\"", result)

    [<Fact>]
    let ``symbol withPosition`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let position = Point.ofInts (10, 20)
        let result = Symbol.create viewBox |> Symbol.withPosition (Some position) |> Symbol.toString
        Assert.Contains("x=\"10\"", result)
        Assert.Contains("y=\"20\"", result)

    [<Fact>]
    let ``symbol addElement`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let result = Symbol.create viewBox |> Symbol.addElement circle |> Symbol.toString
        Assert.Contains("<circle", result)
        Assert.Contains("r=\"5\"", result)

    [<Fact>]
    let ``symbol addElements`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let rect = Rect.create Point.origin Area.full |> Element.create
        let result = Symbol.create viewBox |> Symbol.addElements [circle; rect] |> Symbol.toString
        Assert.Contains("<circle", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``symbol withReference sets rx and ry`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let reference = Point.ofInts (5, 10)
        let result = Symbol.create viewBox |> Symbol.withReference (Some reference) |> Symbol.toString
        Assert.Contains("rx=\"5\"", result)
        Assert.Contains("ry=\"10\"", result)

    // Wiki: Symbol page — reusable icon placed via Use
    [<Fact>]
    let ``Symbol wiki - reusable icon placed at multiple positions`` () =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (100, 100))
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        let fillStyle = Style.createWithFill (Color.ofName Colors.Blue)
        let iconSymbol =
            Symbol.create viewBox
            |> Symbol.withBody [
                Circle.create center radius |> Element.createWithStyle fillStyle
            ]
            |> Element.createWithName "icon"
        let position1 = Point.ofInts (10, 10)
        let position2 = Point.ofInts (120, 10)
        let use1 = Use.create iconSymbol position1 |> Element.create
        let use2 = Use.create iconSymbol position2 |> Element.create
        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement iconSymbol
        let output =
            [ use1; use2 ]
            |> Svg.ofElementsWithDefinitions definitions
            |> Svg.toString
        Assert.Contains("<symbol id=\"icon\"", output)
        Assert.Contains("viewBox=\"0,0 100,100\"", output)
        Assert.Contains("fill=\"blue\"", output)
        let useCount = output.Split("<use ").Length - 1
        Assert.Equal(2, useCount)

    [<SvgProperty>]
    let ``symbol with any positive viewBox dimensions always produces balanced tags`` (x: float, y: float, w: float, h: float) =
        let viewBox = ViewBox.create (Point.ofFloats (x, y)) (Area.ofFloats (w, h))
        let result = Symbol.create viewBox |> Symbol.toString
        checkTag "symbol" result

    [<SvgProperty>]
    let ``symbol with content always has balanced angle brackets`` (x: float, y: float, r: float) =
        let viewBox = ViewBox.create (Point.ofFloats (x, y)) (Area.ofFloats (r, r))
        let circle = Circle.create Point.origin (Length.ofFloat r) |> Element.create
        let result = Symbol.create viewBox |> Symbol.addElement circle |> Symbol.toString
        checkTag "symbol" result

    [<Property>]
    let ``symbol always contains viewBox attribute`` (w: PositiveInt, h: PositiveInt) =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (w.Get, h.Get))
        let result = Symbol.create viewBox |> Symbol.toString
        result.Contains("viewBox=")
