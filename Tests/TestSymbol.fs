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
