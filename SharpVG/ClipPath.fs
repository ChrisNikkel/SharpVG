namespace SharpVG

type ClipPath =
    {
        Id: ElementId
        ClipPathUnits: FilterUnits option
        Body: Body
    }
with
    static member ToTag clipPath =
        let body =
            clipPath.Body
            |> Seq.map (function
                | GroupElement.Element e -> e |> Element.toString
                | GroupElement.Group g -> g |> Group.toString)
            |> String.concat ""
        Tag.create "clipPath"
        |> Tag.withAttributes
            ([
                [ Attribute.createXML "id" clipPath.Id ]
                clipPath.ClipPathUnits |> Option.map (fun u -> [ Attribute.createXML "clipPathUnits" (u.ToString()) ]) |> Option.defaultValue []
            ] |> List.concat)
        |> Tag.withBody body

    override this.ToString() =
        this |> ClipPath.ToTag |> Tag.toString

module ClipPath =
    let create id =
        { Id = id; ClipPathUnits = None; Body = Seq.empty }

    let withClipPathUnits units (clipPath: ClipPath) =
        { clipPath with ClipPathUnits = Some units }

    let addElement element (clipPath: ClipPath) =
        { clipPath with Body = Seq.append clipPath.Body (Seq.singleton (GroupElement.Element element)) }

    let addElements elements (clipPath: ClipPath) =
        { clipPath with Body = Seq.append clipPath.Body (elements |> Seq.map GroupElement.Element) }

    let ofElement id element =
        create id |> addElement element

    let ofElements id elements =
        create id |> addElements elements

    let toTag = ClipPath.ToTag

    let toString (clipPath: ClipPath) = clipPath.ToString()
