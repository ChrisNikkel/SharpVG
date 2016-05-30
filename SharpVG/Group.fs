namespace SharpVG

type group = {
    Body: body
    UpperLeft: point option
    Transform: transform option
}
and groupElement =
    | Group of group
    | Element of element
and body =
    seq<groupElement>

module Group =
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e));
            UpperLeft = None;
            Transform = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq


    let withOffset upperLeft (group:group) =
        { group with UpperLeft = upperLeft }

    let withTransform transform (group:group) =
        { group with Transform = transform }

    let rec toString group =
        let body =
            group.Body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> toString)
            |> String.concat ""
        let transform =
            match group.Transform with Some(transform) -> (Transform.toString transform) + " " | None -> ""
        let upperLeft =
             match group.UpperLeft with | Some(upperLeft) -> Point.toString upperLeft | None -> ""
        "<g " + upperLeft + transform + ">" + body + "</g>"
    // TODO: Integrate tag into recursive function for Group.toString
    // let toTag group =
    //    {
    //        Name = "g";
    //        Attribute = ??;
    //        Body = body
    //    }