namespace SharpVG.Tests

open SharpVG
open Xunit

module TestFilter =

    [<Fact>]
    let ``create blend filter`` () =
        let filter = Filter.create (FilterEffect.createBlend BlendMode.Lighten)
        let element = filter |> Element.createWithName "blendFilter"
        Assert.Equal("<filter id=\"blendFilter\"><feBlend mode=\"Lighten\"/></filter>", element |> Element.toString)

    [<Fact>]
    let ``create gaussian blur filter`` () =
        let filter = Filter.create (FilterEffect.createGaussianBlur 5.0)
        let element = filter |> Element.createWithName "blureFilter"
        Assert.Equal("<filter id=\"blureFilter\"><feGaussianBlur stdDeviation=\"5\"/></filter>", element |> Element.toString)