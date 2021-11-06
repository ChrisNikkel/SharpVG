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
        Location : Point option
        Area : Area option
        FilterUnits : FilterUnits option
        PrimitiveUnits : FilterUnits option
        FilterEffects : FilterEffect list
    }
with
    static member ToTag filter =
        
        let body = filter.FilterEffects |> List.map FilterEffect.toString |> List.toSeq |> (String.concat "")

        Tag.create "filter"
            |> Tag.withAttributes (
                [
                    filter.Location |> Option.map Point.toAttributes
                    filter.Area |> Option.map Area.toAttributes
                    filter.FilterUnits |> Option.map (fun filterUnits -> [ Attribute.createXML "filterUnits" (filterUnits.ToString()) ])
                    filter.PrimitiveUnits |> Option.map (fun primitiveUnits -> [ Attribute.createXML "primitiveUnits" (primitiveUnits.ToString()) ])
                ] |> List.choose id |> List.concat
            )
            |> (Tag.withBody body)

    override this.ToString() =
        this |> Filter.ToTag |> Tag.toString

module Filter =
    let empty =
        { Location = None; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = list.Empty }

    let create filterEffect =
        { Location = None; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithLocation filterEffect location =
        { Location = location; Area = None; FilterUnits = None; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithFilterUnits filterEffect filterUnits =
        { Location = None; Area = None; FilterUnits = filterUnits; PrimitiveUnits = None; FilterEffects = List.singleton filterEffect }

    let createWithPrimitiveUnits filterEffect filterUnits =
        { Location = None; Area = None; FilterUnits = None; PrimitiveUnits = filterUnits; FilterEffects = List.singleton filterEffect }

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

    let addFilterEffect filter filterEffect =
        { filter with FilterEffects = filter.FilterEffects |> List.append filterEffect }

    let toTag =
        Filter.ToTag

    let toString (filter : Filter) =
        filter.ToString()