namespace SharpVG

type MarkerUnits =
    | StrokeWidthUnits
    | UserSpaceOnUseUnits
with
    override this.ToString() =
        match this with
        | StrokeWidthUnits -> "strokeWidth"
        | UserSpaceOnUseUnits -> "userSpaceOnUse"

type MarkerOrient =
    | AutoOrient
    | AngleOrient of float
with
    override this.ToString() =
        match this with
        | AutoOrient -> "auto"
        | AngleOrient a -> string a

type Marker =
    {
        Id: ElementId
        ViewBox: ViewBox option
        RefPoint: Point option
        Size: Area option
        Units: MarkerUnits option
        Orient: MarkerOrient option
        Body: Body
    }
with
    static member ToTag marker =
        let body = Body.toString marker.Body
        Tag.create "marker"
        |> Tag.withAttributes
            ([
                [ Attribute.createXML "id" marker.Id ]
                marker.ViewBox |> Option.map ViewBox.toAttributes |> Option.defaultValue []
                marker.RefPoint |> Option.map (fun p ->
                    [ Attribute.createXML "refX" (Length.toString p.X)
                      Attribute.createXML "refY" (Length.toString p.Y) ]) |> Option.defaultValue []
                marker.Size |> Option.map Area.toAttributes |> Option.defaultValue []
                marker.Units |> Option.map (fun u -> [ Attribute.createXML "markerUnits" (u.ToString()) ]) |> Option.defaultValue []
                marker.Orient |> Option.map (fun o -> [ Attribute.createXML "orient" (o.ToString()) ]) |> Option.defaultValue []
            ] |> List.concat)
        |> Tag.withBody body

    override this.ToString() =
        this |> Marker.ToTag |> Tag.toString

module Marker =
    let create id =
        { Id = id; ViewBox = None; RefPoint = None; Size = None; Units = None; Orient = None; Body = Seq.empty }

    let withViewBox viewBox (marker: Marker) =
        { marker with ViewBox = Some viewBox }

    let withRefPoint refPoint (marker: Marker) =
        { marker with RefPoint = Some refPoint }

    let withSize size (marker: Marker) =
        { marker with Size = Some size }

    let withUnits units (marker: Marker) =
        { marker with Units = Some units }

    let withOrient orient (marker: Marker) =
        { marker with Orient = Some orient }

    let addElement element (marker: Marker) =
        { marker with Body = Seq.append marker.Body (Seq.singleton (Element element)) }

    let addElements elements (marker: Marker) =
        { marker with Body = Seq.append marker.Body (elements |> Seq.map Element) }

    let toTag = Marker.ToTag

    let toString (marker: Marker) = marker.ToString()
