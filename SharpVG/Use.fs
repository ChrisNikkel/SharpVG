namespace SharpVG

type Use =
    {
        Name : ElementId
        Position : Point
        Size : Area option
    }
with
    static member ToTag useObj =
        Tag.create "use"
        |> Tag.withAttribute (Attribute.createXML "href" ("#" + useObj.Name))
        |> Tag.addAttributes (Point.toAttributes useObj.Position)
        |> Tag.addAttributes (match useObj.Size with Some(s) -> Area.toAttributes s | _ -> [])


    override this.ToString() =
        this |> Use.ToTag |> Tag.toString

module Use =

    let create element position : Use =
        match Element.tryGetName element with
            | Some name -> { Name = name; Position = position; Size = None }
            | None -> failwith "Can not use an element without a name"

    let tryCreate element position : Use option =
        Element.tryGetName element |> Option.map (fun name -> { Name = name; Position = position; Size = None })

    let toTag =
        Use.ToTag

    let toString (useObj : Use) =
        useObj.ToString()