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

    let toHtml title svg =
        "<!DOCTYPE html>\n<html>\n<head>\n<title>" + escapeHtmlTitle title + "</title>\n</head>\n<body>\n" + (toString svg) + "\n</body>\n</html>\n"
