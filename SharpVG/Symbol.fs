namespace SharpVG

type Symbol =
    {
        ViewBox : ViewBox
        Size : Area option
        Position : Point option
        Reference : Point option
        PreserveAspectRatio: PreserveAspectRatio option
        Elements : seq<Element>
    }
with
    static member ToTag symbol =
        Tag.create "symbol"
        |> Tag.withAttributes (ViewBox.toAttributes symbol.ViewBox)
        |> (match symbol.Size with Some s -> Tag.addAttributes (Area.toAttributes s) | None -> id)
        |> (match symbol.Position with Some p -> Tag.addAttributes (Point.toAttributes p) | None -> id)
        |> (match symbol.Reference with Some r -> Tag.addAttributes (Point.toAttributesWithModifier "r" "" r) | None -> id)
        |> (match symbol.PreserveAspectRatio with Some par -> Tag.addAttributes [PreserveAspectRatio.toAttribute par] | None -> id)
        |> Tag.withBody (symbol.Elements |> Seq.map Element.toString |> String.concat "")

    override this.ToString() =
        this |> Symbol.ToTag |> Tag.toString

module Symbol =

    let create viewBox =
        { ViewBox = viewBox; Size = None; Reference = None; Position = None; PreserveAspectRatio = None; Elements = Seq.empty }

    let withSize size (symbol : Symbol) =
        { symbol with Size = size }

    let withPosition position (symbol : Symbol) =
        { symbol with Position = position }

    let withReference reference (symbol : Symbol) =
        { symbol with Reference = reference }

    let withPreserveAspectRatio par (symbol: Symbol) =
        { symbol with PreserveAspectRatio = Some par }

    let withBody elements (symbol : Symbol) =
        { symbol with Elements = elements }

    let addElements elements symbol =
        symbol |> withBody (Seq.append symbol.Elements elements)

    let addElement element symbol =
        addElements (Seq.singleton element) symbol

    let toTag =
        Symbol.ToTag

    let toString (symbol : Symbol) =
        symbol.ToString()