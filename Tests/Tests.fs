module SharpVGTests
open LogHelpers
open SharpVG
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
// TODO: Reenable do everything test
(*
[<Fact>]
let ``do lots and don't fail`` () =
    configureLogs
    let points = seq {
        yield {x = Pixels 1; y = Pixels 1}
        yield {x = Pixels 4; y = Pixels 4}
        yield {x = Pixels 10; y = Pixels 10}
    }
    let point = {x = Pixels 24; y = Pixels 15}
    let size = {height = Pixels 30; width = Pixels 30}
    let style1 = {stroke = Hex 0xff0000; strokeWidth = Pixels 3; fill = Name Colors.Red}
    let style2 = {stroke = SmallHex 0xf00s; strokeWidth = Pixels 6; fill = Name Colors.Blue}
    let transform = Scale(2, 5)

    let graphics = seq {
        yield { UpperLeft = point; Length = size; Source = "myimage.jpg" }, None).toString
        yield { UpperLeft = point; Body = "Hello World!" }, Some style1).toString
        yield SvgText({ UpperLeft = point; Body =  "Hello World!" }, Some style2).toString
        // TODO: Add: yield group "MyGroup" transform point { Element = Polygon(Polygon { Points = points; Style = Some(style) }) }.toString
        yield SvgPolyline(points, Some style2 ).toString
        yield SvgLine({ Point1 = point; Point2 = point }, Some style1).toString
        yield SvgCircle({ Center = point; Radius = (Pixels(2)) }, Some style2).toString
        yield SvgEllipse({ Center = point; Radius = point }, Some style1).toString
        yield SvgRect({ UpperLeft = point; Length = size }, Some style2).toString
// TODO: Add this
//            yield Script { Body = """
//            function circle_click(evt) {
//                var circle = evt.target;
//                var currentRadius = circle.getAttribute("r");
//                if (currentRadius == 100)
//                circle.setAttribute("r", currentRadius*2);
//                else
//                circle.setAttribute("r", currentRadius*0.5);
//            }
//            """ }
    }

    let styleBody = style
    let svgBody = graphics |> String.concat "\n  " |> (svg size)
    let body = seq {
        yield styleBody
        yield svgBody
    }

    body |> String.concat "\n" |> html "SVG Demo" |> isMatched '<' '>'
*)