namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open System

module Tests =

    [<SvgProperty>]
    let ``create rect with style`` (r, g, b) =
      let style = Style.createWithStroke <| Values(r, g, b)
      let element =
        Rect.create Point.origin Area.full
          |> Element.create
          |> Element.withStyle style
          |> Element.toString

      let result = sprintf "<rect stroke=\"rgb(%d,%d,%d)\" x=\"0\" y=\"0\" width=\"100%%\" height=\"100%%\"/>" r g b
      Assert.Equal(result, element);

    [<SvgProperty>]
    let ``draw lines`` (x1, y1, x2, y2, c, r, g, b, p, o, fo) =
        let point1, point2 = Point.ofFloats (x1, y1), Point.ofFloats (x2, y2)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let line = Line.create point1 point2
        let tag = line |> Element.create |> Element.withStyle style |> Element.toString

        checkBodylessTag "line" tag

    [<SvgProperty>]
    let ``draw rectangles`` (x, y, h, w, c, r, g, b, p, o, fo) =
        let point = Point.ofFloats (x, y)
        let area = Area.ofFloats (h, w)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let rect = Rect.create point area
        let tag = rect |> Element.create |> Element.withStyle style |> Element.toString

        checkBodylessTag "rect" tag

    [<SvgProperty>]
    let ``draw circles`` (x, y, radius, c, r, g, b, p, o, fo) =
        let point = Point.ofFloats (x, y)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let circle = Circle.create point (Length.ofFloat radius)
        let tag = circle |> Element.create |> Element.withStyle style |> Element.toString

        checkBodylessTag "circle" tag


    [<SvgProperty>]
    let ``draw ellipses`` (x1, y1, x2, y2, c, r, g, b, p, o, fo) =
        let point1, point2 = Point.ofFloats (x1, y1), Point.ofFloats (x2, y2)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let ellipse = Ellipse.create point1 point2
        let tag = ellipse |> Element.create |> Element.withStyle style |> Element.toString

        checkBodylessTag "ellipse" tag

    [<Property>]
    let ``draw images`` (x, y, h, w) =
        let point = Point.ofFloats (x, y)
        let area = Area.ofFloats (h, w)
        let image = Image.create point area "test.jpg"
        let tag = image |> Element.create |> Element.toString

        checkBodylessTag "image" tag

    [<SvgProperty>]
    let ``draw texts`` (x, y, c, r, g, b, p, o, fo) =
        let point = Point.ofFloats (x, y)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let text = Text.create point "test"
        let tag = text |> Element.create |> Element.withStyle style |> Element.toString

        checkTag "text" tag

    [<SvgProperty>]
    let ``animate circles`` (x, y, radius, c, r, g, b, p, o, fo) =
        let p1 = Point.ofInts (100, 100)
        let p2 = Point.ofInts (500, 500)
        let p3 = Point.ofInts (200, 200)
        let point = Point.ofFloats (x, y)
        let path = Path.empty |> (Path.addAbsolute CurveTo p1) |> (Path.addAbsolute LineTo p2) |> (Path.addAbsolute CurveTo p3)
        let timing = Timing.create <| TimeSpan.FromSeconds(0.0)
        let fill, stroke, strokeWidth, opacity, fillOpacity = Hex c, Values(r, g, b), Length.ofFloat p, o, fo
        let style = Style.create fill stroke strokeWidth opacity fillOpacity
        let circle = Circle.create point (Length.ofFloat radius)
        let animation = Animation.createMotion timing path None
        let tag = circle |> Element.create |> Element.withStyle style |> Element.withAnimation animation |> Element.toString

        checkTag "circle" tag

    [<Fact>]
    let ``do lots and don't fail`` () =
        let points = seq {
            yield Point.ofInts (1, 1)
            yield Point.ofInts (4, 4)
            yield Point.ofInts (8, 8)
        }

        let point = Point.ofInts (24, 15)
        let style1 = Style.create (Name Colors.Red) (Hex 0xff0000) (Length.ofFloat 3.0) 1.0 1.0
        let style2 = Style.create (Name Colors.Blue) (SmallHex 0xf00s) (Length.ofFloat 6.0) 1.0 1.0
        let length = Length.ofPixels 1
        let area = Area.create length length

        let graphics = seq {
            yield Image.create point area "myimage1.jpg" |> Element.create
            yield Image.create point area "myimage2.jpg" |> Element.create |> Element.withStyle style1
            yield Text.create point "Hello World!" |> Element.create |> Element.withStyle style2
            yield Line.create point point |> Element.create |> Element.withStyle style1
            yield Rect.create point area |> Element.create |> Element.withStyle style2
            yield Circle.create point length |> Element.create
            yield Ellipse.create point point |> Element.create |> Element.withStyle style1
        }

        let html = graphics |> Svg.ofSeq |> Svg.toHtml "SVG Demo"
        checkTag "SVG Demo" html