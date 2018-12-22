namespace SharpVG

type Element = {
        Id: string option
        Classes: seq<string>
        BaseTag: Tag
        Style: Style option
        Transform: Transform option
        Animations: seq<Animation>
    }

module Element =

    let inline create< ^T when ^T: (static member ToTag: ^T -> Tag)> taggable =
        {
            Id = None
            Classes = Seq.empty
            BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
            Style = None
            Transform = None
            Animations = Seq.empty
        }

    let withStyle style element =
        { element with Style = Some style }

    let withTransform transform element =
        { element with Transform = Some transform }

    let withAnimation animation element =
        { element with Animations = Seq.singleton animation }

    let withAnimations animations element =
        { element with Animations = animations }

    let addAnimation animation element =
        element.Animations |> Seq.append (Seq.singleton animation)

    let withId id element =
        { element with Id = Some id }

    let withClass className element =
        { element with Classes = Seq.singleton className }

    let withClasses classes element =
        { element with Classes = classes }

    let addClass className element =
        element.Classes |> Seq.append (Seq.singleton className)

    let toTag element =
        let styleClass =
            match element.Style with
                | Some s -> s.Name
                | None -> None

        let classes =
            match styleClass with
                | Some n -> element.Classes |> Seq.append (Seq.singleton n)
                | None -> element.Classes

        element.BaseTag
        |> Tag.insertAttributes
            (
                [
                    element.Id |> Option.map (Attribute.createCSS "id" >> List.singleton)
                    classes |> Seq.map (Attribute.createCSS "class") |> Seq.toList |> Option.Some
                    element.Style |> Option.filter (not << Style.isNamed) |> Option.map Style.toAttributes
                    element.Transform |> Option.map (Transform.toAttribute >> List.singleton)
                ]
                |> List.choose id
                |> List.concat
            )
        |> if element.Animations |> Seq.isEmpty then id else Tag.addBody (element.Animations |> Seq.map Animation.toString |> String.concat "")

    let toString element = element |> toTag |> Tag.toString

    let setTo timing newElement element =
        let attributesDiff = ((newElement |> toTag).Attributes |> Set.ofList) - ((element |> toTag).Attributes |> Set.ofList) |> Set.toList
        element |> withAnimations (attributesDiff |> List.map (fun {Name = n; Value = v; Type = t} -> Animation.createSet timing t n v))

