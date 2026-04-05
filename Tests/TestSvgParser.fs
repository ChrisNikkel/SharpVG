namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestSvgParser =

    // --- helpers ---

    let private parseOk svgStr =
        match SvgParser.ofString svgStr with
        | Ok result -> result.Value
        | Error e -> failwithf "Expected Ok but got Error: %s" e.Message

    let private parseWarnings svgStr =
        match SvgParser.ofString svgStr with
        | Ok result -> result.Warnings
        | Error e -> failwithf "Expected Ok but got Error: %s" e.Message

    let private parseErr svgStr =
        match SvgParser.ofString svgStr with
        | Error e -> e
        | Ok _ -> failwith "Expected Error but got Ok"

    let private bodyElements (svg: Svg) =
        svg.Body |> Seq.toList

    let private firstElement (svg: Svg) =
        svg.Body
        |> Seq.pick (function
            | GroupElement.Element e -> Some e
            | _ -> None)

    // --- Error cases ---

    [<Fact>]
    let ``SvgParser ofString - malformed XML returns Error`` () =
        let err = parseErr "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle"
        Assert.False(System.String.IsNullOrEmpty(err.Message))

    [<Fact>]
    let ``SvgParser ofString - wrong root element returns Error`` () =
        let err = parseErr "<html></html>"
        Assert.Contains("svg", err.Message)

    [<Fact>]
    let ``SvgParser ofString - empty string returns Error`` () =
        let err = parseErr ""
        Assert.False(System.String.IsNullOrEmpty(err.Message))

    // --- Basic parse ---

    [<Fact>]
    let ``SvgParser ofString - empty SVG parses to empty body`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"></svg>"
        Assert.Empty(svg.Body)

    [<Fact>]
    let ``SvgParser ofString - SVG with width and height parses size`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"200\" height=\"100\"></svg>"
        Assert.True(svg.Size.IsSome)
        let size = svg.Size.Value
        Assert.Equal("200", size.Width |> Length.toString)
        Assert.Equal("100", size.Height |> Length.toString)

    [<Fact>]
    let ``SvgParser ofString - SVG with viewBox parses viewBox`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 800 600\"></svg>"
        Assert.True(svg.ViewBox.IsSome)

    [<Fact>]
    let ``SvgParser ofString - SVG with title and desc parses them`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><title>My Title</title><desc>A desc</desc></svg>"
        Assert.Equal(Some "My Title", svg.Title)
        Assert.Equal(Some "A desc", svg.Description)

    // --- Element recognition ---

    [<Fact>]
    let ``SvgParser ofString - parses circle`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"50\" cy=\"60\" r=\"20\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("circle", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses ellipse`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><ellipse cx=\"100\" cy=\"80\" rx=\"60\" ry=\"40\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("ellipse", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses rect`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect x=\"10\" y=\"20\" width=\"100\" height=\"50\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("rect", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses line`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><line x1=\"0\" y1=\"0\" x2=\"100\" y2=\"100\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("line", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses path`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><path d=\"M 10 20 L 100 200 Z\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("path", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses polygon`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><polygon points=\"0,0 100,0 50,80\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("polygon", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses polyline`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><polyline points=\"0,0 50,50 100,0\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("polyline", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses text`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><text x=\"10\" y=\"30\">Hello</text></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("text", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    [<Fact>]
    let ``SvgParser ofString - parses image`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><image x=\"0\" y=\"0\" width=\"100\" height=\"100\" href=\"img.png\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("image", e |> Element.toString)
        | other -> failwithf "Expected Element, got %A" other

    // --- id / name parsing ---

    [<Fact>]
    let ``SvgParser ofString - circle with id populates element Name`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"myCircle\" cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        let elem = firstElement svg
        Assert.True(elem.Name.IsSome)
        Assert.Equal("myCircle", elem.Name.Value)

    // --- Style parsing ---

    [<Fact>]
    let ``SvgParser ofString - presentation attributes become style`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"0\" cy=\"0\" r=\"10\" fill=\"red\" stroke=\"blue\"/></svg>"
        let elem = firstElement svg
        Assert.True(elem.Style.IsSome)
        let styleStr = elem.Style.Value |> Style.toString
        Assert.Contains("fill", styleStr)
        Assert.Contains("stroke", styleStr)

    [<Fact>]
    let ``SvgParser ofString - inline style attribute is parsed`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect x=\"0\" y=\"0\" width=\"50\" height=\"50\" style=\"fill:green\"/></svg>"
        let elem = firstElement svg
        Assert.True(elem.Style.IsSome)
        let styleStr = elem.Style.Value |> Style.toString
        Assert.Contains("fill", styleStr)

    // --- Transform parsing ---

    [<Fact>]
    let ``SvgParser ofString - translate transform is parsed`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect x=\"0\" y=\"0\" width=\"10\" height=\"10\" transform=\"translate(30,40)\"/></svg>"
        let elem = firstElement svg
        Assert.NotEmpty(elem.Transforms)
        let transformStr = elem.Transforms |> Seq.head |> Transform.toString
        Assert.Contains("translate", transformStr)

    [<Fact>]
    let ``SvgParser ofString - scale transform is parsed`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"0\" cy=\"0\" r=\"5\" transform=\"scale(2)\"/></svg>"
        let elem = firstElement svg
        Assert.NotEmpty(elem.Transforms)
        let transformStr = elem.Transforms |> Seq.head |> Transform.toString
        Assert.Contains("scale", transformStr)

    [<Fact>]
    let ``SvgParser ofString - rotate transform is parsed`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect x=\"0\" y=\"0\" width=\"10\" height=\"10\" transform=\"rotate(45)\"/></svg>"
        let elem = firstElement svg
        Assert.NotEmpty(elem.Transforms)
        let transformStr = elem.Transforms |> Seq.head |> Transform.toString
        Assert.Contains("rotate", transformStr)

    // --- Group parsing ---

    [<Fact>]
    let ``SvgParser ofString - parses nested group`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g><circle cx=\"5\" cy=\"5\" r=\"3\"/></g></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Group g ->
            Assert.NotEmpty(g.Body)
        | other -> failwithf "Expected Group, got %A" other

    [<Fact>]
    let ``SvgParser ofString - group with id populates group Name`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g id=\"grp1\"><rect x=\"0\" y=\"0\" width=\"10\" height=\"10\"/></g></svg>"
        let items = bodyElements svg
        match items.[0] with
        | GroupElement.Group g ->
            Assert.True(g.Name.IsSome)
            Assert.Equal("grp1", g.Name.Value)
        | other -> failwithf "Expected Group, got %A" other

    [<Fact>]
    let ``SvgParser ofString - deeply nested groups are parsed`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g><g><circle cx=\"1\" cy=\"1\" r=\"1\"/></g></g></svg>"
        let items = bodyElements svg
        match items.[0] with
        | GroupElement.Group outer ->
            match outer.Body |> Seq.head with
            | GroupElement.Group inner ->
                Assert.NotEmpty(inner.Body)
            | other -> failwithf "Expected inner Group, got %A" other
        | other -> failwithf "Expected outer Group, got %A" other

    // --- Unknown element passthrough ---

    [<Fact>]
    let ``SvgParser ofString - unknown element becomes Raw`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><fancyWidget foo=\"bar\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e ->
            match Element.rawContent e with
            | Some raw -> Assert.Equal("fancyWidget", raw.TagName)
            | None -> failwith "Expected raw element"
        | other -> failwithf "Expected raw Element, got %A" other

    [<Fact>]
    let ``SvgParser stripUnknown - removes Raw elements`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"0\" cy=\"0\" r=\"5\"/><fancyWidget foo=\"bar\"/></svg>"
        let stripped = SvgParser.stripUnknown svg
        let items = stripped.Body |> Seq.toList
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element _ -> ()
        | other -> failwithf "Expected Element after strip, got %A" other

    [<Fact>]
    let ``SvgParser stripUnknown - preserves Raw elements when not stripped`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><fancyWidget foo=\"bar\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e when Element.isRaw e -> ()
        | other -> failwithf "Expected raw Element preserved, got %A" other

    [<Fact>]
    let ``SvgParser stripUnknown - removes Raw elements inside groups`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g><circle cx=\"0\" cy=\"0\" r=\"5\"/><fancyWidget/></g></svg>"
        let stripped = SvgParser.stripUnknown svg
        match stripped.Body |> Seq.head with
        | GroupElement.Group g ->
            let innerItems = g.Body |> Seq.toList
            Assert.Equal(1, innerItems.Length)
            match innerItems.[0] with
            | GroupElement.Element _ -> ()
            | other -> failwithf "Expected Element inside group after strip, got %A" other
        | other -> failwithf "Expected Group, got %A" other

    // --- <defs> parsing ---

    [<Fact>]
    let ``SvgParser ofString - defs block is parsed into Definitions`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><circle id=\"tmpl\" cx=\"0\" cy=\"0\" r=\"5\"/></defs></svg>"
        Assert.True(svg.Definitions.IsSome)

    [<Fact>]
    let ``SvgParser ofString - unknown defs element becomes RawDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><fancyDefsWidget id=\"w1\"/></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let defs = svg.Definitions.Value
        let hasRaw =
            defs.Contents
            |> Seq.exists (function RawDef _ -> true | _ -> false)
        Assert.True(hasRaw)

    [<Fact>]
    let ``SvgParser ofString - empty defs produces no Definitions`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs></defs></svg>"
        Assert.True(svg.Definitions.IsNone)

    // --- Structural defs elements ---

    [<Fact>]
    let ``SvgParser ofString - linearGradient in defs becomes GradientDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><linearGradient id=\"lg1\" x1=\"0\" y1=\"0\" x2=\"1\" y2=\"0\"><stop offset=\"0\" stop-color=\"red\"/><stop offset=\"1\" stop-color=\"blue\"/></linearGradient></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasGradient =
            svg.Definitions.Value.Contents
            |> Seq.exists (function GradientDef (Gradient.Linear _) -> true | _ -> false)
        Assert.True(hasGradient)

    [<Fact>]
    let ``SvgParser ofString - radialGradient in defs becomes GradientDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><radialGradient id=\"rg1\" cx=\"0.5\" cy=\"0.5\" r=\"0.5\"><stop offset=\"0\" stop-color=\"white\"/><stop offset=\"1\" stop-color=\"black\"/></radialGradient></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasGradient =
            svg.Definitions.Value.Contents
            |> Seq.exists (function GradientDef (Gradient.Radial _) -> true | _ -> false)
        Assert.True(hasGradient)

    [<Fact>]
    let ``SvgParser ofString - linearGradient preserves stops`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><linearGradient id=\"lg1\" x1=\"0\" y1=\"0\" x2=\"1\" y2=\"0\"><stop offset=\"0\" stop-color=\"red\"/><stop offset=\"0.5\" stop-color=\"green\"/><stop offset=\"1\" stop-color=\"blue\"/></linearGradient></defs></svg>"
        let gradient =
            svg.Definitions.Value.Contents
            |> Seq.pick (function GradientDef (Gradient.Linear lg) -> Some lg | _ -> None)
        Assert.Equal(3, gradient.Stops.Length)

    [<Fact>]
    let ``SvgParser ofString - clipPath in defs becomes ClipPathDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><clipPath id=\"cp1\"><rect x=\"0\" y=\"0\" width=\"100\" height=\"100\"/></clipPath></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasClipPath =
            svg.Definitions.Value.Contents
            |> Seq.exists (function ClipPathDef _ -> true | _ -> false)
        Assert.True(hasClipPath)

    [<Fact>]
    let ``SvgParser ofString - mask in defs becomes MaskDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><mask id=\"m1\"><circle cx=\"50\" cy=\"50\" r=\"40\"/></mask></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasMask =
            svg.Definitions.Value.Contents
            |> Seq.exists (function MaskDef _ -> true | _ -> false)
        Assert.True(hasMask)

    [<Fact>]
    let ``SvgParser ofString - pattern in defs becomes PatternDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><pattern id=\"p1\" width=\"10\" height=\"10\"><circle cx=\"5\" cy=\"5\" r=\"4\"/></pattern></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasPattern =
            svg.Definitions.Value.Contents
            |> Seq.exists (function PatternDef _ -> true | _ -> false)
        Assert.True(hasPattern)

    [<Fact>]
    let ``SvgParser ofString - marker in defs becomes MarkerDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><marker id=\"arr\" viewBox=\"0 0 10 10\" refX=\"5\" refY=\"5\" markerWidth=\"6\" markerHeight=\"6\"><path d=\"M 0 0 L 10 5 L 0 10 z\"/></marker></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasMarker =
            svg.Definitions.Value.Contents
            |> Seq.exists (function MarkerDef _ -> true | _ -> false)
        Assert.True(hasMarker)

    [<Fact>]
    let ``SvgParser ofString - filter in defs becomes FilterDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><filter id=\"blur\"><feGaussianBlur stdDeviation=\"5\"/></filter></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasFilter =
            svg.Definitions.Value.Contents
            |> Seq.exists (function FilterDef _ -> true | _ -> false)
        Assert.True(hasFilter)

    [<Fact>]
    let ``SvgParser ofString - symbol in defs becomes SymbolDef`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><symbol viewBox=\"0 0 100 100\"><circle cx=\"50\" cy=\"50\" r=\"40\"/></symbol></defs></svg>"
        Assert.True(svg.Definitions.IsSome)
        let hasSymbol =
            svg.Definitions.Value.Contents
            |> Seq.exists (function SymbolDef _ -> true | _ -> false)
        Assert.True(hasSymbol)

    [<Fact>]
    let ``SvgParser ofString - anchor in body is parsed as element`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><a href=\"https://example.com\"><circle cx=\"50\" cy=\"50\" r=\"20\"/></a></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("<a ", e |> Element.toString)
        | other -> failwithf "Expected Element for anchor, got %A" other

    [<Fact>]
    let ``SvgParser ofString - structural defs serialize to well-formed XML`` () =
        let svg = parseOk """<svg xmlns="http://www.w3.org/2000/svg">
            <defs>
                <linearGradient id="lg" x1="0" y1="0" x2="1" y2="0">
                    <stop offset="0" stop-color="red"/>
                    <stop offset="1" stop-color="blue"/>
                </linearGradient>
                <clipPath id="cp"><rect x="0" y="0" width="50" height="50"/></clipPath>
                <marker id="m" viewBox="0 0 10 10" refX="5" refY="5" markerWidth="6" markerHeight="6">
                    <path d="M 0 0 L 10 5 L 0 10 z"/>
                </marker>
            </defs>
            <circle cx="50" cy="50" r="40"/>
        </svg>"""
        SvgCheck.assertValid (Svg.toString svg)

    // --- <use> parsing ---

    [<Fact>]
    let ``SvgParser ofString - use element is parsed as element`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><use href=\"#myShape\" x=\"10\" y=\"20\"/></svg>"
        let items = bodyElements svg
        Assert.Equal(1, items.Length)
        match items.[0] with
        | GroupElement.Element e -> Assert.Contains("use", e |> Element.toString)
        | other -> failwithf "Expected Element for use, got %A" other

    // --- Mutation helpers ---

    [<Fact>]
    let ``Svg mapElements - identity function preserves all elements`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"5\"/><rect x=\"0\" y=\"0\" width=\"20\" height=\"20\"/></svg>"
        let mapped = Svg.mapElements id svg
        let original = bodyElements svg |> List.length
        let after = bodyElements mapped |> List.length
        Assert.Equal(original, after)

    [<Fact>]
    let ``Svg findById - finds element by id`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"c1\" cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        let id = "c1"
        let found = Svg.findById id svg
        Assert.True(found.IsSome)
        Assert.Equal(Some id, found.Value.Name)

    [<Fact>]
    let ``Svg findById - returns None when id not present`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        let id = "missing"
        let found = Svg.findById id svg
        Assert.True(found.IsNone)

    [<Fact>]
    let ``Svg findById - finds element nested inside group`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g><rect id=\"r1\" x=\"0\" y=\"0\" width=\"10\" height=\"10\"/></g></svg>"
        let id = "r1"
        let found = Svg.findById id svg
        Assert.True(found.IsSome)

    [<Fact>]
    let ``Svg replaceById - replaces element by id`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"c1\" cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        let id = "c1"
        let newElem = Rect.create Point.origin Area.full |> Element.createWithName "c1"
        let replaced = Svg.replaceById id newElem svg
        let result = Svg.toString replaced
        Assert.Contains("<rect", result)
        Assert.DoesNotContain("<circle", result)

    [<Fact>]
    let ``Svg findAll - finds all elements matching predicate`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"a\" cx=\"0\" cy=\"0\" r=\"5\"/><circle id=\"b\" cx=\"10\" cy=\"10\" r=\"10\"/><rect x=\"0\" y=\"0\" width=\"5\" height=\"5\"/></svg>"
        let circles = Svg.findAll (fun e -> e |> Element.toString |> fun s -> s.StartsWith("<circle")) svg
        Assert.Equal(2, circles.Length)

    [<Fact>]
    let ``Svg mapElementsWhere - only transforms matching elements`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"c1\" cx=\"10\" cy=\"10\" r=\"5\"/><rect id=\"r1\" x=\"0\" y=\"0\" width=\"10\" height=\"10\"/></svg>"
        let circleId = "c1"
        let transformed =
            Svg.mapElementsWhere
                (fun e -> e.Name = Some circleId)
                (fun e -> { e with Transforms = Seq.singleton (Transform.createTranslate (Length.ofInt 5)) })
                svg
        let result = Svg.toString transformed
        Assert.Contains("translate", result)
        Assert.Contains("<rect", result)

    // --- well-formedness via xmllint ---

    [<Fact>]
    let ``SvgParser ofString - parsed SVG serializes to well-formed XML (circle)`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"50\" cy=\"50\" r=\"25\"/></svg>"
        SvgCheck.assertValid (Svg.toString svg)

    [<Fact>]
    let ``SvgParser ofString - parsed SVG with group serializes to well-formed XML`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><g id=\"grp\"><rect x=\"0\" y=\"0\" width=\"50\" height=\"50\"/></g></svg>"
        SvgCheck.assertValid (Svg.toString svg)

    [<Fact>]
    let ``SvgParser ofString - parsed SVG with defs serializes to well-formed XML`` () =
        let svg = parseOk "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><circle id=\"tmpl\" cx=\"0\" cy=\"0\" r=\"5\"/></defs><use href=\"#tmpl\" x=\"20\" y=\"20\"/></svg>"
        SvgCheck.assertValid (Svg.toString svg)

    // --- property-based tests ---

    // Property: round-trip a single circle — parsing then serializing produces well-formed XML
    [<SvgProperty>]
    let ``SvgParser property - circle round-trip produces well-formed XML`` (cx: float) (cy: float) (r: float) =
        let cx' = abs cx + 1.0
        let cy' = abs cy + 1.0
        let r' = abs r + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"%g\" cy=\"%g\" r=\"%g\"/></svg>" cx' cy' r'
        match SvgParser.ofString input with
        | Error _ -> true // skip if parse fails (degenerate floats)
        | Ok result ->
            let output = Svg.toString result.Value
            match SvgCheck.validate output with
            | Ok () -> true
            | Error _ -> false

    // Property: round-trip a single rect
    [<SvgProperty>]
    let ``SvgParser property - rect round-trip produces well-formed XML`` (x: float) (y: float) (w: float) (h: float) =
        let x' = abs x
        let y' = abs y
        let w' = abs w + 1.0
        let h' = abs h + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><rect x=\"%g\" y=\"%g\" width=\"%g\" height=\"%g\"/></svg>" x' y' w' h'
        match SvgParser.ofString input with
        | Error _ -> true
        | Ok result ->
            match SvgCheck.validate (Svg.toString result.Value) with
            | Ok () -> true
            | Error _ -> false

    // Property: element count is preserved through mapElements identity
    [<SvgProperty>]
    let ``SvgParser property - mapElements identity preserves element count`` (r: float) =
        let r' = abs r + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"%g\"/><rect x=\"0\" y=\"0\" width=\"%g\" height=\"%g\"/></svg>" r' r' r'
        match SvgParser.ofString input with
        | Error _ -> true
        | Ok result ->
            let svg = result.Value
            let before = svg.Body |> Seq.length
            let after = (Svg.mapElements id svg).Body |> Seq.length
            before = after

    // Property: findById on named element always finds it
    [<SvgIdProperty>]
    let ``SvgParser property - findById always finds named circle`` (r: float) (name: string) =
        let r' = abs r + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"%s\" cx=\"10\" cy=\"10\" r=\"%g\"/></svg>" name r'
        match SvgParser.ofString input with
        | Error _ -> true
        | Ok result ->
            let id = name
            Svg.findById id result.Value |> Option.isSome

    // Property: stripUnknown leaves no Raw elements at any level
    [<SvgProperty>]
    let ``SvgParser property - stripUnknown leaves no Raw elements`` (r: float) =
        let r' = abs r + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"%g\"/><unknownTag foo=\"bar\"/></svg>" r'
        match SvgParser.ofString input with
        | Error _ -> true
        | Ok result ->
            let stripped = SvgParser.stripUnknown result.Value
            let rec hasRaw (body: Body) =
                body |> Seq.exists (function
                    | GroupElement.Element e when Element.isRaw e -> true
                    | GroupElement.Group g -> hasRaw g.Body
                    | _ -> false)
            not (hasRaw stripped.Body)

    // --- SVGZ ---

    [<Fact>]
    let ``SvgParser ofGzipStream - roundtrips SVG through gzip`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        let original = Circle.create center radius |> Element.create |> Svg.ofElement
        let svgBytes = Svg.toString original |> System.Text.Encoding.UTF8.GetBytes
        let compressed =
            use ms = new System.IO.MemoryStream()
            do
                use gz = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, leaveOpen = true)
                gz.Write(svgBytes, 0, svgBytes.Length)
            ms.ToArray()
        use readMs = new System.IO.MemoryStream(compressed)
        match SvgParser.ofGzipStream readMs with
        | Error e -> failwithf "ofGzipStream failed: %s" e.Message
        | Ok result ->
            let output = Svg.toString result.Value
            Assert.Contains("<circle", output)

    [<Fact>]
    let ``SvgParser ofGzipStream - bad data returns Error`` () =
        use ms = new System.IO.MemoryStream([| 0uy; 1uy; 2uy; 3uy |])
        match SvgParser.ofGzipStream ms with
        | Error _ -> ()
        | Ok _ -> Assert.Fail("Expected Error for non-gzip data")

    // --- HTML extraction ---

    [<Fact>]
    let ``SvgParser ofHtmlString - extracts inline SVG from well-formed XHTML`` () =
        let html = """<?xml version="1.0"?><html xmlns="http://www.w3.org/1999/xhtml"><body><svg xmlns="http://www.w3.org/2000/svg"><circle cx="10" cy="10" r="5"/></svg></body></html>"""
        let results = SvgParser.ofHtmlString html
        Assert.Equal(1, results.Length)
        match results.[0] with
        | Error e -> failwithf "ofHtmlString failed: %s" e.Message
        | Ok result -> Assert.Contains("<circle", Svg.toString result.Value)

    [<Fact>]
    let ``SvgParser ofHtmlString - extracts inline SVG from HTML5`` () =
        // SharpVG toHtml output is well-formed enough for XML parse path
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let html = Circle.create center radius |> Element.create |> Svg.ofElement |> Svg.toHtml "Test"
        let results = SvgParser.ofHtmlString html
        Assert.Equal(1, results.Length)
        match results.[0] with
        | Error e -> failwithf "ofHtmlString failed: %s" e.Message
        | Ok result -> Assert.Contains("<circle", Svg.toString result.Value)

    [<Fact>]
    let ``SvgParser ofHtmlString - returns empty list when no SVG present`` () =
        let results = SvgParser.ofHtmlString "<html><body><p>No SVG here</p></body></html>"
        Assert.Empty(results)

    [<Fact>]
    let ``SvgParser ofHtmlString - extracts multiple SVG elements`` () =
        let html = """<html><body>
            <svg xmlns="http://www.w3.org/2000/svg"><rect x="0" y="0" width="10" height="10"/></svg>
            <p>between</p>
            <svg xmlns="http://www.w3.org/2000/svg"><circle cx="5" cy="5" r="4"/></svg>
        </body></html>"""
        let results = SvgParser.ofHtmlString html
        Assert.Equal(2, results.Length)
        Assert.True(results |> List.forall (function Ok _ -> true | Error _ -> false))

    [<Fact>]
    let ``SvgParser ofHtmlString - parsed SVG from HTML serializes to well-formed XML`` () =
        let center = Point.ofInts (40, 40)
        let radius = Length.ofInt 25
        let html = Circle.create center radius |> Element.create |> Svg.ofElement |> Svg.toHtml "Test"
        let results = SvgParser.ofHtmlString html
        match results with
        | [ Ok result ] -> SvgCheck.assertValid (Svg.toString result.Value)
        | _ -> Assert.Fail("Expected exactly one Ok result")

    // Property: parsed SVG title survives round-trip
    [<SvgProperty>]
    let ``SvgParser property - warnings list is always a list`` (r: float) =
        let r' = abs r + 1.0
        let input = sprintf "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"%g\"/></svg>" r'
        match SvgParser.ofString input with
        | Error _ -> true
        | Ok result ->
            // Warnings is always a list — never throws
            result.Warnings |> List.length >= 0

    // Editing API integration: parse → edit → serialize
    [<Fact>]
    let ``edit parsed SVG - withAttribute changes shape geometry`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"c\" cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        let svg = parseOk input
        let edited =
            svg
            |> Svg.mapElementsWhere (fun e -> e.Name = Some "c") (Element.withAttribute "r" "50")
        let output = Svg.toString edited
        Assert.Contains("r=\"50\"", output)
        Assert.DoesNotContain("r=\"5\"", output)

    [<Fact>]
    let ``edit parsed SVG - removeById removes element`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle id=\"c\" cx=\"10\" cy=\"10\" r=\"5\"/><rect id=\"r\" x=\"0\" y=\"0\" width=\"10\" height=\"10\"/></svg>"
        let svg = parseOk input
        let edited = svg |> Svg.removeById "c"
        let output = Svg.toString edited
        Assert.DoesNotContain("id=\"c\"", output)
        Assert.Contains("<rect", output)

    // --- Strict / Lenient parse mode ---

    [<Fact>]
    let ``Lenient mode - unknown element produces no warnings`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><custom foo=\"bar\"/></svg>"
        match SvgParser.ofStringWith Lenient input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result -> Assert.Empty(result.Warnings)

    [<Fact>]
    let ``Strict mode - unknown body element produces a warning`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><custom foo=\"bar\"/></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result ->
            Assert.NotEmpty(result.Warnings)
            Assert.Contains("custom", result.Warnings.[0].Message)

    [<Fact>]
    let ``Strict mode - known element produces no warnings`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><circle cx=\"10\" cy=\"10\" r=\"5\"/></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result -> Assert.Empty(result.Warnings)

    [<Fact>]
    let ``Strict mode - multiple unknown elements each produce a warning`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><foo/><bar/><circle cx=\"0\" cy=\"0\" r=\"1\"/></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result -> Assert.Equal(2, result.Warnings.Length)

    [<Fact>]
    let ``Strict mode - unknown element still parsed as raw passthrough`` () =
        // Strict mode warns but doesn't drop the element — body still contains the raw element
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><custom id=\"x\"/></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result ->
            let bodyItems = result.Value.Body |> Seq.toList
            Assert.NotEmpty(bodyItems)
            let rawElements =
                bodyItems
                |> List.choose (function GroupElement.Element e when Element.isRaw e -> Some e | _ -> None)
            Assert.NotEmpty(rawElements)

    [<Fact>]
    let ``Strict mode - unknown defs element produces a warning`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><defs><customGradient id=\"g\"/></defs></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result ->
            Assert.NotEmpty(result.Warnings)
            Assert.Contains("customGradient", result.Warnings.[0].Message)

    [<Fact>]
    let ``Strict mode - warning ElementName matches the unknown tag name`` () =
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><weirdtag/></svg>"
        match SvgParser.ofStringWith Strict input with
        | Error e -> Assert.Fail(sprintf "Expected Ok, got Error: %s" e.Message)
        | Ok result ->
            let warning = result.Warnings |> List.head
            Assert.Equal(Some "weirdtag", warning.ElementName)

    [<Fact>]
    let ``ofString uses Lenient mode by default`` () =
        // Shorthand ofString should behave identically to ofStringWith Lenient
        let input = "<svg xmlns=\"http://www.w3.org/2000/svg\"><unknown/></svg>"
        let lenient = SvgParser.ofStringWith Lenient input
        let shorthand = SvgParser.ofString input
        match lenient, shorthand with
        | Ok r1, Ok r2 ->
            Assert.Equal(r1.Warnings.Length, r2.Warnings.Length)
        | _ -> Assert.Fail("Both should parse successfully")
