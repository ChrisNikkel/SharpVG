namespace SharpVG

type SvgDefinitionsContent =
    | ElementDef of Element
    | GroupDef of Group
    | GradientDef of Gradient
    | ClipPathDef of ClipPath
    | MarkerDef of Marker
    | FilterDef of Filter

type SvgDefinitions = {
    Contents: seq<SvgDefinitionsContent>
}
with
    static member ToTag definitions =
        let body =
            definitions.Contents
            |> Seq.map (function
                | ElementDef e -> e |> Element.toString
                | GroupDef g -> g |> Group.toString
                | GradientDef g -> g |> Gradient.toString
                | ClipPathDef cp -> cp |> ClipPath.toString
                | MarkerDef m -> m |> Marker.toString
                | FilterDef f -> f |> Filter.toString)
            |> String.concat ""
        Tag.create "defs"
        |> Tag.withBody body

    override this.ToString() =
        this |> SvgDefinitions.ToTag |> Tag.toString

module SvgDefinitions =
    let empty = { Contents = Seq.empty }

    let create = empty

    let withContents contents (definitions: SvgDefinitions) =
        { definitions with Contents = contents }

    let addElement element (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (ElementDef element)) }

    let addGroup group (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (GroupDef group)) }

    let addGradient gradient (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (GradientDef gradient)) }

    let addClipPath clipPath (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (ClipPathDef clipPath)) }

    let addMarker marker (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (MarkerDef marker)) }

    let addFilter filter (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (FilterDef filter)) }

    let addElements elements (definitions: SvgDefinitions) =
        elements
        |> Seq.map ElementDef
        |> fun newContents -> { definitions with Contents = Seq.append definitions.Contents newContents }

    let toStyles (definitions: SvgDefinitions) =
        definitions.Contents
        |> Seq.choose (function
            | ElementDef e -> Some (e.Style |> Option.toList |> Set.ofList)
            | GroupDef g -> Some (g |> Group.toStyleSet)
            | _ -> None)
        |> Seq.fold (+) Set.empty
        |> Set.toSeq

    let toTag =
        SvgDefinitions.ToTag

    let toString (definitions: SvgDefinitions) =
        definitions.ToString()
