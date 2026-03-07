namespace SharpVG

type SvgDefinitionsContent =
    | Element of Element
    | Group of Group

type SvgDefinitions = {
    Contents: seq<SvgDefinitionsContent>
}
with
    static member ToTag definitions =
        let body =
            definitions.Contents
            |> Seq.map (function
                | Element e -> e |> Element.toString
                | Group g -> g |> Group.toString)
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
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (Element element)) }

    let addGroup group (definitions: SvgDefinitions) =
        { definitions with Contents = Seq.append definitions.Contents (Seq.singleton (Group group)) }

    let addElements elements (definitions: SvgDefinitions) =
        elements
        |> Seq.map Element
        |> fun newContents -> { definitions with Contents = Seq.append definitions.Contents newContents }

    let toStyles (definitions: SvgDefinitions) =
        definitions.Contents
        |> Seq.map (function
            | Element e -> e.Style |> Option.toList |> Set.ofList
            | Group g -> g |> Group.toStyleSet)
        |> Seq.fold (+) Set.empty
        |> Set.toSeq

    let toTag =
        SvgDefinitions.ToTag

    let toString (definitions: SvgDefinitions) =
        definitions.ToString()
