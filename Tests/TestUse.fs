namespace SharpVG.Tests

open SharpVG
open Xunit

module TestUse =

    [<Fact>]
    let ``create symbol and use`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "name"
        let useElement = Use.create symbolElement Point.origin |> Element.create
        let elements = [ symbolElement; useElement ]

        Assert.Equal("<symbol id=\"name\" viewBox=\"0,0 100%,100%\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></symbol><use href=\"#name\" x=\"0\" y=\"0\"/>", elements |> List.map Element.toString |> String.concat "")
