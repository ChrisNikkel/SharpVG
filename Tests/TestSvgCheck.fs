namespace SharpVG.Tests

open System
open SharpVG
open Xunit

module TestSvgCheck =

    let private mkSvg elements =
        elements
        |> Svg.ofList
        |> Svg.withSize (Area.ofInts (200, 200))
        |> Svg.withViewBox (ViewBox.create Point.origin (Area.ofInts (200, 200)))
        |> Svg.toString

    [<Fact>]
    let ``circle is valid SVG`` () =
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 40)
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``rect is valid SVG`` () =
        Rect.create (Point.ofInts (10, 10)) (Area.ofInts (80, 60))
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``ellipse is valid SVG`` () =
        Ellipse.create (Point.ofInts (100, 100)) (Point.ofInts (60, 40))
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``line is valid SVG`` () =
        Line.create Point.origin (Point.ofInts (100, 100))
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with id is valid SVG`` () =
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        |> Element.createWithName "myCircle"
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with class is valid SVG`` () =
        Rect.create Point.origin (Area.ofInts (100, 100))
        |> Element.createWithClass "highlight"
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with multiple classes is valid SVG`` () =
        Rect.create Point.origin (Area.ofInts (100, 100))
        |> Element.create
        |> Element.withClasses ["foo"; "bar"; "baz"]
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``element with style is valid SVG`` () =
        let style = Style.empty |> Style.withFill (Color.ofName Colors.Blue) |> Style.withStroke (Color.ofName Colors.Black) |> Style.withStrokeWidth (Length.ofInt 2)
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        |> Element.createWithStyle style
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``group is valid SVG`` () =
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 20) |> Element.create
        let rect = Rect.create (Point.ofInts (10, 10)) (Area.ofInts (30, 30)) |> Element.create
        let group = Group.ofList [circle; rect] |> Element.create
        [group]
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``group with id is valid SVG`` () =
        let circle = Circle.create Point.origin (Length.ofInt 10) |> Element.create
        let group = Group.ofList [circle] |> Group.withName "myGroup" |> Element.create
        [group]
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``chartreuse color is valid SVG`` () =
        let style = Style.empty |> Style.withFill (Color.ofName Colors.Chartreuse)
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        |> Element.createWithStyle style
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``transform is valid SVG`` () =
        let transform = Transform.createTranslate (Length.ofInt 10) |> Transform.withY (Length.ofInt 20)
        Rect.create Point.origin (Area.ofInts (50, 50))
        |> Element.create
        |> Element.withTransform transform
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``animation is valid SVG`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 2.0)
        let animation = Animation.createAnimation timing AttributeType.XML "cx" "50" "150"
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        |> Element.create
        |> Element.withAnimation animation
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``path is valid SVG`` () =
        let path =
            Path.empty
            |> Path.addMoveTo Absolute (Point.ofInts (10, 10))
            |> Path.addLineTo Absolute (Point.ofInts (90, 90))
            |> Path.addLineTo Absolute (Point.ofInts (90, 10))
            |> Path.addClosePath
        path
        |> Element.create
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid

    [<Fact>]
    let ``restart whenNotActive is valid SVG`` () =
        let timing =
            Timing.create (TimeSpan.FromSeconds 0.0)
            |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
            |> Timing.withResart WhenNotActive
        let animation = Animation.createAnimation timing AttributeType.XML "opacity" "1" "0"
        Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        |> Element.create
        |> Element.withAnimation animation
        |> List.singleton
        |> mkSvg
        |> SvgCheck.assertValid
