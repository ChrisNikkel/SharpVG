namespace SharpVG

type Tag =
    {
        Name: string;
        Attributes: List<Attribute>;
        Body: string option;
    }

module Tag =

    let create name =
        { Name = name; Attributes = List.empty; Body = None }

    let withAttribute attribute tag =
        { tag with Attributes = List.singleton attribute }

    let withAttributes attributes tag =
        { tag with Attributes = attributes }

    let withBody body tag =
        { tag with Body = Some(body) }

    let addAttributes attributes tag =
        tag
        |> withAttributes (tag.Attributes @ attributes)

    let addAttribute attribute tag =
        tag
        |> addAttributes (List.singleton attribute)

    let insertAttributes attributes tag =
        tag
        |> withAttributes (attributes @ tag.Attributes)

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

    let toString tag =
        let attributesToString attributes = (attributes |> List.map Attribute.toString |> String.concat " ")
        match (List.isEmpty tag.Attributes), tag with
        | false, { Name = n; Attributes = a; Body = Some(b) } -> "<" + n + " " + (attributesToString a) + ">" + b + "</" + n + ">"
        | false, { Name = n; Attributes = a; Body = None } -> "<" + n + " " + (attributesToString a) + "/>"
        | true, { Name = n; Attributes = a; Body = Some(b) } -> "<" + n + ">" + b + "</" + n + ">"
        | true, { Name = n; Attributes = a; Body = None } -> "<" + n + "/>"
