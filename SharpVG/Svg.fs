namespace SharpVG

type Svg = {
    Body: Body
    Definitions: SvgDefinitions option
    Size: Area option
    ViewBox: ViewBox option
    PreserveAspectRatio: PreserveAspectRatio option
    Title: string option
    Description: string option
}
with
    override this.ToString() =
        let attributes =
            Attribute.create AttributeType.XML "xmlns" "http://www.w3.org/2000/svg"
            :: match this.Size with | Some size -> Area.toAttributes size | None -> []
            @ match this.ViewBox with | Some viewBox -> ViewBox.toAttributes viewBox | None -> []
            @ match this.PreserveAspectRatio with | Some par -> [PreserveAspectRatio.toAttribute par] | None -> []

        let styles =
            let bodyStyles = this.Body |> Body.toStyles
            let definitionsStyles = this.Definitions |> Option.map SvgDefinitions.toStyles |> Option.defaultValue Seq.empty
            let namedStyles = Seq.append bodyStyles definitionsStyles |> Styles.named
            if Seq.isEmpty namedStyles then
                ""
            else
                Styles.toString namedStyles

        let titleBlock =
            this.Title |> Option.map (fun t -> "<title>" + t + "</title>") |> Option.defaultValue ""

        let descBlock =
            this.Description |> Option.map (fun d -> "<desc>" + d + "</desc>") |> Option.defaultValue ""

        let definitionsBlock =
            this.Definitions |> Option.map SvgDefinitions.toString |> Option.defaultValue ""

        let body = Body.toString this.Body

        Tag.create "svg"
        |> Tag.withAttributes attributes
        |> Tag.withBody (titleBlock + descBlock + styles + definitionsBlock + body)
        |> Tag.toString

