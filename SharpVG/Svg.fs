namespace SharpVG

type viewbox = {
    Minimums: point
    Size: area
}

type svg = {
    Body: body
    Size: area option
    Viewbox: viewbox option
}

module Svg =
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e));
            Size = None;
            Viewbox = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let ofGroup group =
        {
            Body = seq { yield Group(group) }
            Size = None;
            Viewbox = None
        }

    let withSize size (svg:svg) =
        { svg with Size = Some(size) }

    let withViewbox viewbox (svg:svg) =
        { svg with Viewbox = Some(viewbox) }

    let toString svg =
        let viewbox =
            match svg.Viewbox with
                | Some(viewbox) -> " viewBox=" + Tag.quote ((Point.toString viewbox.Minimums) + " " + (Area.toString viewbox.Size))
                | None -> ""
        let size =
            match svg.Size with
                | Some(size) -> Area.toDescriptiveString size 
                | None -> ""
        let body =
            svg.Body
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