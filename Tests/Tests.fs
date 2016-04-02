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
        let point1 = { X = Size.Pixels(x1); Y = Size.Pixels(y1) }
        let point2 = { X = Size.Pixels(x2); Y = Size.Pixels(y2) }
        let style = { Stroke = Color.Values(r, g, b); StrokeWidth = Pixels(p); Fill = Color.Hex(c); }
        let line = { Point1 = point1; Point2 = point2; }
        let tagString = SvgLine(line, Some(style)).toString

        info "%d %s" c tagString
        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "line")

    [<Property>]
    let ``draw rectangles`` (x, y, h, w, c, r, g, b, p) =
        configureLogs
        let point = { X = Size.Pixels(x); Y = Size.Pixels(y); }
        let area = { Height = Size.Pixels(h); Width = Size.Pixels(w); }
        let style = { Stroke = Color.Values(r, g, b); StrokeWidth = Pixels(p); Fill = Color.Hex(c); }
        let (rect : Rect) = { UpperLeft = point; Size = area; }
        let tagString = SvgRect(rect, Some(style)).toString

        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "rect")

    [<Property>]
    let ``draw circles`` (x, y, radius, c, r, g, b, p) =
        configureLogs
        let point = { X = Size.Pixels(x); Y = Size.Pixels(y) }
        let style = { Stroke = Color.Values(r, g, b); StrokeWidth = Pixels(p); Fill = Color.Hex(c); }
        let circle = { Center = point; Radius = radius }
        let tagString = SvgCircle(circle, Some(style)).toString

        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "circle")

    [<Fact>]
    let ``do lots and don't fail`` () =
        configureLogs
        let points = seq {
            yield {X = Size.Pixels(1); Y = Size.Pixels(1)}
            yield {X = Size.Pixels(4); Y = Size.Pixels(4)}
            yield {X = Size.Pixels(10); Y = Size.Pixels(10)}
        }
        let point = {X = Size.Pixels(24); Y = Size.Pixels(15)}
        let size = {Height = Size.Pixels(30); Width = Size.Pixels(30)}
        let style1 = {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Color.Name(Colors.Red); }
        let style2 = {Stroke = (SmallHex(0xf00s)); StrokeWidth = Pixels(6); Fill = Color.Name(Colors.Blue); }
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
