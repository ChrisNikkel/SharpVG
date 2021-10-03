namespace SharpVG

type HtmlCanvas = {
    Body: Body
    Size: Area option
    // TODO: Add Styling of boarder of HTML5 Canvas
}
with
    override this.ToString() =
        let attributes =
            match this.Size with | Some size -> Area.toAttributes size | None -> []

        let body =
            if Seq.isEmpty this.Body then
                ""
            else
                this.Body
                |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> Group.toString)
                |> String.concat ""

        Tag.create "canvas"
        |> Tag.withAttributes attributes
        |> Tag.withBody body
        |> Tag.toString

module HtmlCanvas =
    let withSize size (canvas : Canvas) =
        { canvas with Size = Some(size) }

    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e))
            Size = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofElement element =
        element |> Seq.singleton |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let toString (canvas : Canvas) =
        canvas.ToString()

    let toHtml title canvas =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + title + "</title>\n</head>\n<body>\n" + (toString canvas) + "\n</body>\n</html>\n"
