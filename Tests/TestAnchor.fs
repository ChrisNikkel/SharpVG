namespace SharpVG.Tests

open SharpVG
open Xunit

module TestAnchor =

    [<Fact>]
    let ``create anchor`` () =
        let anchor = Anchor.create "http://www.sharpvg.com"
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        Assert.Equal("<a href=\"http://www.sharpvg.com\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></a>", Anchor.withBody body anchor |> Anchor.toString)
