namespace SharpVG

type RawElement =
    {
        TagName: string
        Attributes: Attribute list
        Children: RawContent list
    }
and RawContent =
    | RawChild of RawElement
    | RawText of string

module RawElement =
    let create tagName attributes children =
        {
            TagName = tagName
            Attributes = attributes
            Children = children
        }

    let rec toString rawElement =
        let attrString =
            rawElement.Attributes
            |> List.map Attribute.toString
            |> function
               | [] -> ""
               | parts -> " " + (parts |> String.concat " ")
        let body =
            rawElement.Children
            |> List.map (function
                | RawChild child -> toString child
                | RawText text -> text)
            |> String.concat ""
        if body = "" then
            "<" + rawElement.TagName + attrString + "/>"
        else
            "<" + rawElement.TagName + attrString + ">" + body + "</" + rawElement.TagName + ">"
