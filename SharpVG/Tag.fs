namespace SharpVG

type HtmlCanvasCode =
    {
        Code : string
        Parameters : string list
    }

type Tag =
    {
        Name : string
        Attributes : Attribute list
        Body : string option
        HtmlCanvasCode : HtmlCanvasCode option
    }
with
    override this.ToString() =
        let attributesToString attributes = (attributes |> List.map Attribute.toString |> String.concat " ")
        match (List.isEmpty this.Attributes), this with
        | false, { Name = n; Attributes = a; Body = Some(b) } -> "<" + n + " " + (attributesToString a) + ">" + b + "</" + n + ">"
        | false, { Name = n; Attributes = a; Body = None } -> "<" + n + " " + (attributesToString a) + "/>"
        | true, { Name = n; Attributes = a; Body = Some(b) } -> "<" + n + ">" + b + "</" + n + ">"
        | true, { Name = n; Attributes = a; Body = None } -> "<" + n + "/>"

module Tag =

    let create name =
        { Name = name; Attributes = List.empty; Body = None; HtmlCanvasCode = None }

    let withHtmlCanvasCode code parameters tag =
        { tag with HtmlCanvasCode = Some { Code = code; Parameters = parameters } }

    let withAttribute attribute tag =
        { tag with Attributes = List.singleton attribute }

    let withAttributes attributes tag =
        { tag with Attributes = attributes }

    let withBody body tag =
        { tag with Body = Option.ofObj body }

    let addAttributes attributes tag =
        tag
        |> withAttributes (tag.Attributes @ attributes |> List.distinctBy (fun attribute -> attribute.Name))

    let addAttribute attribute tag =
        tag
        |> addAttributes (List.singleton attribute)

    let insertAttributes attributes tag =
        tag
        |> withAttributes (attributes @ tag.Attributes |> List.distinctBy (fun attribute -> attribute.Name))

    let insertAttribute attribute tag =
        tag
        |> insertAttributes (List.singleton attribute)

    let addBody body tag =
        tag
        |> withBody (
            match tag.Body with
                | Some(b) -> b + body
                | None -> body
        )

    let toString (tag : Tag) =
        tag.ToString()
