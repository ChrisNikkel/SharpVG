namespace SharpVG

type viewbox = {
    minimums: point
    size: area
}

type svg = {
    body: body
    size: area option
    viewbox: viewbox option
}

module Svg =
    let ofSeq seq =
        {
            body = seq |> Seq.map (fun e -> Element(e));
            size = None;
            viewbox = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let withSize size (svg:svg) =
        { svg with size = Some(size) }

    let withViewbox viewbox (svg:svg) =
        { svg with viewbox = Some(viewbox) }

    let toString svg =
        let viewbox =
            match svg.viewbox with
                | Some(viewbox) -> " viewBox=" + Tag.quote ((Point.toString viewbox.minimums) + " " + (Area.toString viewbox.size))
                | None -> ""
        let size =
            match svg.size with
                | Some(size) -> Area.toDescriptiveString size 
                | None -> ""
        let body =
            svg.body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> Group.toString)
            |> String.concat ""

        "<svg " + size + viewbox + ">\n" +  body + "\n</svg>\n"

    let toHtml title svg =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + title + "</title>\n</head>\n<body>\n" + (toString svg) + "</body>\n</html>\n"

    // TODO: Add styles that can be referred to in later elements
    let style =
        "<style>
        circle {
        stroke: #006600;
        fill  : #00cc00;
        }
        circle.allBlack {
        stroke: #000000;
        fill  : #000000;
        }\n" +
        "</style>\n"