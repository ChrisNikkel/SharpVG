namespace SharpVG

type Group = {
    Body: Body
    UpperLeft: Point option
    Transform: Transform option
}
and GroupElement =
    | Group of Group
    | Element of Element
and Body =
    seq<GroupElement>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
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

    let withOffset upperLeft (group:Group) =
        { group with UpperLeft = Some upperLeft }

    let withTransform transform (group:Group) =
        { group with Transform = Some transform }

    let asCartesian x y (group:Group) =
        group
        |> withTransform (Transform.createWithScale (1.0, -1.0) |> Transform.withTranslate (x, y))

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