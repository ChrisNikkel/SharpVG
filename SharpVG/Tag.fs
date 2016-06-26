namespace SharpVG

type Tag =
    {
        Name: string;
        Attributes: Set<Attribute>;
        Body: string option;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Tag =

    let create name =
        { Name = name; Attributes = Set.empty; Body = None }

    let withAttribute attribute tag =
        { tag with Attributes = set [attribute] }

    let withAttributes attributes tag =
        { tag with Attributes = attributes }

    let withBody body tag =
        { tag with Body = Some(body) }

    let addAttribute attribute tag =
        tag 
        |> withAttributes (Set.add attribute tag.Attributes)

    let addAttributes attributes tag =
        tag
        |> withAttributes (attributes + tag.Attributes)

    let addBody body tag =
        tag
        |> withBody (
            match tag.Body with
                | Some(b) -> b + body
                | None -> body
        )

    let toString tag =
        let attributesToString attributes = (attributes |> Set.map Attribute.toString |> Set.toSeq |> String.concat " ")
        match tag with
        | { Name = n; Attributes = a; Body = Some(b) } when a <> Set.empty -> "<" + n + " " + (attributesToString a) + ">" + b + "</" + n + ">"
        | { Name = n; Attributes = a; Body = Some(b) } when a = Set.empty -> "<" + n + ">" + b + "</" + n + ">"
        | { Name = n; Attributes = a; Body = None } when a <> Set.empty  -> "<" + n + " " + (attributesToString a) + "/>"
        | { Name = n; Attributes = a; Body = None } when a = Set.empty  -> "<" + n + "/>"
        | _ -> failwith "Unmatched tag"
