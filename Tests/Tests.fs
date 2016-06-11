module SharpVGTests
open LogHelpers
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

type Positive =
    static member Int() =
        Arb.Default.Int32()
        |> Arb.mapFilter abs (fun t -> t > 0)

[<Property>]
let ``draw lines`` (x1 : NormalFloat, y1 : NormalFloat, x2 : NormalFloat, y2 : NormalFloat, c, r, g, b, p, o) =
    configureLogs
    let point1 = { X = Pixels (x1 |> double); Y = Pixels (y1 |> double) }
    let point2 = { X = Pixels (x2 |> double); Y = Pixels (y2 |> double) }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let line = Line.create point1 point2
    let tagString = line |> Element.ofLine |> Element.withStyle style |> Element.toString

    basicChecks "line" tagString

[<Property>]
let ``draw rectangles`` (x : NormalFloat, y : NormalFloat, h : NormalFloat, w : NormalFloat, c, r, g, b, p, o) =
    configureLogs
    let point = { X = Pixels (x |> double); Y = Pixels (y |> double)}
    let area = { Height = Pixels (h |> double); Width = Pixels (w |> double) }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let rect = Rect.create point area
    let tagString = rect |> Element.ofRect |> Element.withStyle style |> Element.toString

    basicChecks "rect" tagString

[<Property>]
let ``draw circles`` (x : NormalFloat, y : NormalFloat, radius : NormalFloat, c, r, g, b, p, o) =
    configureLogs
    let point = { X = Pixels (x |> double); Y = Pixels (y |> double) }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = { Center = point; Radius = Pixels(radius |> double) }
    let tagString = circle |> Element.ofCircle |> Element.withStyle style |> Element.toString

    basicChecks "circle" tagString

[<Fact>]
let ``do lots and don't fail`` () =
    configureLogs

    let points = seq {
        yield Point.create (Pixels 1.0) (Pixels 1.0)
        yield Point.create (Pixels 4.0) (Pixels 4.0)
        yield Point.create (Pixels 8.0) (Pixels 8.0)
    }

    let point = Point.create (Pixels 24.0) (Pixels 15.0)
    let style1 = Style.create (Name colors.Red) (Hex 0xff0000) (Pixels 3.0) 1.0
    let style2 = Style.create (Name colors.Blue) (SmallHex 0xf00s) (Pixels 6.0) 1.0
    let length = Length.createWithPixels 1.0
    let area = Area.create length length

    // TODO: Add transform, circle, ellipse, polygon, polyline, path, script
    let graphics = seq {
        yield Image.create point area "myimage1.jpg" |> Element.ofImage
        yield Image.create point area "myimage2.jpg" |> Element.ofImage |> Element.withStyle style1
        yield Text.create point "Hello World!" |> Element.ofText |> Element.withStyle style2
        yield Line.create point point |> Element.ofLine |> Element.withStyle style1
        yield Rect.create point area |> Element.ofRect |> Element.withStyle style2
    }

    let svgBody = graphics |> Svg.ofSeq

    svgBody |> Svg.toHtml "SVG Demo" |> isMatched '<' '>'
