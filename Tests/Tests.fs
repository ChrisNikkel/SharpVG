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
    let point1, point2 = Point.ofFloats (x1, y1), Point.ofFloats (x2, y2)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let line = Line.create point1 point2
    let tag = line |> Element.ofLine |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "line" tag

[<SvgProperty>]
let ``draw rectangles`` (x, y, h, w, c, r, g, b, p, o) =
    configureLogs
    let point = Point.ofFloats (x, y)
    let area = Area.ofFloats (h, w)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let rect = Rect.create point area
    let tag = rect |> Element.ofRect |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "rect" tag

[<SvgProperty>]
let ``draw circles`` (x, y, radius, c, r, g, b, p, o) =
    configureLogs
    let point = Point.ofFloats (x, y)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = Circle.create point (Length.ofFloat radius)
    let tag = circle |> Element.ofCircle |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "circle" tag


[<SvgProperty>]
let ``draw ellipses`` (x1, y1, x2, y2, c, r, g, b, p, o) =
    configureLogs
    let point1, point2 = Point.ofFloats (x1, y1), Point.ofFloats (x2, y2)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let ellipse = Ellipse.create point1 point2
    let tag = ellipse |> Element.ofEllipse |> Element.withStyle style |> Element.toString

    test <| checkBodylessTag "ellipse" tag

[<Property>]
let ``draw images`` (x, y, h, w, i) =
    configureLogs
    let point = Point.ofFloats (x, y)
    let area = Area.ofFloats (h, w)
    let image = Image.create point area i
    let tag = image |> Element.ofImage |> Element.toString

    test <| checkBodylessTag "image" tag

[<SvgProperty>]
let ``draw texts`` (x, y, c, r, g, b, p, o) =
    configureLogs
    let point = Point.ofFloats (x, y)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let text = Text.create point "test"
    let tag = text |> Element.ofText |> Element.withStyle style |> Element.toString

    test <| checkTag "text" tag

[<SvgProperty>]
let ``animate circles`` (x, y, radius, c, r, g, b, p, o) =
    configureLogs
    let p1 = Point.ofInts (100, 100)
    let p2 = Point.ofInts (500, 500)
    let p3 = Point.ofInts (200, 200)
    let point = Point.ofFloats (x, y)
    let path = Path.empty |> (Path.addAbsolute CurveTo p1) |> (Path.addAbsolute LineTo p2) |> (Path.addAbsolute CurveTo p3)
    let timing = Timing.create <| TimeSpan.FromSeconds(0.0)
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Length.ofFloat p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = Circle.create point (Length.ofFloat radius)
    let animation = Animation.createMotion timing path None
    let tag = circle |> Element.ofCircle |> Element.withStyle style |> Element.withAnimation animation |> Element.toString

    test <| checkTag "circle" tag

[<Fact>]
let ``do lots and don't fail`` () =
    configureLogs

    let points = seq {
        yield Point.ofInts (1, 1)
        yield Point.ofInts (4, 4)
        yield Point.ofInts (8, 8)
    }

    let point = Point.ofInts (24, 15)
    let style1 = Style.create (Name Colors.Red) (Hex 0xff0000) (Length.ofFloat 3.0) 1.0
    let style2 = Style.create (Name Colors.Blue) (SmallHex 0xf00s) (Length.ofFloat 6.0) 1.0
    let length = Length.ofPixels 1
    let area = Area.create length length

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
