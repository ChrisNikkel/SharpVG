namespace SharpVG.Tests

open SharpVG
open Xunit

module TestMask =

    [<Fact>]
    let ``Mask create empty`` () =
        let result = Mask.create "myMask" |> Mask.toString
        Assert.Equal("<mask id=\"myMask\"></mask>", result)

    [<Fact>]
    let ``Mask create has correct id`` () =
        let result = Mask.create "testMask" |> Mask.toString
        Assert.Contains("id=\"testMask\"", result)

    [<Fact>]
    let ``Mask ofElement contains element`` () =
        let rect = Rect.create Point.origin (Area.ofInts (100, 100)) |> Element.create
        let result = Mask.ofElement "myMask" rect |> Mask.toString
        Assert.Contains("id=\"myMask\"", result)
        Assert.Contains("<rect", result)

    [<Fact>]
    let ``Mask withMaskUnits userSpaceOnUse`` () =
        let result = Mask.create "myMask" |> Mask.withMaskUnits UserSpaceOnUse |> Mask.toString
        Assert.Contains("maskUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Mask withMaskUnits objectBoundingBox`` () =
        let result = Mask.create "myMask" |> Mask.withMaskUnits ObjectBoundingBox |> Mask.toString
        Assert.Contains("maskUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``Mask withMaskContentUnits objectBoundingBox`` () =
        let result = Mask.create "myMask" |> Mask.withMaskContentUnits ObjectBoundingBox |> Mask.toString
        Assert.Contains("maskContentUnits=\"objectBoundingBox\"", result)

    [<Fact>]
    let ``Mask withMaskContentUnits userSpaceOnUse`` () =
        let result = Mask.create "myMask" |> Mask.withMaskContentUnits UserSpaceOnUse |> Mask.toString
        Assert.Contains("maskContentUnits=\"userSpaceOnUse\"", result)

    [<Fact>]
    let ``Mask withLocation adds x y attributes`` () =
        let result = Mask.create "myMask" |> Mask.withLocation (Point.ofInts (10, 20)) |> Mask.toString
        Assert.Contains("x=\"10\"", result)
        Assert.Contains("y=\"20\"", result)

    [<Fact>]
    let ``Mask withSize adds width height attributes`` () =
        let result = Mask.create "myMask" |> Mask.withSize (Area.ofInts (200, 100)) |> Mask.toString
        Assert.Contains("width=\"200\"", result)
        Assert.Contains("height=\"100\"", result)

    [<Fact>]
    let ``Mask addElements adds multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30) |> Element.create
        let result = Mask.create "myMask" |> Mask.addElements [rect; circle] |> Mask.toString
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Mask ofElements creates with multiple elements`` () =
        let rect = Rect.create Point.origin Area.full |> Element.create
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30) |> Element.create
        let result = Mask.ofElements "myMask" [rect; circle] |> Mask.toString
        Assert.Contains("id=\"myMask\"", result)
        Assert.Contains("<rect", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``Mask toString produces valid mask tag`` () =
        let rect = Rect.create Point.origin (Area.ofInts (100, 100)) |> Element.create
        let result = Mask.ofElement "mask1" rect |> Mask.toString
        Assert.StartsWith("<mask", result)
        Assert.EndsWith("</mask>", result)

    // Wiki: Mask page — white circle mask applied to a red rect
    [<Fact>]
    let ``Mask wiki - white circle mask applied to rect via SvgDefinitions`` () =
        let maskCircle =
            Circle.create (Point.ofInts (100, 100)) (Length.ofInt 80)
            |> Element.createWithStyle (Style.createWithFill (Color.ofName Colors.White))
        let mask = Mask.ofElement "fade" maskCircle
        let maskedStyle =
            Style.createWithFill (Color.ofName Colors.Red)
            |> Style.withMask "fade"
        let rect =
            Rect.create Point.origin (Area.ofInts (200, 200))
            |> Element.createWithStyle maskedStyle
        let definitions = SvgDefinitions.create |> SvgDefinitions.addMask mask
        let result = [ rect ] |> Svg.ofList |> Svg.withDefinitions definitions |> Svg.toString
        Assert.Contains("<mask id=\"fade\"", result)
        Assert.Contains("<circle", result)
        Assert.Contains("mask=\"url(#fade)\"", result)
