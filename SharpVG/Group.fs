namespace SharpVG

type group = {
    body: body
    upperLeft: point
    transform: transform option
}
and groupElement =
    | Group of group
    | Element of element
and body =
    seq<groupElement>

module Group =
    let rec toString group =
        let body =
            group.body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> toString)
            |> String.concat ""
        let transform =
            match group.transform with Some(transform) -> (Transform.toString transform) + " " | None -> ""
        let upperLeft =
             Point.toString group.upperLeft
        "<g " + upperLeft + transform + ">" + body + "</g>"