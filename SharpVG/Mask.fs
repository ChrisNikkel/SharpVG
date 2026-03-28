namespace SharpVG

type Mask =
    {
        Id: ElementId
        MaskUnits: FilterUnits option
        MaskContentUnits: FilterUnits option
        Location: Point option
        Size: Area option
        Body: Body
    }
with
    static member ToTag mask =
        let body = Body.toString mask.Body
        Tag.create "mask"
        |> Tag.withAttribute (Attribute.createXML "id" mask.Id)
        |> (match mask.MaskUnits with Some u -> Tag.addAttributes [Attribute.createXML "maskUnits" (u.ToString())] | None -> id)
        |> (match mask.MaskContentUnits with Some u -> Tag.addAttributes [Attribute.createXML "maskContentUnits" (u.ToString())] | None -> id)
        |> (match mask.Location with Some p -> Tag.addAttributes (Point.toAttributes p) | None -> id)
        |> (match mask.Size with Some s -> Tag.addAttributes (Area.toAttributes s) | None -> id)
        |> Tag.withBody body

    override this.ToString() =
        this |> Mask.ToTag |> Tag.toString

module Mask =
    let create id =
        { Id = id; MaskUnits = None; MaskContentUnits = None; Location = None; Size = None; Body = Seq.empty }

    let withMaskUnits units (mask: Mask) =
        { mask with MaskUnits = Some units }

    let withMaskContentUnits units (mask: Mask) =
        { mask with MaskContentUnits = Some units }

    let withLocation location (mask: Mask) =
        { mask with Location = Some location }

    let withSize size (mask: Mask) =
        { mask with Size = Some size }

    let addElement element (mask: Mask) =
        { mask with Body = Seq.append mask.Body (Seq.singleton (Element element)) }

    let addElements elements (mask: Mask) =
        { mask with Body = Seq.append mask.Body (elements |> Seq.map Element) }

    let ofElement id element =
        create id |> addElement element

    let ofElements id elements =
        create id |> addElements elements

    let toTag = Mask.ToTag

    let toString (mask: Mask) = mask.ToString()
