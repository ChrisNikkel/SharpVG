namespace SharpVG

type tag =
    {
        name: string;
        attribute: string option;
        body: string option;
    }

module Tag =
    let toString tag =
        match tag with
        | { name = n; attribute = Some(a); body = Some(b) } -> "<" + n + " " + a + ">" + b + "</" + n + ">"
        | { name = n; attribute = None; body = Some(b) } -> "<" + n + ">" + b + "</" + n + ">"
        | { name = n; attribute = Some(a); body = None } -> "<" + n + " " + a + "/>"
        | { name = n; attribute = None; body = None } -> "<" + n + "/>"

// TODO: Move out or do it better
    let inline quote i = "\"" + string i + "\""

    let addSpace needsSpace =
        (if needsSpace then " " else "")

    let addSpaces strings = (strings |> Seq.reduce (fun acc str -> acc + " " + str)).TrimStart()
