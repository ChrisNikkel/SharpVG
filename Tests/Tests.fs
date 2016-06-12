module SharpVGTests
open LogHelpers
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

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
    let point1 = { X = Pixels x1; Y = Pixels y1 }
    let point2 = { X = Pixels x2; Y = Pixels y2 }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let line = Line.create point1 point2
    let tag = line |> Element.ofLine |> Element.withStyle style |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "line")
    && (isTagEnclosed tag) @>

[<SvgProperty>]
let ``draw rectangles`` (x, y, h, w, c, r, g, b, p, o) =
    configureLogs
    let point = { X = Pixels x; Y = Pixels y}
    let area = { Height = Pixels h; Width = Pixels w }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let rect = Rect.create point area
    let tag = rect |> Element.ofRect |> Element.withStyle style |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "rect")
    && (isTagEnclosed tag) @>

[<SvgProperty>]
let ``draw circles`` (x, y, radius, c, r, g, b, p, o) =
    configureLogs
    let point = { X = Pixels x; Y = Pixels y }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let circle = Circle.create point (Pixels radius)
    let tag = circle |> Element.ofCircle |> Element.withStyle style |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "circle")
    && (isTagEnclosed tag) @>


[<SvgProperty>]
let ``draw ellipses`` (x1, y1, x2, y2, c, r, g, b, p, o) =
    configureLogs
    let point1 = { X = Pixels x1; Y = Pixels y1 }
    let point2 = { X = Pixels x2; Y = Pixels y2 }
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let ellipse = Ellipse.create point1 point2
    let tag = ellipse |> Element.ofEllipse |> Element.withStyle style |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "ellipse")
    && (isTagEnclosed tag) @>

[<Property>]
let ``draw images`` (x, y, h, w, i) =
    configureLogs
    let point = { X = Pixels x; Y = Pixels y }
    let area = { Height = Pixels h; Width = Pixels w }
    let image = Image.create point area i
    let tag = image |> Element.ofImage |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "image")
    && (isTagEnclosed tag) @>

[<SvgProperty>]
let ``draw texts`` (x, y, c, r, g, b, p, o) =
    configureLogs
    let point = { X = Pixels (x |> float); Y = Pixels (y |> float)}
    let fill, stroke, strokeWidth, opacity = Hex c, Values(r, g, b), Pixels p, o
    let style = Style.create fill stroke strokeWidth opacity
    let text = Text.create point "test"
    let tag = text |> Element.ofText |> Element.withStyle style |> Element.toString

    test <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains "text")
    && (tag.Contains "test") @>

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

    graphics |> Svg.ofSeq |> Svg.toHtml "SVG Demo" |> isMatched '<' '>'
