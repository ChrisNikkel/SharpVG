namespace SharpVG

type Symbol =
    {
        ViewBox : ViewBox
        Size : Area option
        Position : Point option
        Reference : Point option
        Elements : seq<Element>
    }
with
    static member ToTag symbol =
        Tag.create "symbol"
        |> Tag.withAttributes (ViewBox.toAttributes symbol.ViewBox)
        |> Tag.addAttributes (match symbol.Size with Some(s) -> Area.toAttributes s | _ -> [])
        |> Tag.addAttributes (match symbol.Position with Some(p) -> Point.toAttributes p | _ -> [])
        |> Tag.addAttributes (match symbol.Reference with Some(r) -> Point.toAttributesWithModifier "r" "" r | _ -> [])
        |> Tag.withBody (symbol.Elements |> Seq.map Element.toString |> String.concat "")

    override this.ToString() =
       this |> Symbol.ToTag |> Tag.toString

module Symbol =

    let create viewBox =
        { ViewBox = viewBox; Size = None; Reference = None; Position = None; Elements = Seq.empty }

    let withSize size (symbol : Symbol) =
        { symbol with Size = size }

    let withPosition position (symbol : Symbol) =
        { symbol with Position = position }

    let withReference reference (symbol : Symbol) =
        { symbol with Reference = reference }

    let withBody elements symbol =
        { symbol with Elements = elements }

    let addElements elements symbol =
        symbol |> withBody (Seq.append symbol.Elements elements)

    let addElement element symbol =
        addElements (Seq.singleton element) symbol

    let toTag =
        Symbol.ToTag

    let toString (symbol : Symbol) =
        symbol.ToString()