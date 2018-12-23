namespace SharpVG

type Group = {
    Id: string option
    Body: Body
    Transforms: seq<Transform>
}
and GroupElement =
    | Group of Group
    | Element of Element
and Body =
    seq<GroupElement>

module Group =
    let empty =
        { Id = None; Body = Seq.empty<GroupElement>; Transforms = Seq.empty }

    let ofSeq seq =
        {
            Id = None
            Body = seq |> Seq.map (fun e -> Element(e))
            Transforms = Seq.empty
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let private withBody body (group:Group) =
        { group with Body = body }

    let withTransforms transforms group =
        { group with Transforms = transforms }

    let withTransform transform group =
        { group with Transforms = Seq.singleton transform }

    let withId idName (group:Group) =
        { group with Id = Some idName }

    let addTransform transform group =
        group |> withTransforms (Seq.append (Seq.singleton transform) group.Transforms)

    let addElements elements group =
        group |> withBody (Seq.append group.Body (elements |> Seq.map Element))

    let addElement element group =
        addElements (Seq.singleton element) group

    let asCartesian x y group =
        group
        |> withTransform (Transform.createScale (UserSpace 1.0) |> Transform.withY (UserSpace -1.0))
        |> addTransform (Transform.createTranslate x |> Transform.withY y)

    let rec toStyleSet group =
        group.Body
            |> Seq.map (function | Element(e) -> e.Body.Style |> Option.toList |> Set.ofList | Group(g) -> g |> toStyleSet)
            |> Seq.reduce (+)

    let rec toString group =
        group
        |> toTag
        |> Tag.toString

    and toTag group =
        let body =
            group.Body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> toString)
            |> String.concat ""

        let attributes =
            [
                group.Id |> Option.map (Attribute.createXML "Id")
                (if group.Transforms |> Seq.isEmpty then None else Some (group.Transforms |> Transforms.toAttribute))
            ]
            |> List.choose id

        (if attributes |> List.isEmpty then Tag.create "g" else Tag.create "g" |> (Tag.withAttributes attributes)) |> Tag.withBody body

module Body =
    let toStyles body =
        body
        |> Seq.map (fun b ->
            match b with
                | Group(g) -> g |> Group.toStyleSet
                | Element(e) -> e.Body.Style |> Option.toList |> Set.ofList)
        |> Seq.reduce (+)
        |> Set.toSeq