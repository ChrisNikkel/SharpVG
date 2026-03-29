namespace SharpVG.Tests

open System
open SharpVG
open Xunit

module TestSvgCheck =

    let private mkSvg elements =
        let size = Area.ofInts (200, 200)
        elements
        |> Svg.ofList
        |> Svg.withSize size
        |> Svg.withViewBox (ViewBox.create Point.origin size)
        |> Svg.toString

    [<Fact>]
    let ``circle is valid SVG`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 40
        Circle.create center radius
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``rect is valid SVG`` () =
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (80, 60)
        Rect.create position area
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``ellipse is valid SVG`` () =
        let center = Point.ofInts (100, 100)
        let radii = Point.ofInts (60, 40)
        Ellipse.create center radii
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``line is valid SVG`` () =
        let endPoint = Point.ofInts (100, 100)
        Line.create Point.origin endPoint
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with id is valid SVG`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.createWithName "myCircle"
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with class is valid SVG`` () =
        let area = Area.ofInts (100, 100)
        Rect.create Point.origin area
        |> Element.createWithClass "highlight"
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with multiple classes is valid SVG`` () =
        let area = Area.ofInts (100, 100)
        Rect.create Point.origin area
        |> Element.create
        |> Element.withClasses ["foo"; "bar"; "baz"]
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with style is valid SVG`` () =
        let strokeColor = Color.ofName Colors.Black
        let fillColor = Color.ofName Colors.Blue
        let penWidth = Length.ofInt 2
        let style = Style.empty |> Style.withFill fillColor |> Style.withStroke strokeColor |> Style.withStrokeWidth penWidth
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.createWithStyle style
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``group is valid SVG`` () =
        let center = Point.ofInts (50, 50)
        let circleRadius = Length.ofInt 20
        let rectPosition = Point.ofInts (10, 10)
        let rectArea = Area.ofInts (30, 30)
        let circle = Circle.create center circleRadius |> Element.create
        let rect = Rect.create rectPosition rectArea |> Element.create
        let group = Group.ofList [circle; rect] |> Element.create
        [group]
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``group with id is valid SVG`` () =
        let radius = Length.ofInt 10
        let circle = Circle.create Point.origin radius |> Element.create
        let group = Group.ofList [circle] |> Group.withName "myGroup" |> Element.create
        [group]
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``chartreuse color is valid SVG`` () =
        let fillColor = Color.ofName Colors.Chartreuse
        let style = Style.empty |> Style.withFill fillColor
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.createWithStyle style
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``transform is valid SVG`` () =
        let translateX = Length.ofInt 10
        let translateY = Length.ofInt 20
        let transform = Transform.createTranslate translateX |> Transform.withY translateY
        let area = Area.ofInts (50, 50)
        Rect.create Point.origin area
        |> Element.create
        |> Element.withTransform transform
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``animation is valid SVG`` () =
        let beginTime = TimeSpan.FromSeconds 0.0
        let duration = TimeSpan.FromSeconds 2.0
        let timing = Timing.create beginTime |> Timing.withDuration duration
        let animation = Animation.createAnimation timing AttributeType.XML "cx" "50" "150"
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.create
        |> Element.withAnimation animation
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``path is valid SVG`` () =
        let startPoint = Point.ofInts (10, 10)
        let endPoint = Point.ofInts (90, 90)
        let cornerPoint = Point.ofInts (90, 10)
        let path =
            Path.empty
            |> Path.addMoveTo Absolute startPoint
            |> Path.addLineTo Absolute endPoint
            |> Path.addLineTo Absolute cornerPoint
            |> Path.addClosePath
        path
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``restart whenNotActive is valid SVG`` () =
        let beginTime = TimeSpan.FromSeconds 0.0
        let duration = TimeSpan.FromSeconds 1.0
        let timing =
            Timing.create beginTime
            |> Timing.withDuration duration
            |> Timing.withResart WhenNotActive
        let animation = Animation.createAnimation timing AttributeType.XML "opacity" "1" "0"
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 30
        Circle.create center radius
        |> Element.create
        |> Element.withAnimation animation
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid
