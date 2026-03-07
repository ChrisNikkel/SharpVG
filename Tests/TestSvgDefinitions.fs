namespace SharpVG.Tests

open SharpVG
open Xunit

module TestSvgDefinitions =

    [<Fact>]
    let ``create Svg with definitions containing symbol and use in body`` () =
        let viewBox = ViewBox.create Point.origin Area.full
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let symbolElement = Symbol.create viewBox |> Symbol.withBody body |> Element.createWithName "icon"
        let useElement = Use.create symbolElement Point.origin |> Element.create

        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement symbolElement
        let svg = [ useElement ] |> Svg.ofElementsWithDefinitions definitions

        let output = svg |> Svg.toString
        Assert.Contains("<defs>", output)
        Assert.Contains("</defs>", output)
        Assert.Contains("<symbol id=\"icon\"", output)
        Assert.Contains("<use href=\"#icon\" x=\"0\" y=\"0\"", output)
        // Symbol should appear only inside defs (before the use)
        let defsEnd = output.IndexOf("</defs>")
        let useStart = output.IndexOf("<use ")
        Assert.True(defsEnd < useStart, "defs block should appear before use element")

    [<Fact>]
    let ``SvgDefinitions toString renders defs tag with content`` () =
        let circleElement = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement circleElement
        let output = definitions |> SvgDefinitions.toString
        Assert.Equal("<defs><circle r=\"5\" cx=\"0\" cy=\"0\"/></defs>", output)
