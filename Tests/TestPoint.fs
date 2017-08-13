namespace SharpVG.Tests
open SharpVG
open Xunit

module TestPoint =

    [<Fact>]
    let ``create point`` () =
        Assert.Equal((Point.create Length.empty Length.empty |> Point.toString), "0,0")

    [<Fact>]
    let ``create origin`` () =
        Assert.Equal((Point.origin |> Point.toString), "0,0")

    [<Fact>]
    let ``transform point to attribute`` () =
        Assert.Equal((Point.origin |> Point.toAttributes |> List.map Attribute.toString |> String.concat " "), "x=\"0\" y=\"0\"")

    [<Fact>]
    let ``transform point to attribute with modifier`` () =
        Assert.Equal((Point.origin |> Point.toAttributesWithModifier "a" "b" |> List.map Attribute.toString |> String.concat " "), "axb=\"0\" ayb=\"0\"")

    [<Fact>]
    let ``transform point to attribute with separator`` () =
        Assert.Equal((Point.origin |> Point.toStringWithSeparator " "), "0 0")

    [<Fact>]
    let ``points to string`` () =
        Assert.Equal((seq { yield Point.origin; yield Point.origin } |> Points.toString), "0,0 0,0")
