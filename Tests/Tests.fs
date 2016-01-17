module Tests
    open SharpVG
    open SharpVG.Core
    open Xunit
    open FsCheck
    open FsCheck.Xunit
    open log4net
    open log4net.Config

    let _log = LogManager.GetLogger "Tests"
    let debug format = Printf.ksprintf _log.Debug format
    let info format = Printf.ksprintf _log.Info format
    let warn format = Printf.ksprintf _log.Warn format
    let error format = Printf.ksprintf _log.Error format
    let fatal format = Printf.ksprintf _log.Fatal format

    type Positive =
        static member Int() =
            Arb.Default.Int32()
            |> Arb.mapFilter abs (fun t -> t > 0)

    let isTagEnclosed (tag:string) =
        let trimmedTag = tag.Trim()
        trimmedTag.[0..0] = "<" && trimmedTag.[(trimmedTag.Length - 2)..(trimmedTag.Length - 1)] = "/>"

    let happensEvenly chr (str:string) =
        str.ToCharArray()
        |> Array.fold
            (fun acc c -> if chr = c then acc + 1 else acc) 0
        |> (%) 2 = 0

    let isMatched left right (str:string) =
        str.ToCharArray()
        |> Array.fold
            (fun acc c ->
                match c with
                    | c when c = left -> acc + 1
                    | c when c = right -> acc - 1
                    | _ -> acc
            ) 0 = 0

    [<Property>]
    let ``draw lines`` (x1, y1, x2, y2, c, r, g, b, p) =
        BasicConfigurator.Configure() |> ignore
        let point1 = {X = Size.Pixels(x1); Y = Size.Pixels(y1)}
        let point2 = {X = Size.Pixels(x2); Y = Size.Pixels(y2)}
        let tagString =
            line {
                Stroke = Color.Values(r, g, b);
                StrokeWidth = Pixels(p);
                Fill = Color.Hex(c);
            } point1 point2

        info "%d %s" c tagString
        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "line")

    [<Property>]
    let ``draw rectangles`` (x, y, h, w, c, r, g, b, p) =
        BasicConfigurator.Configure() |> ignore
        let point = {X = Size.Pixels(x); Y = Size.Pixels(y)}
        let area = {Height = Size.Pixels(h); Width = Size.Pixels(w)}
        let tagString =
            rect {
                Stroke = Color.Values(r, g, b);
                StrokeWidth = Pixels(p);
                Fill = Color.Hex(c);
            } point area

        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "rect")

    [<Property>]
    let ``draw circles`` (x, y, radius, c, r, g, b, p) =
        BasicConfigurator.Configure() |> ignore
        let point = {X = Size.Pixels(x); Y = Size.Pixels(y)}
        let tagString =
            circle {
                Stroke = Color.Values(r, g, b);
                StrokeWidth = Pixels(p);
                Fill = Color.Hex(c);
            } point radius

        isTagEnclosed tagString
        && (isMatched '<' '>' tagString)
        && (tagString.Contains "circle")

    [<Fact>]
    let ``do lots and don't fail`` () =
        BasicConfigurator.Configure() |> ignore
        let points = seq {
            yield {X = Size.Pixels(1); Y = Size.Pixels(1)}
            yield {X = Size.Pixels(4); Y = Size.Pixels(4)}
            yield {X = Size.Pixels(10); Y = Size.Pixels(10)}
        }
        let point = {X = Size.Pixels(24); Y = Size.Pixels(15)}
        let size = {Height = Size.Pixels(30); Width = Size.Pixels(30)}
        let style1 = {Stroke = (Hex(0xff0000)); StrokeWidth = Pixels(3); Fill = Color.Name(Colors.Red); }
        let style2 = {Stroke = (SmallHex(0xf00s)); StrokeWidth = Pixels(6); Fill = Color.Name(Colors.Blue); }


        let graphics = seq {
            yield image point size "myimage.jpg"
            yield text style1 point "Hello World!"
            yield text style2 point "Hello World!"
            yield polygon style1 points
            yield polyline style2 points
            yield line style1 point point
            yield circle style2 point (Pixels(2))
            yield ellipse style1 point point
            yield rect style2 point size
            yield script """
            function circle_click(evt) {
                var circle = evt.target;
                var currentRadius = circle.getAttribute("r");
                if (currentRadius == 100)
                circle.setAttribute("r", currentRadius*2);
                else
                circle.setAttribute("r", currentRadius*0.5);
            }
            """
        }

        let styleBody = style
        let svgBody = graphics |> String.concat "\n  " |> (svg size)
        let body = seq {
            yield styleBody
            yield svgBody
        }

        body |> String.concat "\n" |> html "SVG Demo" |> isMatched '<' '>'
