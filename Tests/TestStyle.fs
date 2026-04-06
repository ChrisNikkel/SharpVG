namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

module TestStyle =

    [<Fact>]
    let ``create style with fill`` () =
        Assert.Equal("fill:white", Style.empty |> Style.withFill (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke`` () =
        Assert.Equal("stroke:white", Style.empty |> Style.withStroke (Name Colors.White) |> Style.toString)

    [<Fact>]
    let ``create style with stroke width`` () =
        Assert.Equal("stroke:white;stroke-width:0", Style.empty |> Style.withStroke (Name Colors.White) |> Style.withStrokeWidth Length.empty |> Style.toString)

    [<Fact>]
    let ``create style with opacity`` () =
        Assert.Equal("opacity:0.5", Style.empty |> Style.withOpacity 0.5 |> Style.toString)

    [<Fact>]
    let ``create style with fill opacity`` () =
        Assert.Equal("fill-opacity:0.8", Style.empty |> Style.withFillOpacity 0.8 |> Style.toString)

    [<Fact>]
    let ``create style with pen and fill pen`` () =
        let strokeColor = Color.ofName Colors.Blue
        let fillColor = Color.ofName Colors.Cyan
        let penWidth = Length.ofInt 3
        let strokePen = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
        let fillPen = Pen.create fillColor
        let style = Style.createWithPen strokePen |> Style.withFillPen fillPen
        let result = style |> Style.toString
        Assert.Contains("stroke:blue", result)
        Assert.Contains("fill:cyan", result)
        Assert.Contains("stroke-width:3", result)

    [<Fact>]
    let ``named style isNamed returns true`` () =
        let style = Style.createWithName "myStyle"
        Assert.True(Style.isNamed style)

    [<Fact>]
    let ``unnamed style isNamed returns false`` () =
        Assert.False(Style.isNamed Style.empty)

    [<Fact>]
    let ``named style toCssString`` () =
        let fillColor = Color.ofName Colors.Red
        let style = Style.createWithFill fillColor |> Style.withName "highlight"
        let result = style |> Style.toCssString
        Assert.Contains(".highlight", result)
        Assert.Contains("fill:red", result)

    [<Fact>]
    let ``style toAttribute produces inline style`` () =
        let fillColor = Color.ofName Colors.Green
        let result = Style.createWithFill fillColor |> Style.toAttribute |> Attribute.toString
        Assert.Equal("style=\"fill:green\"", result)

    [<Fact>]
    let ``named style applied to element uses class not inline attributes`` () =
        let strokeColor = Color.ofName Colors.Black
        let style = Style.createWithStroke strokeColor |> Style.withName "outlined"
        let result = Rect.create Point.origin Area.full |> Element.createWithStyle style |> Element.toString
        Assert.Contains("class=\"outlined\"", result)
        Assert.DoesNotContain("stroke=", result)

    [<Fact>]
    let ``style withStrokeLinecap butt`` () =
        let result = Style.empty |> Style.withStrokeLinecap ButtLinecap |> Style.toString
        Assert.Contains("stroke-linecap:butt", result)

    [<Fact>]
    let ``style withStrokeLinecap round`` () =
        let result = Style.empty |> Style.withStrokeLinecap RoundLinecap |> Style.toString
        Assert.Contains("stroke-linecap:round", result)

    [<Fact>]
    let ``style withStrokeLinecap square`` () =
        let result = Style.empty |> Style.withStrokeLinecap SquareLinecap |> Style.toString
        Assert.Contains("stroke-linecap:square", result)

    [<Fact>]
    let ``style withStrokeLinejoin miter`` () =
        let result = Style.empty |> Style.withStrokeLinejoin MiterLinejoin |> Style.toString
        Assert.Contains("stroke-linejoin:miter", result)

    [<Fact>]
    let ``style withStrokeLinejoin round`` () =
        let result = Style.empty |> Style.withStrokeLinejoin RoundLinejoin |> Style.toString
        Assert.Contains("stroke-linejoin:round", result)

    [<Fact>]
    let ``style withStrokeLinejoin bevel`` () =
        let result = Style.empty |> Style.withStrokeLinejoin BevelLinejoin |> Style.toString
        Assert.Contains("stroke-linejoin:bevel", result)

    [<Fact>]
    let ``style withStrokeDashArray`` () =
        let result = Style.empty |> Style.withStrokeDashArray [5.0; 3.0] |> Style.toString
        Assert.Contains("stroke-dasharray:5,3", result)

    [<Fact>]
    let ``style withStrokeDashOffset`` () =
        let result = Style.empty |> Style.withStrokeDashOffset 2.0 |> Style.toString
        Assert.Contains("stroke-dashoffset:2", result)

    [<Fact>]
    let ``style withFillRule nonzero`` () =
        let result = Style.empty |> Style.withFillRule NonZero |> Style.toString
        Assert.Contains("fill-rule:nonzero", result)

    [<Fact>]
    let ``style withFillRule evenodd`` () =
        let result = Style.empty |> Style.withFillRule EvenOdd |> Style.toString
        Assert.Contains("fill-rule:evenodd", result)

    [<Fact>]
    let ``style withClipPath`` () =
        let result = Style.empty |> Style.withClipPath "myClip" |> Style.toString
        Assert.Contains("clip-path:url(#myClip)", result)

    [<Fact>]
    let ``style withFilter`` () =
        let result = Style.empty |> Style.withFilter "myFilter" |> Style.toString
        Assert.Contains("filter:url(#myFilter)", result)

    [<Fact>]
    let ``style withMarkerStart`` () =
        let result = Style.empty |> Style.withMarkerStart "arrow" |> Style.toString
        Assert.Contains("marker-start:url(#arrow)", result)

    [<Fact>]
    let ``style withMarkerMid`` () =
        let result = Style.empty |> Style.withMarkerMid "dot" |> Style.toString
        Assert.Contains("marker-mid:url(#dot)", result)

    [<Fact>]
    let ``style withMarkerEnd`` () =
        let result = Style.empty |> Style.withMarkerEnd "tip" |> Style.toString
        Assert.Contains("marker-end:url(#tip)", result)

    [<Fact>]
    let ``style withStrokeMiterLimit`` () =
        let result = Style.empty |> Style.withStrokeMiterLimit 4.0 |> Style.toString
        Assert.Contains("stroke-miterlimit:4", result)

    [<Fact>]
    let ``style withMask`` () =
        let result = Style.empty |> Style.withMask "myMask" |> Style.toString
        Assert.Contains("mask:url(#myMask)", result)

    [<Fact>]
    let ``style withVisibility visible`` () =
        let result = Style.empty |> Style.withVisibility Visible |> Style.toString
        Assert.Contains("visibility:visible", result)

    [<Fact>]
    let ``style withVisibility hidden`` () =
        let result = Style.empty |> Style.withVisibility Hidden |> Style.toString
        Assert.Contains("visibility:hidden", result)

    [<Fact>]
    let ``style withVisibility collapse`` () =
        let result = Style.empty |> Style.withVisibility Collapse |> Style.toString
        Assert.Contains("visibility:collapse", result)

    [<Fact>]
    let ``style withDisplay inline`` () =
        let result = Style.empty |> Style.withDisplay Inline |> Style.toString
        Assert.Contains("display:inline", result)

    [<Fact>]
    let ``style withDisplay none`` () =
        let result = Style.empty |> Style.withDisplay DisplayNone |> Style.toString
        Assert.Contains("display:none", result)

    [<Fact>]
    let ``style withVectorEffect none`` () =
        let result = Style.empty |> Style.withVectorEffect VectorEffectNone |> Style.toString
        Assert.Contains("vector-effect:none", result)

    [<Fact>]
    let ``style withVectorEffect non-scaling-stroke`` () =
        let result = Style.empty |> Style.withVectorEffect NonScalingStroke |> Style.toString
        Assert.Contains("vector-effect:non-scaling-stroke", result)

    [<Fact>]
    let ``style withShapeRendering auto`` () =
        let result = Style.empty |> Style.withShapeRendering ShapeRenderingAuto |> Style.toString
        Assert.Contains("shape-rendering:auto", result)

    [<Fact>]
    let ``style withShapeRendering optimizeSpeed`` () =
        let result = Style.empty |> Style.withShapeRendering OptimizeSpeed |> Style.toString
        Assert.Contains("shape-rendering:optimizeSpeed", result)

    [<Fact>]
    let ``style withShapeRendering crispEdges`` () =
        let result = Style.empty |> Style.withShapeRendering CrispEdges |> Style.toString
        Assert.Contains("shape-rendering:crispEdges", result)

    [<Fact>]
    let ``style withShapeRendering geometricPrecision`` () =
        let result = Style.empty |> Style.withShapeRendering GeometricPrecision |> Style.toString
        Assert.Contains("shape-rendering:geometricPrecision", result)

    [<Fact>]
    let ``style withPaintOrder fill`` () =
        let result = Style.empty |> Style.withPaintOrder [FillLayer] |> Style.toString
        Assert.Contains("paint-order:fill", result)

    [<Fact>]
    let ``style withPaintOrder stroke markers`` () =
        let result = Style.empty |> Style.withPaintOrder [StrokeLayer; MarkersLayer] |> Style.toString
        Assert.Contains("paint-order:stroke markers", result)

    [<Property>]
    let ``withVectorEffect always adds vector-effect property to style`` (effect: VectorEffect) =
        let result = Style.empty |> Style.withVectorEffect effect |> Style.toString
        result.Contains("vector-effect:")

    [<Property>]
    let ``withShapeRendering always adds shape-rendering property to style`` (rendering: ShapeRendering) =
        let result = Style.empty |> Style.withShapeRendering rendering |> Style.toString
        result.Contains("shape-rendering:")

    [<Property>]
    let ``withPaintOrder with any single layer always adds paint-order property`` (layer: PaintLayer) =
        let result = Style.empty |> Style.withPaintOrder [layer] |> Style.toString
        result.Contains("paint-order:")

    [<SvgProperty>]
    let ``style with fill and stroke always has balanced quotes`` (r, g, b, p) =
        let strokeColor = Color.ofValues (r, g, b)
        let style = Style.createWithStroke strokeColor |> Style.withStrokeWidth (Length.ofFloat p)
        let result = style |> Style.toString
        happensEvenly '"' result

    [<SvgProperty>]
    let ``element with any fill color always produces bodyless tag`` (r, g, b) =
        let style = Style.createWithFill (Color.ofValues (r, g, b))
        let result = Rect.create Point.origin Area.full |> Element.createWithStyle style |> Element.toString
        checkBodylessTag "rect" result

    // Wiki: Style page — pen-based style example
    [<Fact>]
    let ``Style wiki - rect styled with stroke and fill pens`` () =
        let strokeColor = Color.ofName Colors.Blue
        let fillColor = Color.ofName Colors.Cyan
        let penWidth = Length.ofInt 3
        let strokePen = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
        let fillPen = Pen.create fillColor
        let style = Style.createWithPen strokePen |> Style.withFillPen fillPen
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (50, 50)
        let result = Rect.create position area |> Element.createWithStyle style |> Element.toString
        Assert.Contains("<rect", result)
        Assert.Contains("stroke=\"blue\"", result)
        Assert.Contains("fill=\"cyan\"", result)
        Assert.Contains("stroke-width=\"3\"", result)

    // Wiki: Style page — named styles and CSS string generation
    [<Fact>]
    let ``Style wiki - named style toString and toCssString`` () =
        let style1 =
            Style.createWithName "Default"
            |> Style.withFill (Color.ofName Colors.Blue)
            |> Style.withStroke (Color.ofName Colors.Aqua)
            |> Style.withOpacity 0.5
        let color = Color.ofPercents (0.5, 0.5, 0.5)
        let style2 = Style.createNamed color color Length.one 1.0 1.0 "Boring"
        Assert.Contains("stroke:aqua", Style.toString style1)
        Assert.Contains("fill:blue", Style.toString style1)
        Assert.Contains("opacity:0.5", Style.toString style1)
        Assert.Contains(".Boring", Style.toCssString style2)
        Assert.Contains("stroke:", Style.toCssString style2)

    // Wiki: Style page — Styles.toString wraps named styles in CDATA block
    [<Fact>]
    let ``Style wiki - Styles.toString produces style tag with CDATA`` () =
        let style1 =
            Style.createWithName "Default"
            |> Style.withFill (Color.ofName Colors.Blue)
            |> Style.withStroke (Color.ofName Colors.Aqua)
            |> Style.withOpacity 0.5
        let color = Color.ofPercents (0.5, 0.5, 0.5)
        let style2 = Style.createNamed color color Length.one 1.0 1.0 "Boring"
        let styles = seq { yield style1; yield style2 }
        let result = Styles.toString styles
        Assert.Contains("<style", result)
        Assert.Contains("<![CDATA[", result)
        Assert.Contains(".Default", result)
        Assert.Contains(".Boring", result)

