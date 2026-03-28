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
        PreserveAspectRatio: PreserveAspectRatio option
        Body: Body
    }
with
    static member ToTag pattern =
        let body = Body.toString pattern.Body
        Tag.create "pattern"
        |> Tag.withAttribute (Attribute.createXML "id" pattern.Id)
        |> (match pattern.Position with Some p -> Tag.addAttributes (Point.toAttributes p) | None -> id)
        |> (match pattern.Size with Some s -> Tag.addAttributes (Area.toAttributes s) | None -> id)
        |> (match pattern.PatternUnits with Some u -> Tag.addAttributes [Attribute.createXML "patternUnits" (u.ToString())] | None -> id)
        |> (match pattern.PatternContentUnits with Some u -> Tag.addAttributes [Attribute.createXML "patternContentUnits" (u.ToString())] | None -> id)
        |> (match pattern.PatternTransform with Some t -> Tag.addAttributes [Attribute.createXML "patternTransform" (Transform.toString t)] | None -> id)
        |> (match pattern.ViewBox with Some v -> Tag.addAttributes (ViewBox.toAttributes v) | None -> id)
        |> (match pattern.PreserveAspectRatio with Some par -> Tag.addAttributes [PreserveAspectRatio.toAttribute par] | None -> id)
        |> Tag.withBody body

    override this.ToString() =
        this |> Pattern.ToTag |> Tag.toString

module Pattern =
    let create id =
        { Id = id; Position = None; Size = None; PatternUnits = None; PatternContentUnits = None; PatternTransform = None; ViewBox = None; PreserveAspectRatio = None; Body = Seq.empty }

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

    let withPreserveAspectRatio par (pattern: Pattern) =
        { pattern with PreserveAspectRatio = Some par }

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
