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

    [<Fact>]
    let ``create symbol and use with style`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "name"
        let style = Style.empty |> Style.withStroke (Name Colors.White) |> Style.withFill (Name Colors.Black)
        let useElement = Use.create symbolElement Point.origin |> Element.createWithStyle style
        let elements = [ symbolElement; useElement ]
        Assert.Equal("<symbol id=\"name\" viewBox=\"0,0 100%,100%\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></symbol><use stroke=\"white\" fill=\"black\" href=\"#name\" x=\"0\" y=\"0\"/>", elements |> List.map Element.toString |> String.concat "")
