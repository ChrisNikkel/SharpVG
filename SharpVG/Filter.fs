namespace SharpVG

type FilterUnits =
    | UserSpaceOnUse
    | ObjectBoundingBox
with
    override this.ToString() = 
        match this with
            | UserSpaceOnUse -> "userSpaceOnUse"
            | ObjectBoundingBox -> "objectBoundingBox"

// TODO: Make it so you can use feImage to define "In" or maybe "In2" or instead use a ElementId.  Not sure if this should go in Filters or Effects.

type Filter =
    {
        Id : ElementId option
        Location : Point option
        Area : Area option
        FilterUnits : FilterUnits option
        PrimitiveUnits : FilterUnits option
        FilterEffects : FilterEffect list
    }
with
    static member ToTag filter =

        let body = filter.FilterEffects |> List.map FilterEffect.toString |> String.concat ""

        Tag.create "filter"
        |> (match filter.Id with Some i -> Tag.withAttribute (Attribute.createXML "id" i) | None -> id)
        |> (match filter.Location with Some p -> Tag.addAttributes (Point.toAttributes p) | None -> id)
        |> (match filter.Area with Some a -> Tag.addAttributes (Area.toAttributes a) | None -> id)
        |> (match filter.FilterUnits with Some u -> Tag.addAttributes [Attribute.createXML "filterUnits" (u.ToString())] | None -> id)
        |> (match filter.PrimitiveUnits with Some u -> Tag.addAttributes [Attribute.createXML "primitiveUnits" (u.ToString())] | None -> id)
        |> Tag.withBody body

    override this.ToString() =
        this |> Filter.ToTag |> Tag.toString

module Filter =
    let empty =
        { Id = None; Location = None; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = list.Empty }

    let create filterEffect =
        { Id = None; Location = None; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithLocation filterEffect location =
        { Id = None; Location = location; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithFilterUnits filterEffect filterUnits =
        { Id = None; Location = None; Area = None; FilterUnits = filterUnits; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithPrimitiveUnits filterEffect filterUnits =
        { Id = None; Location = None; Area = None; FilterUnits = None; PrimitiveUnits = filterUnits; FilterEffects = List.singleton filterEffect }

    let withId id filter =
        { filter with Id = Some id }

    let withLocation filter location =
        { filter with Location = location }

    let withFilterUnits filter filterUnits =    
        { filter with FilterUnits = filterUnits }

    let withPrimitiveUnits filter primitiveUnits =
        { filter with PrimitiveUnits = primitiveUnits }

    let withFilterEffect filter filterEffect =
        { filter with FilterEffects = List.singleton filterEffect }

    let withFilterEffects filter filterEffects =
        { filter with FilterEffects = filterEffects }

    let addFilterEffect filterEffect filter =
        { filter with FilterEffects = filter.FilterEffects @ [filterEffect] }

    let toTag =
        Filter.ToTag

    let toString (filter : Filter) =
        filter.ToString()