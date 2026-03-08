namespace SharpVG.Tests

open SharpVG
open Xunit

module TestSvg =

    [<Fact>]
    let ``create empty SVG`` () =
        Assert.Equal("<!DOCTYPE html>\n<html>\n<head>\n<title>empty</title>\n</head>\n<body>\n<svg xmlns=\"http://www.w3.org/2000/svg\"></svg>\n</body>\n</html>\n", ([] |> Svg.ofList |> Svg.toHtml "empty"))

    [<Fact>]
    let ``SVG toString`` () =
        let result = [] |> Svg.ofList |> Svg.toString
        Assert.Equal("<svg xmlns=\"http://www.w3.org/2000/svg\"></svg>", result)

    [<Fact>]
    let ``SVG ofList with elements`` () =
        let circle = Circle.create Point.origin (Length.ofInt 10) |> Element.create
        let rect = Rect.create Point.origin Area.full |> Element.create
        let result = [circle; rect] |> Svg.ofList |> Svg.toString
        Assert.Contains("<circle", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``SVG ofArray`` () =
        let elements = [| Circle.create Point.origin (Length.ofInt 5) |> Element.create |]
        let result = elements |> Svg.ofArray |> Svg.toString
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``SVG withSize`` () =
        let size = Area.ofInts (200, 100)
        let result = [] |> Svg.ofList |> Svg.withSize size |> Svg.toString
        Assert.Contains("width=\"200\"", result)
        Assert.Contains("height=\"100\"", result)

    [<Fact>]
    let ``SVG withViewBox`` () =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (200, 100))
        let result = [] |> Svg.ofList |> Svg.withViewBox viewBox |> Svg.toString
        Assert.Contains("viewBox=\"0,0 200,100\"", result)

    [<Fact>]
    let ``SVG ofGroup`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let group = Group.ofList [circle]
        let result = group |> Svg.ofGroup |> Svg.toString
        Assert.Contains("<g>", result)
        Assert.Contains("<circle", result)
        Assert.Contains("width=\"100%\"", result)
        Assert.Contains("height=\"100%\"", result)
