namespace SharpVG

type Use =
    {
        Name : string
        Position : Point
        Size : Area option
    }
with
    static member ToTag useObj =
        Tag.create "use"
        |> Tag.withAttributes (Point.toAttributes useObj.Position)
        |> Tag.addAttributes (match useObj.Size with Some(s) -> Area.toAttributes s | _ -> [])
    override this.ToString() =
       this |> Use.ToTag |> Tag.toString

module Use =

    let create element position : Use =
        match element with
            | Named name -> { Name = name; Position = position; Size = None }
            | Unnamed -> failwith "Can not use an element without a name"

    let toTag =
        Use.ToTag

    let toString (useObj : Use) =
        useObj.ToString()