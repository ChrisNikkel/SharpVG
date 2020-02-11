namespace SharpVG

type Svg = {
    Body: Body
    Size: Area option
    ViewBox: ViewBox option
}
with
    override this.ToString() =
        let attributes =
            match this.Size with | Some size -> Area.toAttributes size | None -> []
            @ match this.ViewBox with | Some viewBox -> ViewBox.toAttributes viewBox | None -> []

        let styles =
          let namedStyles = this.Body |> Body.toStyles |> Styles.named
          if Seq.isEmpty namedStyles then
            ""
          else
            Styles.toString namedStyles

        let body =
            if Seq.isEmpty this.Body then
              ""
            else
              this.Body
              |> Seq.map (function | Element(e) -> e |> Element.toString | Group(g) -> g |> Group.toString)
              |> String.concat ""

        Tag.create "svg"
        |> Tag.withAttributes attributes
        |> Tag.withBody (styles + body)
        |> Tag.toString

module Svg =
    let withSize size (svg:Svg) =
        { svg with Size = Some(size) }

    let withViewBox viewBox (svg:Svg) =
        { svg with ViewBox = Some(viewBox) }

    /// <summary>
    /// This function takes seqence of elements and creates a simple svg object.
    /// </summary>
    /// <param name="seq">The sequence of elements</param>
    /// <returns>The svg object containing the elements.</returns>
    let ofSeq seq =
        {
            Body = seq |> Seq.map (fun e -> Element(e))
            Size = None
            ViewBox = None
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
            ViewBox = None
        }

    let ofPlot plot =
        plot
        |> Plot.toGroup
        |> ofGroup
        |> withViewBox (ViewBox.create Point.origin (Area.fromPoints (Point.ofFloats plot.Minimum) (Point.ofFloats plot.Maximum)))

    let toString (svg : Svg) =
        svg.ToString()

    let toHtml title svg =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + title + "</title>\n</head>\n<body>\n" + (toString svg) + "\n</body>\n</html>\n"
