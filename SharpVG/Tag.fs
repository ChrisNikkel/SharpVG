namespace SharpVG

type Tag =
    {
        Name: string;
        Attribute: string option;
        Body: string option;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Tag =
    let create name =
        { Name = name; Attribute = None; Body = None }

    let withAttribute attribute tag =
        { tag with Attribute = Some(attribute) }

    let withBody body tag =
        { tag with Body = Some(body) }

    let addAttribute attribute tag =
        tag 
        |> withAttribute (
            match tag.Attribute with
                | Some(a) -> a + " " + attribute
                | None -> attribute
        )

    let addBody body tag =
        tag
        |> withBody (
            match tag.Body with
                | Some(b) -> b + body
                | None -> body
        )

    let toString tag =
        match tag with
        | { Name = n; Attribute = Some(a); Body = Some(b) } -> "<" + n + " " + a + ">" + b + "</" + n + ">"
        | { Name = n; Attribute = None; Body = Some(b) } -> "<" + n + ">" + b + "</" + n + ">"
        | { Name = n; Attribute = Some(a); Body = None } -> "<" + n + " " + a + "/>"
        | { Name = n; Attribute = None; Body = None } -> "<" + n + "/>"

// TODO: Move quote or do it better
    let inline quote i = "\"" + string i + "\""
