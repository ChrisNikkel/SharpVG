namespace SharpVG

type Viewbox = {
    Minimum: Point
    Size: Area
}

type Svg = {
    Body: Body
    Size: Area option
    Viewbox: Viewbox option
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Svg =
    let withSize size (svg:Svg) =
        { svg with Size = Some(size) }

    let withViewbox minimum size (svg:Svg) =
        { svg with Viewbox = Some({ Minimum = minimum; Size = size }) }

    /// <summary>
    /// This function takes seqence of elements and creates a simple svg object.
    /// </summary>
    /// <param name="seq">The sequence of elements</param>
    /// <returns>The svg object containing the elements.</returns>
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e))
            Size = None
            Viewbox = None
        }

    let ofList list =
        list |> Seq.ofList |> ofSeq

    let ofElement element =
        element |> Seq.singleton |> ofSeq

    let ofArray array =
        array |> Seq.ofArray |> ofSeq

    let ofGroup group =
        {
            Body = seq { yield Group(group) }
            Size = Some(Area.full);
            Viewbox = None
        }

    let ofPlot plot =
        plot
        |> Plot.toGroup
        |> ofGroup
        |> withViewbox Point.origin (Area.fromPoints (Point.ofFloats plot.Minimum) (Point.ofFloats plot.Maximum))

    let toString svg =
        let attributes =
            match svg.Size with | Some size -> Area.toAttributes size | None -> []
            @ match svg.Viewbox with | Some viewbox -> [Attribute.createXML "viewBox" ((Point.toString viewbox.Minimum) + " " + (Area.toString viewbox.Size))] | None -> []

        let styles =
          let namedStyles = svg.Body |> Body.toStyles |> Styles.named
          if Seq.isEmpty namedStyles then
            ""
          else
            Styles.toString namedStyles

        let body =
            svg.Body
            |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> Group.toString)
            |> String.concat ""

        Tag.create "svg"
        |> Tag.withAttributes attributes
        |> Tag.withBody (styles + body)
        |> Tag.toString

    let toHtml title svg =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + title + "</title>\n</head>\n<body>\n" + (toString svg) + "\n</body>\n</html>\n"
