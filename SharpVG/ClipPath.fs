namespace SharpVG

type ClipPath =
    {
        Id: ElementId
        ClipPathUnits: FilterUnits option
        Body: Body
    }
with
    static member ToTag clipPath =
        let body = Body.toString clipPath.Body
        Tag.create "clipPath"
        |> Tag.withAttribute (Attribute.createXML "id" clipPath.Id)
        |> (match clipPath.ClipPathUnits with Some u -> Tag.addAttributes [Attribute.createXML "clipPathUnits" (u.ToString())] | None -> id)
        |> Tag.withBody body

    override this.ToString() =
        this |> ClipPath.ToTag |> Tag.toString

module ClipPath =
    let create id =
        { Id = id; ClipPathUnits = None; Body = Seq.empty }

    let withClipPathUnits units (clipPath: ClipPath) =
        { clipPath with ClipPathUnits = Some units }

    let addElement element (clipPath: ClipPath) =
        { clipPath with Body = Seq.append clipPath.Body (Seq.singleton (Element element)) }

    let addElements elements (clipPath: ClipPath) =
        { clipPath with Body = Seq.append clipPath.Body (elements |> Seq.map Element) }

    let ofElement id element =
        create id |> addElement element

    let ofElements id elements =
        create id |> addElements elements

    let toTag = ClipPath.ToTag

    let toString (clipPath: ClipPath) = clipPath.ToString()
