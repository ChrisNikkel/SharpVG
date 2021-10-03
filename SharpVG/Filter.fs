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
        Offset : Point option
        Area : Area option
        FilterUnits : FilterUnits option
        PrimitiveUnits : FilterUnits option
    }
with
    static member ToTag filter =
        Tag.create "filter"
        |> Tag.withAttributes (
            [
                filter.Offset |> Option.map Point.toAttributes
                filter.Area |> Option.map Area.toAttributes
                filter.FilterUnits |> Option.map (fun filterUnits -> [ Attribute.createXML "filterUnits" (filterUnits.ToString()) ])
                filter.PrimitiveUnits |> Option.map (fun primitiveUnits -> [ Attribute.createXML "primitiveUnits" (primitiveUnits.ToString()) ])
            ] |> List.choose id |> List.concat
        )

    override this.ToString() =
        this |> Filter.ToTag |> Tag.toString

module Filter =
    let empty =
        {
            Offset = None; Area = None; FilterUnits = None; PrimitiveUnits = None;
        }

    let create = empty // TODO: Figure out what the most useful create function would be (which are likely inner tags)

    let toTag =
        Filter.ToTag

    let toString (filter : Filter) =
        filter.ToString()