namespace SharpVG

type Element = {
        Name: ElementId option
        Classes: seq<string>
        BaseTag: Tag
        Style: Style option
        Href: string option
        Transforms: seq<Transform>
        Animations: seq<Animation>
    }
with
    static member ToTag element =
        element.BaseTag

    static member ToFullTag element =
        let styleClass =
            match element.Style with
                | Some s -> s.Name
                | None -> None

        let classes =
            match styleClass with
                | Some n -> element.Classes |> Seq.append (Seq.singleton n)
                | None -> element.Classes

        let attributes =
            [
                    element.Name |> Option.map (Attribute.createCSS "id" >> List.singleton)
                    classes |> Seq.map (Attribute.createCSS "class") |> Seq.toList |> Option.Some
                    element.Style |> Option.filter (not << Style.isNamed) |> Option.map Style.toAttributes
                    element.Href |> Option.map (Attribute.createCSS "href" >> List.singleton)
                    if Seq.isEmpty element.Transforms then None else Some [ element.Transforms |> Transforms.toAttribute ]
            ]
            |> List.choose id
            |> List.concat

        element.BaseTag
        |> Tag.insertAttributes attributes
        |> if element.Animations |> Seq.isEmpty then id else Tag.addBody (element.Animations |> Seq.map Animation.toString |> String.concat "")

    override this.ToString() =
        this |> Element.ToFullTag |> Tag.toString

module Element =

    // TODO: Move away from static member to just member
    let inline create< ^T when ^T: (static member ToTag: ^T -> Tag)> taggable =
        {
            Name = None
            Classes = Seq.empty
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Style = None
            Href = None
            Transforms = Seq.empty
            Animations = Seq.empty
        }

    let inline createWithStyle< ^T when ^T: (static member ToTag: ^T -> Tag)> style taggable =
        {
            Name = None
            Classes = Seq.empty
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Style = Some(style)
            Href = None
            Transforms = Seq.empty
            Animations = Seq.empty
        }

    let inline createWithClass< ^T when ^T: (static member ToTag: ^T -> Tag)> theClass taggable =
        {
            Name = None
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Classes = Seq.singleton theClass
            Style = None
            Href = None
            Transforms = Seq.empty
            Animations = Seq.empty
        }

    let inline createWithClasses< ^T when ^T: (static member ToTag: ^T -> Tag)> classes taggable =
        {
            Name = None
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Classes = classes
            Style = None
            Href = None
            Transforms = Seq.empty
            Animations = Seq.empty
        }

    let inline createWithName< ^T when ^T: (static member ToTag: ^T -> Tag)> name taggable =
        {
            Name = Option.ofObj name
            Classes = Seq.empty
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Style = None
            Href = None
            Transforms = Seq.empty
            Animations = Seq.empty
        }

    let inline createFull< ^T when ^T: (static member ToTag: ^T -> Tag)> name classes style transform animations taggable =
        {
            Name = name
            Classes = classes
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Style = style
            Href = None
            Transforms = transform
            Animations = animations
        }

    let withStyle style element =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transforms = Seq.singleton transform }

    let withTransforms transforms element =
        { element with Transforms = transforms }

    let withAnimation animation element =
        { element with Animations = Seq.singleton animation }

    let withAnimations animations element =
        { element with Animations = animations }

    let addAnimation animation element =
        element.Animations |> Seq.append (Seq.singleton animation)

    let withName name (element : Element) =
        { element with Name = Some name }

    let withHref href (element : Element) =
        { element with Href = Some href }

    let withClass className element =
        { element with Classes = Seq.singleton className }

    let withClasses classes element =
        { element with Classes = classes }

    let addClass className element =
        { element with Classes = (element.Classes |> Seq.append (Seq.singleton className)) }

    let tryGetName element =
        element.Name

    let isNamed (element : Element) =
        element.Name.IsSome

    let toTag =
        Element.ToTag

    let toString (element : Element) =
        element.ToString()

    let setTo timing newElement element =
        let attributesDiff = ((newElement |> toTag).Attributes |> Set.ofList) - ((element |> toTag).Attributes |> Set.ofList) |> Set.toList
        element |> withAnimations (attributesDiff |> List.map (fun {Name = n; Value = v; Type = t} -> Animation.createSet timing t n v))

