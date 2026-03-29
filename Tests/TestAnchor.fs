namespace SharpVG.Tests

open SharpVG
open Xunit

module TestAnchor =

    [<Fact>]
    let ``create anchor`` () =
        let anchor = Anchor.create "http://www.sharpvg.com"
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        Assert.Equal("<a href=\"http://www.sharpvg.com\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></a>", Anchor.withBody body anchor |> Anchor.toString)

    [<Fact>]
    let ``addElement`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let result = Anchor.create "http://example.com" |> Anchor.addElement circle |> Anchor.toString
        Assert.Contains("<circle", result)
        Assert.Contains("href=\"http://example.com\"", result)

    [<Fact>]
    let ``addElements`` () =
        let smallCircle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let offsetCircle = Circle.create (Point.ofInts (10, 10)) (Length.ofInt 3) |> Element.create
        let result = Anchor.create "http://example.com" |> Anchor.addElements [smallCircle; offsetCircle] |> Anchor.toString
        Assert.Equal(2, result.Split("<circle").Length - 1)
