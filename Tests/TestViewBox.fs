namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestViewBox =

    [<Fact>]
    let ``create and toAttributes`` () =
        let vb = ViewBox.create Point.origin Area.full
        let result = vb |> ViewBox.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Equal("viewBox=\"0,0 100%,100%\"", result)

    [<Fact>]
    let ``create with specific values`` () =
        let vb = ViewBox.create (Point.ofInts (10, 20)) (Area.ofInts (200, 100))
        let result = vb |> ViewBox.toAttributes |> List.map Attribute.toString |> String.concat " "
        Assert.Equal("viewBox=\"10,20 200,100\"", result)

    [<Fact>]
    let ``toAttributes returns single attribute`` () =
        let vb = ViewBox.create Point.origin (Area.ofInts (800, 600))
        Assert.Equal(1, vb |> ViewBox.toAttributes |> List.length)

    [<SvgProperty>]
    let ``viewBox with any positive dimensions always produces one attribute starting with viewBox`` (x: float, y: float, w: float, h: float) =
        let vb = ViewBox.create (Point.ofFloats (x, y)) (Area.ofFloats (w, h))
        let attrs = vb |> ViewBox.toAttributes
        attrs.Length = 1 && (attrs |> List.map Attribute.toString |> List.head).StartsWith("viewBox=\"")

    [<SvgProperty>]
    let ``SVG with any viewBox always produces balanced SVG tag`` (x: float, y: float, w: float, h: float) =
        let viewBox = ViewBox.create (Point.ofFloats (x, y)) (Area.ofFloats (w, h))
        let result = [] |> Svg.ofList |> Svg.withViewBox viewBox |> Svg.toString
        checkTag "svg" result

    [<SvgProperty>]
    let ``viewBox attribute always has balanced quotes`` (x: float, y: float, w: float, h: float) =
        let vb = ViewBox.create (Point.ofFloats (x, y)) (Area.ofFloats (w, h))
        let attr = vb |> ViewBox.toAttributes |> List.map Attribute.toString |> String.concat ""
        happensEvenly '"' attr

    // Wiki: ViewBox page — SVG with size and viewBox, coordinate mapping example
    [<Fact>]
    let ``ViewBox wiki - svg with 400x400 viewport and 100x100 viewBox`` () =
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
        Assert.Contains("<circle", result)
        Assert.Contains("r=\"40\"", result)
