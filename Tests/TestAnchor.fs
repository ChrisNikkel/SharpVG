namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

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

    // Wiki: Anchor page — hyperlink wrapping a styled circle
    [<Fact>]
    let ``Anchor wiki - styled circle wrapped in hyperlink`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        let style = Style.createWithFill (Color.ofName Colors.Blue)
        let circle = Circle.create center radius |> Element.createWithStyle style
        let result =
            Anchor.create "https://example.com"
            |> Anchor.addElement circle
            |> Anchor.toString
        Assert.Contains("href=\"https://example.com\"", result)
        Assert.Contains("<circle", result)
        Assert.Contains("fill=\"blue\"", result)
        Assert.StartsWith("<a ", result)

    [<SvgIdProperty>]
    let ``anchor with any safe url and one element always has balanced angle brackets`` (url: string) =
        let circle = Circle.create Point.origin (Length.ofInt 10) |> Element.create
        let result = Anchor.create url |> Anchor.addElement circle |> Anchor.toString
        isMatched '<' '>' result

    [<SvgIdProperty>]
    let ``anchor with any safe url always contains it in href attribute`` (url: string) =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let result = Anchor.create url |> Anchor.addElement circle |> Anchor.toString
        result.Contains(sprintf "href=\"%s\"" url)

    [<Property>]
    let ``anchor with N elements always contains N child elements`` (n: PositiveInt) =
        let count = min n.Get 20
        let elements = List.init count (fun i -> Circle.create (Point.ofInts (i, i)) (Length.ofInt (i + 1)) |> Element.create)
        let result = Anchor.create "http://x.com" |> Anchor.addElements elements |> Anchor.toString
        result.Split("<circle").Length - 1 = count
