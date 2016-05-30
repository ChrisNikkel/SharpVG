module Tests
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
let ``draw lines`` (x1, y1, x2, y2, c, r, g, b, p) =
    configureLogs
    let point1 = { X = Pixels x1; Y = Pixels y1 }
    let point2 = { X = Pixels x2; Y = Pixels y2 }
    let style = { Name = None; Stroke = Some(Values(r, g, b)); StrokeWidth = Some(Pixels p); Fill = Some(Hex c); Opacity = Some(1.0) }
    let line = Line.create point1 point2
    let tagString = line |> Element.ofLine |> Element.withStyle style |> Element.toString

    basicChecks "line" tagString

[<Property>]
let ``draw rectangles`` (x, y, h, w, c, r, g, b, p) =
    configureLogs
    let point = { X = Pixels x; Y = Pixels y }
    let area = { Height = Pixels h; Width = Pixels w }
    let style = { Name = None; Stroke = Some(Values(r, g, b)); StrokeWidth = Some(Pixels p); Fill = Some(Hex c); Opacity = Some(1.0) }
    let rect = Rect.create point area
    let tagString = rect |> Element.ofRect |> Element.withStyle style |> Element.toString

    basicChecks "rect" tagString

[<Property>]
let ``draw circles`` (x, y, radius, c, r, g, b, p) =
    configureLogs
    let point = { X = Pixels x; Y = Pixels y }
    let style = { Name = None; Stroke = Some(Values(r, g, b)); StrokeWidth = Some(Pixels p); Fill = Some(Hex c); Opacity = Some(1.0) }
    let circle = { Center = point; Radius = radius }
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