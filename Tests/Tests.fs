module SharpVGTests
open LogHelpers
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote
open System

type Positive =
    static member Int() =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun t -> t > 0)

type PositiveFloat =
    static member Float() =
        Arb.Default.Float()
        |> Arb.mapFilter abs (fun t -> t > 0.0)

type SvgProperty() =
    inherit PropertyAttribute(Arbitrary = [| typeof<PositiveFloat> |])


[<SvgProperty>]
let ``draw lines`` (x1, y1, x2, y2, c, r, g, b, p, o) =
    configureLogs
    let point1, point2 = (Point.create (Pixels x1) (Pixels y1)), (Point.create (Pixels x2) (Pixels y2))
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let line = Line.create point1 point2
    let tag = line |> Element.ofLine |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "line" tag

[<SvgProperty>]
let ``draw rectangles`` (x, y, h, w, c, r, g, b, p, o) =
    configureLogs
    let point = Point.create (Pixels x) (Pixels y)
    let area = Area.create (Pixels h) (Pixels w)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let rect = Rect.create point area
    let tag = rect |> Element.ofRect |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "rect" tag

[<SvgProperty>]
let ``draw circles`` (x, y, radius, c, r, g, b, p, o) =
    configureLogs
    let point = Point.create (Pixels x) (Pixels y)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = Circle.create point (Pixels radius)
    let tag = circle |> Element.ofCircle |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "circle" tag


[<SvgProperty>]
let ``draw ellipses`` (x1, y1, x2, y2, c, r, g, b, p, o) =
    configureLogs
    let point1, point2 = (Point.create (Pixels x1) (Pixels y1)), (Point.create (Pixels x2) (Pixels y2))
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let ellipse = Ellipse.create point1 point2
    let tag = ellipse |> Element.ofEllipse |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "ellipse" tag

[<Property>]
let ``draw images`` (x, y, h, w, i) =
    configureLogs
    let point = Point.create (Pixels x) (Pixels y)
    let area = { Height = Pixels h; Width = Pixels w }
    let image = Image.create point area i
    let tag = image |> Element.ofImage |> Element.toString

    test <| checkBodylessTag "image" tag

[<SvgProperty>]
let ``draw texts`` (x, y, c, r, g, b, p, o) =
    configureLogs
    let point = Point.create (Pixels x) (Pixels y)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let text = Text.create point "test"
    let tag = text |> Element.ofText |> Element.withStyle style |> Element.toString

    test <| checkTag "text" tag

[<SvgProperty>]
let ``animate circles`` (x, y, radius, c, r, g, b, p, o) =
    configureLogs
    let p1 = Point.create (Pixels 100.0) (Pixels 100.0)
    let p2 = Point.create (Pixels 500.0) (Pixels 500.0)
    let p3 = Point.create (Pixels 200.0) (Pixels 400.0)
    let point = Point.create (Pixels x) (Pixels y)
    let path = Path.empty |> (Path.addAbsolute CurveTo p1) |> (Path.addAbsolute LineTo p2) |> (Path.addAbsolute CurveTo p3)
    let timing = Timing.create <| TimeSpan.FromSeconds(0.0)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = Circle.create point (Pixels radius)
    let animation = Animation.createMotion timing path None
    let tag = circle |> Element.ofCircle |> Element.withStyle style |> Element.withAnimation animation |> Element.toString

    test <| checkTag "circle" tag

[<Fact>]
let ``do lots and don't fail`` () =
    configureLogs

    let points = seq {
        yield Point.create (Pixels 1.0) (Pixels 1.0)
        yield Point.create (Pixels 4.0) (Pixels 4.0)
        yield Point.create (Pixels 8.0) (Pixels 8.0)
    }

    let point = Point.create (Pixels 24.0) (Pixels 15.0)
    let style1 = Style.create (Name Colors.Red) (Hex 0xff0000) (Pixels 3.0) 1.0
    let style2 = Style.create (Name Colors.Blue) (SmallHex 0xf00s) (Pixels 6.0) 1.0
    let length = Length.createWithPixels 1.0
    let area = Area.create length length

    // TODO: Add transform, polygon, polyline, path, script
    let graphics = seq {
        yield Image.create point area "myimage1.jpg" |> Element.ofImage
        yield Image.create point area "myimage2.jpg" |> Element.ofImage |> Element.withStyle style1
        yield Text.create point "Hello World!" |> Element.ofText |> Element.withStyle style2
        yield Line.create point point |> Element.ofLine |> Element.withStyle style1
        yield Rect.create point area |> Element.ofRect |> Element.withStyle style2
        yield Circle.create point length |> Element.ofCircle
        yield Ellipse.create point point |> Element.ofEllipse |> Element.withStyle style1
    }

    let html = graphics|> Svg.ofSeq |> Svg.toHtml "SVG Demo"
    test <| checkTag "SVG Demo" html
