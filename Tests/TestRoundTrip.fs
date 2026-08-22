namespace SharpVG.Tests

open SharpVG
open System
open Xunit

module TestRoundTrip =

    // Render an Svg, parse the output, render again, assert both renders are equal.
    let private roundTrip (svg: Svg) =
        let original = svg |> Svg.toString
        match SvgParser.ofString original with
        | Error e -> failwithf "Parse failed: %s (input: %s)" e.Message original
        | Ok result ->
            let roundTripped = result.Value |> Svg.toString
            Assert.Equal(original, roundTripped)

    // --- Basic shapes ---

    [<Fact>]
    let ``round-trip circle`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip ellipse`` () =
        let center = Point.ofInts (60, 40)
        let radius = Point.ofInts (40, 20)
        Ellipse.create center radius |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip rect`` () =
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (80, 60)
        Rect.create position area |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip line`` () =
        let startPoint = Point.ofInts (0, 0)
        let endPoint = Point.ofInts (100, 100)
        Line.create startPoint endPoint |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip polyline`` () =
        let points = [ Point.ofInts (0, 0); Point.ofInts (50, 50); Point.ofInts (100, 0) ]
        Polyline.create points |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip polygon`` () =
        let points = [ Point.ofInts (50, 0); Point.ofInts (100, 100); Point.ofInts (0, 100) ]
        Polygon.create points |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip path`` () =
        let path =
            Path.empty
            |> Path.addMoveTo Absolute (Point.ofInts (10, 10))
            |> Path.addLineTo Absolute (Point.ofInts (90, 10))
            |> Path.addLineTo Absolute (Point.ofInts (90, 90))
            |> Path.addClosePath
        path |> Element.create |> Svg.ofElement |> roundTrip

    // --- Text ---

    [<Fact>]
    let ``round-trip text`` () =
        let position = Point.ofInts (20, 40)
        Text.create position "Hello SVG" |> Element.create
        |> Svg.ofElement |> roundTrip

    // --- Style ---

    [<Fact>]
    let ``round-trip element with fill and stroke`` () =
        let strokeColor = Color.ofName Colors.Black
        let fillColor = Color.ofName Colors.Red
        let strokePen = Pen.createWithWidth strokeColor (Length.ofInt 2)
        let style = Style.createWithPen strokePen |> Style.withFill fillColor
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.createWithStyle style
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip element with id`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        Circle.create center radius
        |> Element.createWithName "myCircle"
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip element with class`` () =
        let position = Point.ofInts (0, 0)
        let area = Area.ofInts (100, 50)
        Rect.create position area
        |> Element.create
        |> Element.withClass "highlight"
        |> Svg.ofElement |> roundTrip

    // --- Transform ---

    [<Fact>]
    let ``round-trip element with translate transform`` () =
        let center = Point.ofInts (0, 0)
        let radius = Length.ofInt 20
        let translateX = Length.ofInt 50
        let translateY = Length.ofInt 30
        let transform =
            Transform.createTranslate translateX
            |> Transform.withY translateY
        Circle.create center radius
        |> Element.create
        |> Element.withTransform transform
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip element with rotate transform`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let pivot = Length.ofInt 0
        let transform = Transform.createRotate 45.0 pivot pivot
        Circle.create center radius
        |> Element.create
        |> Element.withTransform transform
        |> Svg.ofElement |> roundTrip

    // --- Group ---

    [<Fact>]
    let ``round-trip group`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (40, 30)
        let circle = Circle.create center radius |> Element.create
        let rect = Rect.create position area |> Element.create
        Group.ofList [ circle; rect ]
        |> Element.create
        |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip nested groups`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 10
        let inner = Circle.create center radius |> Element.create
        let innerGroup = Group.ofList [ inner ] |> Element.create
        let outer = Group.ofList [ innerGroup ] |> Element.create
        outer |> Svg.ofElement |> roundTrip

    [<Fact>]
    let ``round-trip named group`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let circle = Circle.create center radius |> Element.create
        Group.ofList [ circle ]
        |> Group.withName "myGroup"
        |> Element.create
        |> Svg.ofElement |> roundTrip

    // --- ViewBox and size ---

    [<Fact>]
    let ``round-trip SVG with size and viewBox`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let size = Area.ofInts (200, 150)
        let viewBox = ViewBox.create Point.origin size
        Circle.create center radius
        |> Element.create
        |> Svg.ofElement
        |> Svg.withSize size
        |> Svg.withViewBox viewBox
        |> roundTrip

    // --- Defs: linear gradient ---

    [<Fact>]
    let ``round-trip linear gradient`` () =
        let stops =
            [ GradientStop.create 0.0 (Color.ofName Colors.Blue)
              GradientStop.create 1.0 (Color.ofName Colors.White) ]
        let gradient =
            LinearGradient.create "grad1" Point.origin (Point.ofFloats (1.0, 0.0)) stops
            |> Gradient.ofLinear
        let definitions = SvgDefinitions.create |> SvgDefinitions.addGradient gradient
        let style = Style.createWithFill (Color.ofUrl "grad1")
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (180, 80)
        Rect.create position area
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: radial gradient ---

    [<Fact>]
    let ``round-trip radial gradient`` () =
        let stops =
            [ GradientStop.create 0.0 (Color.ofName Colors.Yellow)
              GradientStop.create 1.0 (Color.ofName Colors.Orange) ]
        let gradient =
            RadialGradient.create "radGrad" (Point.ofInts (50, 50)) (Length.ofInt 50) stops
            |> Gradient.ofRadial
        let definitions = SvgDefinitions.create |> SvgDefinitions.addGradient gradient
        let style = Style.createWithFill (Color.ofUrl "radGrad")
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        Circle.create center radius
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: clipPath ---

    [<Fact>]
    let ``round-trip clipPath`` () =
        let clipCircle =
            Circle.create (Point.ofInts (50, 50)) (Length.ofInt 40)
            |> Element.create
        let clipPath = ClipPath.ofElement "clip1" clipCircle
        let definitions = SvgDefinitions.create |> SvgDefinitions.addClipPath clipPath
        let style = Style.empty |> Style.withClipPath "clip1"
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (80, 80)
        Rect.create position area
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: filter ---

    [<Fact>]
    let ``round-trip gaussian blur filter`` () =
        let blur = FilterEffect.createGaussianBlur 5.0
        let filter = Filter.create blur |> Filter.withId "blurFilter"
        let definitions = SvgDefinitions.create |> SvgDefinitions.addFilter filter
        let style = Style.empty |> Style.withFilter "blurFilter"
        let position = Point.ofInts (20, 20)
        let area = Area.ofInts (160, 80)
        Rect.create position area
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    [<Fact>]
    let ``round-trip filter chain`` () =
        let blur = FilterEffect.createGaussianBlur 4.0
        let offset = FilterEffect.createOffset (Point.ofInts (3, 3))
        let filter = Filter.ofChain [ blur; offset ] |> Filter.withId "chainFilter"
        let definitions = SvgDefinitions.create |> SvgDefinitions.addFilter filter
        let style = Style.empty |> Style.withFilter "chainFilter"
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: marker ---

    [<Fact>]
    let ``round-trip marker`` () =
        let markerArea = Area.ofInts (10, 10)
        let markerPath =
            Path.empty
            |> Path.addMoveTo Absolute Point.origin
            |> Path.addLineTo Absolute (Point.ofInts (10, 5))
            |> Path.addLineTo Absolute (Point.ofInts (0, 10))
            |> Path.addClosePath
        let marker =
            Marker.create "arrow"
            |> Marker.withSize markerArea
            |> Marker.addElement (markerPath |> Element.create)
        let definitions = SvgDefinitions.create |> SvgDefinitions.addMarker marker
        let style = Style.empty |> Style.withMarkerEnd "arrow"
        let startPoint = Point.ofInts (10, 50)
        let endPoint = Point.ofInts (190, 50)
        Line.create startPoint endPoint
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: pattern ---

    [<Fact>]
    let ``round-trip pattern`` () =
        let tileArea = Area.ofInts (10, 10)
        let dot = Circle.create (Point.ofInts (5, 5)) (Length.ofInt 3) |> Element.create
        let pattern = Pattern.ofElement "dots" tileArea dot
        let definitions = SvgDefinitions.create |> SvgDefinitions.addPattern pattern
        let style = Style.createWithFill (Color.ofUrl "dots")
        let position = Point.ofInts (0, 0)
        let area = Area.ofInts (200, 100)
        Rect.create position area
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: mask ---

    [<Fact>]
    let ``round-trip mask`` () =
        let maskRect =
            Rect.create (Point.ofInts (10, 10)) (Area.ofInts (80, 80))
            |> Element.createWithStyle (Style.createWithFill (Color.ofName Colors.White))
        let mask = Mask.ofElement "myMask" maskRect
        let definitions = SvgDefinitions.create |> SvgDefinitions.addMask mask
        let style = Style.empty |> Style.withMask "myMask"
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        Circle.create center radius
        |> Element.createWithStyle style
        |> List.singleton
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Defs: symbol + use ---

    [<Fact>]
    let ``round-trip symbol and use`` () =
        let viewBox = ViewBox.create Point.origin (Area.ofInts (100, 100))
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        let symbol =
            Symbol.create viewBox
            |> Symbol.withBody [ Circle.create center radius |> Element.create ]
            |> Element.createWithName "icon"
        let definitions = SvgDefinitions.create |> SvgDefinitions.addElement symbol
        let position = Point.ofInts (10, 10)
        let use1 = Use.create symbol position |> Element.create
        [ use1 ]
        |> Svg.ofElementsWithDefinitions definitions
        |> roundTrip

    // --- Anchor ---

    [<Fact>]
    let ``round-trip anchor`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let circle = Circle.create center radius |> Element.create
        Anchor.create "https://example.com"
        |> Anchor.addElement circle
        |> Element.create
        |> Svg.ofElement |> roundTrip

    // --- Multiple shapes with styles ---

    [<Fact>]
    let ``round-trip multiple styled shapes`` () =
        let strokeColor = Color.ofName Colors.Black
        let redStyle =
            Pen.createWithWidth strokeColor (Length.ofInt 1)
            |> Style.createWithPen
            |> Style.withFill (Color.ofName Colors.Red)
        let blueStyle =
            Pen.createWithWidth strokeColor (Length.ofInt 1)
            |> Style.createWithPen
            |> Style.withFill (Color.ofName Colors.Blue)
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (40, 20)
        let circle = Circle.create center radius |> Element.createWithStyle redStyle
        let rect = Rect.create position area |> Element.createWithStyle blueStyle
        let startPoint = Point.ofInts (0, 0)
        let endPoint = Point.ofInts (100, 100)
        let line = Line.create startPoint endPoint |> Element.create
        let group = Group.ofList [ circle; rect ] |> Element.create
        [ group; line ]
        |> Svg.ofList
        |> Svg.withSize (Area.ofInts (200, 200))
        |> roundTrip

    // --- Raw element passthrough ---

    [<Fact>]
    let ``round-trip unknown element preserved as raw`` () =
        let svgStr = """<svg><foreignObject width="100" height="50"><p>Hello</p></foreignObject></svg>"""
        match SvgParser.ofString svgStr with
        | Error e -> failwithf "Parse failed: %s" e.Message
        | Ok r1 ->
            let rendered1 = r1.Value |> Svg.toString
            match SvgParser.ofString rendered1 with
            | Error e -> failwithf "Second parse failed: %s" e.Message
            | Ok r2 ->
                Assert.Equal(rendered1, r2.Value |> Svg.toString)
