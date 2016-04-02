module Tests
    open LogHelpers
    open SharpVG
    open SharpVG.Core
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
        let point1 = { X = Pixels(x1); Y = Pixels(y1) }
        let point2 = { X = Pixels(x2); Y = Pixels(y2) }
        let style = { Stroke = Values(r, g, b); StrokeWidth = Pixels(p); Fill = Hex(c); }
        let line = { Point1 = point1; Point2 = point2; }
        let tagString = SvgLine(line, Some(style)).toString

        basicChecks "line" tagString

    [<Property>]
    let ``draw rectangles`` (x, y, h, w, c, r, g, b, p) =
        configureLogs
        let point = { X = Pixels(x); Y = Pixels(y); }
        let area = { Height = Pixels(h); Width = Pixels(w); }
        let style = { Stroke = Values(r, g, b); StrokeWidth = Pixels(p); Fill = Hex(c); }
        let (rect : Rect) = { UpperLeft = point; Size = area; }
        let tagString = SvgRect(rect, Some(style)).toString

        basicChecks "rect" tagString

    [<Property>]
    let ``draw circles`` (x, y, radius, c, r, g, b, p) =
        configureLogs
        let point = { X = Pixels(x); Y = Pixels(y) }
        let style = { Stroke = Values(r, g, b); StrokeWidth = Pixels(p); Fill = Hex(c); }
        let circle = { Center = point; Radius = radius }
        let tagString = SvgCircle(circle, Some(style)).toString

        basicChecks "circle" tagString

    [<Fact>]
    let ``do lots and don't fail`` () =
        configureLogs
        let points = seq {
            yield {X = Pixels(1); Y = Pixels(1)}
            yield {X = Pixels(4); Y = Pixels(4)}
            yield {X = Pixels(10); Y = Pixels(10)}
        }
        let point = {X = Pixels(24); Y = Pixels(15)}
        let size = {Height = Pixels(30); Width = Pixels(30)}
        let style1 = {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Name(Colors.Red); }
        let style2 = {Stroke = (SmallHex(0xf00s)); StrokeWidth = Pixels(6); Fill = Name(Colors.Blue); }
        let transform = Transform.Scale(2, 5)

        let graphics = seq {
            yield SvgImage({ UpperLeft = point; Size = size; Source = "myimage.jpg"; }, None).toString
            yield SvgText({ UpperLeft = point; Body = "Hello World!"; }, Some(style1)).toString
            yield SvgText({ UpperLeft = point; Body =  "Hello World!"; }, Some(style2)).toString
            // TODO: Add: yield group "MyGroup" transform point { Element = Polygon(Polygon { Points = points; Style = Some(style) }) }.toString
            yield SvgPolyline(points, Some(style2)).toString
            yield SvgLine({ Point1 = point; Point2 = point; }, Some(style1)).toString
            yield SvgCircle({ Center = point; Radius = (Pixels(2)); }, Some(style2)).toString
            yield SvgEllipse({ Center = point; Radius = point; }, Some(style1)).toString
            yield SvgRect({ UpperLeft = point; Size = size; }, Some(style2)).toString
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
