namespace SharpVG

type Anchor =
    {
        Url: string
        Elements: seq<Element>
    }
with
    static member ToTag anchor =
        Tag.create "a"
        |> Tag.withAttributes [ Attribute.createXML "href" anchor.Url ]

    override this.ToString() =
       this |> Anchor.ToTag |> Tag.toString

module Anchor =
    let create url =
        { Url = url; Elements = Seq.empty }

    let withBody elements (anchor : Anchor) =
        { anchor with Elements = elements }

    let addElements elements anchor =
        anchor |> withBody (Seq.append anchor.Elements elements)

    let addElement element anchor =
        addElements (Seq.singleton element) anchor
    let toTag =
        Anchor.ToTag

    let toString (anchor : Anchor) =
        anchor.ToString()