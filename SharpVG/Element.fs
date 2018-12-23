namespace SharpVG

type ElementBody = {
        Classes: seq<string>
        BaseTag: Tag
        Style: Style option
        Transform: Transform option
        Animations: seq<Animation>
    }

type ElementIdentity = Unnamed | Named of string

type Element =
    {
        Identity : ElementIdentity
        Body : ElementBody
    }
with
    static member ToTag element =
        let body = element.Body
        let styleClass =
            match body.Style with
                | Some s -> s.Name
                | None -> None

        let classes =
            match styleClass with
                | Some n -> body.Classes |> Seq.append (Seq.singleton n)
                | None -> body.Classes

        body.BaseTag
        |> Tag.insertAttributes (match element.Identity with Named name -> [ Attribute.createCSS "id" name ] | Unnamed -> [])
        |> Tag.insertAttributes
            (
                [
                    classes |> Seq.map (Attribute.createCSS "class") |> Seq.toList |> Option.Some
                    body.Style |> Option.filter (not << Style.isNamed) |> Option.map Style.toAttributes
                    body.Transform |> Option.map (Transform.toAttribute >> List.singleton)
                ]
                |> List.choose id
                |> List.concat
            )
        |> if body.Animations |> Seq.isEmpty then id else Tag.addBody (body.Animations |> Seq.map Animation.toString |> String.concat "")

    override this.ToString() =
        this |> Element.ToTag |> Tag.toString

module Element =

    let inline create< ^T when ^T: (static member ToTag: ^T -> Tag)> taggable =
        {
            Identity = Unnamed
            Body =
                {
                    Classes = Seq.empty
                    BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
                    Style = None
                    Transform = None
                    Animations = Seq.empty
                }
        }

    let inline createWithStyle< ^T when ^T: (static member ToTag: ^T -> Tag)> style taggable =
        {
            Identity = Unnamed
            Body =
                {
                    Classes = Seq.empty
                    BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
                    Style = Some(style)
                    Transform = None
                    Animations = Seq.empty
                }
        }

    let inline createWithName< ^T when ^T: (static member ToTag: ^T -> Tag)> name taggable =
        {
            Identity = Named name
            Body =
                {
                    Classes = Seq.empty
                    BaseTag = (^T : (static member ToTag: ^T -> Tag) (taggable))
                    Style = None
                    Transform = None
                    Animations = Seq.empty
                }
        }

    let withStyle style (element : Element) =
        { element with Body = { element.Body with Style = Some style } }

    let withTransform transform (element : Element) =
        { element with Body = { element.Body with Transform = Some transform } }

    let withAnimation animation (element : Element) =
        { element with Body = { element.Body with Animations = Seq.singleton animation } }

    let withAnimations animations (element : Element) =
        { element with Body = { element.Body with Animations = animations } }

    let addAnimation animation (element : Element) =
        { element with Body = { element.Body with Animations = element.Body.Animations |> Seq.append (Seq.singleton animation) } }

    let withName name (element : Element) =
        { element with Identity = Named name }

    let withClass className (element : Element) =
        { element with Body = { element.Body with Classes = Seq.singleton className } }

    let withClasses classes (element : Element) =
        { element with Body = { element.Body with Classes = classes } }

    let addClass className (element : Element) =
        { element with Body = { element.Body with Classes = element.Body.Classes |> Seq.append (Seq.singleton className) } }

    let toTag =
        Element.ToTag

    let toString (element : Element) =
        element.ToString()

    let setTo timing newElement element =
        let attributesDiff = ((newElement |> toTag).Attributes |> Set.ofList) - ((element |> toTag).Attributes |> Set.ofList) |> Set.toList
        element |> withAnimations (attributesDiff |> List.map (fun {Name = n; Value = v; Type = t} -> Animation.createSet timing t n v))

