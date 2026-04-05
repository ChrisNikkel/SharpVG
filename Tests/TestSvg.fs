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

    [<Fact>]
    let ``SVG toHtmlWithCss includes style block`` () =
        let center = Point.ofInts (60, 60)
        let radius = Length.ofInt 50
        let css = "circle { cursor: pointer; }"
        let result =
            Circle.create center radius
            |> Element.create
            |> Svg.ofElement
            |> Svg.toHtmlWithCss "Example" css
        Assert.Contains("<title>Example</title>", result)
        Assert.Contains("<style>", result)
        Assert.Contains("circle { cursor: pointer; }", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``SVG toHtmlWithCss empty css matches toHtml`` () =
        let circle = Circle.create Point.origin (Length.ofInt 10) |> Element.create
        let htmlResult = circle |> Svg.ofElement |> Svg.toHtml "Test"
        let cssResult = circle |> Svg.ofElement |> Svg.toHtmlWithCss "Test" ""
        Assert.Equal(htmlResult, cssResult)

    [<Fact>]
    let ``SVG withTitle adds title element`` () =
        let result = [] |> Svg.ofList |> Svg.withTitle "My Chart" |> Svg.toString
        Assert.Contains("<title>My Chart</title>", result)

    [<Fact>]
    let ``SVG withDescription adds desc element`` () =
        let result = [] |> Svg.ofList |> Svg.withDescription "A description" |> Svg.toString
        Assert.Contains("<desc>A description</desc>", result)

    // Wiki: ViewBox page — setting viewBox on SVG
    [<Fact>]
    let ``ViewBox wiki - svg with size and viewBox`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        let circle = Circle.create center radius |> Element.create
        let size = Area.ofInts (400, 400)
        let viewBoxMin = Point.ofInts (0, 0)
        let viewBoxSize = Area.ofInts (100, 100)
        let viewBox = ViewBox.create viewBoxMin viewBoxSize
        let result =
            circle
            |> Svg.ofElement
            |> Svg.withSize size
            |> Svg.withViewBox viewBox
            |> Svg.toString
        Assert.Contains("width=\"400\"", result)
        Assert.Contains("height=\"400\"", result)
        Assert.Contains("viewBox=\"0,0 100,100\"", result)

    // Editing API — structural helpers
    [<Fact>]
    let ``addElement appends element to SVG body`` () =
        let circle = Circle.create (Point.ofInts (10, 10)) (Length.ofInt 5) |> Element.create
        let rect = Rect.create (Point.ofInts (0, 0)) (Area.ofInts (20, 20)) |> Element.create
        let result =
            circle
            |> Svg.ofElement
            |> Svg.addElement rect
            |> Svg.toString
        Assert.Contains("<circle", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``addElements appends multiple elements to SVG body`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let rect1 = Rect.create Point.origin (Area.ofInts (10, 10)) |> Element.create
        let rect2 = Rect.create Point.origin (Area.ofInts (20, 20)) |> Element.create
        let result =
            circle
            |> Svg.ofElement
            |> Svg.addElements [rect1; rect2]
            |> Svg.toString
        Assert.Contains("<circle", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``addGroup appends group to SVG body`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let group = [circle] |> Group.ofList
        let result =
            [] |> Svg.ofList |> Svg.addGroup group |> Svg.toString
        Assert.Contains("<g>", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``removeById removes the named element`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "target"
        let rect = Rect.create Point.origin (Area.ofInts (10, 10)) |> Element.create
        let result =
            [circle; rect]
            |> Svg.ofList
            |> Svg.removeById "target"
            |> Svg.toString
        Assert.DoesNotContain("id=\"target\"", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``removeById removes element nested in group`` () =
        let inner = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "inner"
        let group = [inner] |> Group.ofList
        let result =
            group
            |> Svg.ofGroup
            |> Svg.removeById "inner"
            |> Svg.toString
        Assert.DoesNotContain("id=\"inner\"", result)

    [<Fact>]
    let ``removeWhere removes matching elements`` () =
        let small = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "small"
        let large = Circle.create Point.origin (Length.ofInt 50) |> Element.createWithName "large"
        let result =
            [small; large]
            |> Svg.ofList
            |> Svg.removeWhere (fun e -> e.Name = Some "small")
            |> Svg.toString
        Assert.DoesNotContain("id=\"small\"", result)
        Assert.Contains("id=\"large\"", result)

    // Editor rendering support
    [<Fact>]
    let ``toStringForEditing injects data-edit-id on top-level elements`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let rect = Rect.create Point.origin (Area.ofInts (10, 10)) |> Element.create
        let result = [circle; rect] |> Svg.ofList |> Svg.toStringForEditing
        Assert.Contains("data-edit-id=\"0\"", result)
        Assert.Contains("data-edit-id=\"1\"", result)

    [<Fact>]
    let ``toStringForEditing injects nested path for elements inside groups`` () =
        let inner = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let group = [inner] |> Group.ofList
        let result = group |> Svg.ofGroup |> Svg.toStringForEditing
        Assert.Contains("data-edit-id=\"0:0\"", result)

    [<Fact>]
    let ``toStringForEditing does not affect model toString output`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let svg = [circle] |> Svg.ofList
        let normal = svg |> Svg.toString
        let forEditing = svg |> Svg.toStringForEditing
        Assert.DoesNotContain("data-edit-id", normal)
        Assert.Contains("data-edit-id", forEditing)

    [<Fact>]
    let ``parseEditPath parses colon-separated integers`` () =
        Assert.Equal(Some [1; 2; 0], Svg.parseEditPath "1:2:0")
        Assert.Equal(Some [0], Svg.parseEditPath "0")
        Assert.Equal(None, Svg.parseEditPath "abc")
        Assert.Equal(None, Svg.parseEditPath "")

    [<Fact>]
    let ``findAtEditPath finds top-level element`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "c"
        let svg = [circle] |> Svg.ofList
        let found = Svg.findAtEditPath [0] svg
        Assert.Equal(Some "c", found |> Option.bind Element.tryGetName)

    [<Fact>]
    let ``findAtEditPath finds element inside group`` () =
        let inner = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "inner"
        let group = [inner] |> Group.ofList
        let svg = group |> Svg.ofGroup
        let found = Svg.findAtEditPath [0; 0] svg
        Assert.Equal(Some "inner", found |> Option.bind Element.tryGetName)

    [<Fact>]
    let ``mapAtEditPath transforms element at path`` () =
        let circle = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        let svg = [circle] |> Svg.ofList
        let result =
            svg
            |> Svg.mapAtEditPath [0] (Element.withAttribute "r" "99")
            |> Svg.toString
        Assert.Contains("r=\"99\"", result)