module Svg =
    let withSize size (svg:Svg) =
        { svg with Size = Some(size) }

    let withViewBox viewBox (svg:Svg) =
        { svg with ViewBox = Some(viewBox) }

    let withPreserveAspectRatio par (svg: Svg) =
        { svg with PreserveAspectRatio = Some par }

    let withTitle title (svg:Svg) =
        { svg with Title = Some title }

    let withDescription description (svg:Svg) =
        { svg with Description = Some description }

    /// <summary>
    /// This function takes seqence of elements and creates a simple svg object.
    /// </summary>
    /// <param name="seq">The sequence of elements</param>
    /// <returns>The svg object containing the elements.</returns>
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> GroupElement.Element(e))
            Definitions = None
            Size = None
            ViewBox = None
            PreserveAspectRatio = None
            Title = None
            Description = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofElement element =
        element |> Seq.singleton |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let ofGroup group =
        {
            Body = seq { yield GroupElement.Group(group) }
            Definitions = None
            Size = Some(Area.full)
            ViewBox = None
            PreserveAspectRatio = None
            Title = None
            Description = None
        }

    let withDefinitions definitions (svg: Svg) =
        { svg with Definitions = Some definitions }

    let ofElementsWithDefinitions definitions elements =
        elements |> ofSeq |> withDefinitions definitions

    let toString (svg : Svg) =
        svg.ToString()

    let private escapeHtmlTitle (s: string) =
        let s = Option.ofObj s |> Option.defaultValue ""
        s.Replace("&", "&amp;").Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;")

    let toHtmlWithCss title css svg =
        let styleBlock = if System.String.IsNullOrEmpty(css) then "" else "\n<style>\n" + css + "\n</style>"
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + escapeHtmlTitle title + "</title>" + styleBlock + "\n</head>\n<body>\n" + toString svg + "\n</body>\n</html>\n"

    let toHtml title svg =
        toHtmlWithCss title "" svg

    // --- Mutation helpers ---

    let mapElements (f: Element -> Element) (svg: Svg) : Svg =
        let rec mapBody (body: Body) : Body =
            body |> Seq.map (function
                | GroupElement.Element e when not (Element.isRaw e) -> GroupElement.Element (f e)
                | GroupElement.Group g   -> GroupElement.Group { g with Body = mapBody g.Body }
                | other -> other)
        { svg with Body = mapBody svg.Body }

    let mapElementsWhere (predicate: Element -> bool) (f: Element -> Element) (svg: Svg) : Svg =
        mapElements (fun e -> if predicate e then f e else e) svg

    let findById (id: ElementId) (svg: Svg) : Element option =
        let rec searchBody (body: Body) : Element option =
            body |> Seq.tryPick (function
                | GroupElement.Element e when e.Name = Some id -> Some e
                | GroupElement.Group g -> searchBody g.Body
                | _ -> None)
        searchBody svg.Body

    let findAll (predicate: Element -> bool) (svg: Svg) : Element list =
        let rec collectBody (body: Body) : Element list =
            body |> Seq.toList |> List.collect (function
                | GroupElement.Element e when not (Element.isRaw e) -> if predicate e then [e] else []
                | GroupElement.Group g   -> collectBody g.Body
                | _ -> [])
        collectBody svg.Body

    let replaceById (id: ElementId) (replacement: Element) (svg: Svg) : Svg =
        let rec replaceBody (body: Body) : Body =
            body |> Seq.map (function
                | GroupElement.Element e when e.Name = Some id -> GroupElement.Element replacement
                | GroupElement.Group g -> GroupElement.Group { g with Body = replaceBody g.Body }
                | other -> other)
        { svg with Body = replaceBody svg.Body }

    let addElement (element: Element) (svg: Svg) : Svg =
        { svg with Body = Seq.append svg.Body (Seq.singleton (GroupElement.Element element)) }

    let addElements (elements: seq<Element>) (svg: Svg) : Svg =
        { svg with Body = Seq.append svg.Body (elements |> Seq.map GroupElement.Element) }

    let addGroup (group: Group) (svg: Svg) : Svg =
        { svg with Body = Seq.append svg.Body (Seq.singleton (GroupElement.Group group)) }

    let removeById (id: ElementId) (svg: Svg) : Svg =
        let rec filterBody (body: Body) : Body =
            body |> Seq.choose (function
                | GroupElement.Element e when e.Name = Some id -> None
                | GroupElement.Group g -> Some (GroupElement.Group { g with Body = filterBody g.Body })
                | other -> Some other)
        { svg with Body = filterBody svg.Body }

    let removeWhere (predicate: Element -> bool) (svg: Svg) : Svg =
        let rec filterBody (body: Body) : Body =
            body |> Seq.choose (function
                | GroupElement.Element e when not (Element.isRaw e) && predicate e -> None
                | GroupElement.Group g -> Some (GroupElement.Group { g with Body = filterBody g.Body })
                | other -> Some other)
        { svg with Body = filterBody svg.Body }

    // --- Editor rendering support ---

    /// Parses a `data-edit-id` attribute value (colon-separated integers) back to a tree path.
    let parseEditPath (editId: string) : int list option =
        try
            if System.String.IsNullOrEmpty(editId) then None
            else editId.Split(':') |> Array.map int |> Array.toList |> Some
        with _ -> None

    /// Renders the SVG with a `data-edit-id` attribute injected into every element,
    /// encoding each element's tree position as a colon-separated index path
    /// (e.g. "0", "1:2", "1:2:0" for nested elements).
    /// These IDs are ephemeral — they are not part of the model and are absent from toString/toHtml.
    let toStringForEditing (svg: Svg) : string =
        let rec annotateBody (prefix: int list) (body: Body) : Body =
            body |> Seq.mapi (fun i ge ->
                let path = prefix @ [i]
                let editId = path |> List.map string |> String.concat ":"
                match ge with
                | GroupElement.Element e when not (Element.isRaw e) ->
                    GroupElement.Element (e |> Element.withAttribute "data-edit-id" editId)
                | GroupElement.Element e -> GroupElement.Element e
                | GroupElement.Group g ->
                    GroupElement.Group { g with Body = annotateBody path g.Body })
        { svg with Body = annotateBody [] svg.Body } |> toString

    /// Renders a full HTML page with `data-edit-id` attributes injected for editor use.
    let toHtmlForEditing (title: string) (svg: Svg) : string =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + escapeHtmlTitle title + "</title>\n</head>\n<body>\n" + toStringForEditing svg + "\n</body>\n</html>\n"

    /// Finds the element at the given tree path (as returned by parseEditPath).
    let findAtEditPath (path: int list) (svg: Svg) : Element option =
        let rec findInBody (remaining: int list) (body: Body) : Element option =
            match remaining with
            | [] -> None
            | [i] ->
                body |> Seq.tryItem i |> Option.bind (function
                    | GroupElement.Element e -> Some e
                    | _ -> None)
            | i :: rest ->
                body |> Seq.tryItem i |> Option.bind (function
                    | GroupElement.Group g -> findInBody rest g.Body
                    | _ -> None)
        findInBody path svg.Body

    /// Applies a transformation to the element at the given tree path.
    let mapAtEditPath (path: int list) (f: Element -> Element) (svg: Svg) : Svg =
        let rec mapBody (remaining: int list) (body: Body) : Body =
            match remaining with
            | [] -> body
            | [i] ->
                body |> Seq.mapi (fun j ge ->
                    if j = i then
                        match ge with
                        | GroupElement.Element e -> GroupElement.Element (f e)
                        | other -> other
                    else ge)
            | i :: rest ->
                body |> Seq.mapi (fun j ge ->
                    if j = i then
                        match ge with
                        | GroupElement.Group g -> GroupElement.Group { g with Body = mapBody rest g.Body }
                        | other -> other
                    else ge)
        { svg with Body = mapBody path svg.Body }
