namespace SharpVG

type Pattern =
    {
        Id: ElementId
        Position: Point option
        Size: Area option
        PatternUnits: FilterUnits option
        PatternContentUnits: FilterUnits option
        PatternTransform: Transform option
        ViewBox: ViewBox option
        Body: Body
    }
with
    static member ToTag pattern =
        let body = Body.toString pattern.Body
        Tag.create "pattern"
        |> Tag.withAttributes
            ([
                [ Attribute.createXML "id" pattern.Id ]
                pattern.Position |> Option.map Point.toAttributes |> Option.defaultValue []
                pattern.Size |> Option.map Area.toAttributes |> Option.defaultValue []
                pattern.PatternUnits |> Option.map (fun u -> [ Attribute.createXML "patternUnits" (u.ToString()) ]) |> Option.defaultValue []
                pattern.PatternContentUnits |> Option.map (fun u -> [ Attribute.createXML "patternContentUnits" (u.ToString()) ]) |> Option.defaultValue []
                pattern.PatternTransform |> Option.map (fun t -> [ Attribute.createXML "patternTransform" (Transform.toString t) ]) |> Option.defaultValue []
                pattern.ViewBox |> Option.map ViewBox.toAttributes |> Option.defaultValue []
            ] |> List.concat)
        |> Tag.withBody body

    override this.ToString() =
        this |> Pattern.ToTag |> Tag.toString

module Pattern =
    let create id =
        { Id = id; Position = None; Size = None; PatternUnits = None; PatternContentUnits = None; PatternTransform = None; ViewBox = None; Body = Seq.empty }

    let withPosition position (pattern: Pattern) =
        { pattern with Position = Some position }

    let withSize size (pattern: Pattern) =
        { pattern with Size = Some size }

    let withPatternUnits units (pattern: Pattern) =
        { pattern with PatternUnits = Some units }

    let withPatternContentUnits units (pattern: Pattern) =
        { pattern with PatternContentUnits = Some units }

    let withPatternTransform transform (pattern: Pattern) =
        { pattern with PatternTransform = Some transform }

    let withViewBox viewBox (pattern: Pattern) =
        { pattern with ViewBox = Some viewBox }

    let addElement element (pattern: Pattern) =
        { pattern with Body = Seq.append pattern.Body (Seq.singleton (Element element)) }

    let addElements elements (pattern: Pattern) =
        { pattern with Body = Seq.append pattern.Body (elements |> Seq.map Element) }

    let ofElement id size element =
        create id |> withSize size |> addElement element

    let ofElements id size elements =
        create id |> withSize size |> addElements elements

    let toTag = Pattern.ToTag

    let toString (pattern: Pattern) = pattern.ToString()
