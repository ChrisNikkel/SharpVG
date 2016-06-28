namespace SharpVG

type Viewbox = {
    Minimums: Point
    Size: Area
}

type Svg = {
    Body: Body
    Styles: seq<NamedStyle> option
    Size: Area option
    Viewbox: Viewbox option
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Svg =
    /// <summary>
    /// This function takes seqence of elements and creates a simple svg object.
    /// </summary>
    /// <param name="seq">The sequence of elements</param>
    /// <returns>The svg object containing the elements.</returns>
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e));
            Styles = None;
            Size = None;
            Viewbox = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let ofGroup group =
        {
            Body = seq { yield Group(group) }
            Styles = None;
            Size = None;
            Viewbox = None
        }

    let withSize size (svg:Svg) =
        { svg with Size = Some(size) }

    let withViewbox viewbox (svg:Svg) =
        { svg with Viewbox = Some(viewbox) }

    let withStyles styles (svg:Svg) =
        { svg with Styles = Some(styles) }

    let withStyle namedStyle (svg:Svg) =
        { svg with Styles = Some(Seq.singleton namedStyle) }


    let toString svg =
        let attributes =
            match svg.Size with | Some size -> Area.toAttributes size | None -> set []
            |> Set.union (match svg.Viewbox with | Some viewbox -> set [Attribute.createXML "viewBox" ((Point.toString viewbox.Minimums) + " " + (Area.toString viewbox.Size))] | None -> set [])

        let styles =
            match svg.Styles with
                | Some(styles) -> styles |> Styles.toString 
                | None -> ""

        let body =
            svg.Body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> Group.toString)
            |> String.concat ""

        Tag.create "svg"
        |> Tag.withAttributes attributes
        |> Tag.withBody (styles + body)
        |> Tag.toString

    let toHtml title svg =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + title + "</title>\n</head>\n<body>\n" + (toString svg) + "</body>\n</html>\n"
