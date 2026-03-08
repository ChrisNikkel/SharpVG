namespace SharpVG.Tests

open SharpVG
open Xunit

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
