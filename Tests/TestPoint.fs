namespace SharpVG.Tests
open SharpVG
open Xunit

module TestPoint =

    [<Fact>]
    let ``create point`` () =
        Assert.Equal("0,0", (Point.create Length.empty Length.empty |> Point.toString))

    [<Fact>]
    let ``create origin`` () =
        Assert.Equal("0,0", (Point.origin |> Point.toString))

    [<Fact>]
    let ``transform point to attribute`` () =
        Assert.Equal("x=\"0\" y=\"0\"", (Point.origin |> Point.toAttributes |> List.map Attribute.toString |> String.concat " "))

    [<Fact>]
    let ``transform point to attribute with modifier`` () =
        Assert.Equal("axb=\"0\" ayb=\"0\"", (Point.origin |> Point.toAttributesWithModifier "a" "b" |> List.map Attribute.toString |> String.concat " "))

    [<Fact>]
    let ``transform point to attribute with separator`` () =
        Assert.Equal("0 0", (Point.origin |> Point.toStringWithSeparator " "))

    [<Fact>]
    let ``points to string`` () =
        Assert.Equal("0,0 0,0", (seq { yield Point.origin; yield Point.origin } |> Points.toString))
