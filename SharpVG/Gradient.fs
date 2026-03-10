namespace SharpVG

type SpreadMethod =
    | Pad
    | Reflect
    | Repeat
with
    override this.ToString() =
        match this with
        | Pad -> "pad"
        | Reflect -> "reflect"
        | Repeat -> "repeat"

type GradientStop =
    {
        Offset: float
        Color: Color
        Opacity: float option
    }
with
    static member ToTag stop =
        Tag.create "stop"
        |> Tag.withAttributes
            [
                Attribute.createXML "offset" (string stop.Offset)
                Attribute.createXML "stop-color" (Color.toString stop.Color)
            ]
        |> (match stop.Opacity with
            | Some o -> Tag.addAttributes [ Attribute.createXML "stop-opacity" (string o) ]
            | None -> id)

    override this.ToString() =
        this |> GradientStop.ToTag |> Tag.toString

type LinearGradient =
    {
        Id: ElementId
        Point1: Point
        Point2: Point
        GradientUnits: FilterUnits option
        SpreadMethod: SpreadMethod option
        GradientTransform: Transform option
        Href: string option
        Stops: GradientStop list
    }
with
    static member ToTag gradient =
        let body = gradient.Stops |> List.map (fun s -> s.ToString()) |> String.concat ""
        Tag.create "linearGradient"
        |> Tag.withAttributes
            ([
                [ Attribute.createXML "id" gradient.Id ]
                Point.toAttributesWithModifier "" "1" gradient.Point1
                Point.toAttributesWithModifier "" "2" gradient.Point2
                gradient.GradientUnits |> Option.map (fun u -> [ Attribute.createXML "gradientUnits" (u.ToString()) ]) |> Option.defaultValue []
                gradient.SpreadMethod |> Option.map (fun sm -> [ Attribute.createXML "spreadMethod" (sm.ToString()) ]) |> Option.defaultValue []
                gradient.GradientTransform |> Option.map (fun t -> [ Attribute.createXML "gradientTransform" (Transform.toString t) ]) |> Option.defaultValue []
                gradient.Href |> Option.map (fun h -> [ Attribute.createXML "href" ("#" + h) ]) |> Option.defaultValue []
            ] |> List.concat)
        |> Tag.withBody body

    override this.ToString() =
        this |> LinearGradient.ToTag |> Tag.toString

type RadialGradient =
    {
        Id: ElementId
        Center: Point
        Focal: Point option
        Radius: Length
        GradientUnits: FilterUnits option
        SpreadMethod: SpreadMethod option
        GradientTransform: Transform option
        Href: string option
        Stops: GradientStop list
    }
with
    static member ToTag gradient =
        let body = gradient.Stops |> List.map (fun s -> s.ToString()) |> String.concat ""
        Tag.create "radialGradient"
        |> Tag.withAttributes
            ([
                [ Attribute.createXML "id" gradient.Id ]
                Point.toAttributesWithModifier "c" "" gradient.Center
                gradient.Focal |> Option.map (Point.toAttributesWithModifier "f" "") |> Option.defaultValue []
                [ Attribute.createXML "r" (Length.toString gradient.Radius) ]
                gradient.GradientUnits |> Option.map (fun u -> [ Attribute.createXML "gradientUnits" (u.ToString()) ]) |> Option.defaultValue []
                gradient.SpreadMethod |> Option.map (fun sm -> [ Attribute.createXML "spreadMethod" (sm.ToString()) ]) |> Option.defaultValue []
                gradient.GradientTransform |> Option.map (fun t -> [ Attribute.createXML "gradientTransform" (Transform.toString t) ]) |> Option.defaultValue []
                gradient.Href |> Option.map (fun h -> [ Attribute.createXML "href" ("#" + h) ]) |> Option.defaultValue []
            ] |> List.concat)
        |> Tag.withBody body

    override this.ToString() =
        this |> RadialGradient.ToTag |> Tag.toString

type Gradient =
    | Linear of LinearGradient
    | Radial of RadialGradient
with
    override this.ToString() =
        match this with
        | Linear lg -> lg.ToString()
        | Radial rg -> rg.ToString()

module GradientStop =
    let create offset color =
        { Offset = offset; Color = color; Opacity = None }

    let createWithOpacity offset color opacity =
        { Offset = offset; Color = color; Opacity = Some opacity }

    let withOpacity opacity (stop: GradientStop) =
        { stop with Opacity = Some opacity }

    let toTag = GradientStop.ToTag

    let toString (stop: GradientStop) = stop.ToString()

module LinearGradient =
    let create id point1 point2 stops =
        { Id = id; Point1 = point1; Point2 = point2; GradientUnits = None; SpreadMethod = None; GradientTransform = None; Href = None; Stops = stops }

    let withGradientUnits units (gradient: LinearGradient) =
        { gradient with GradientUnits = Some units }

    let withSpreadMethod method (gradient: LinearGradient) =
        { gradient with SpreadMethod = Some method }

    let withGradientTransform transform (gradient: LinearGradient) =
        { gradient with GradientTransform = Some transform }

    let withHref id (gradient: LinearGradient) =
        { gradient with Href = Some id }

    let toTag = LinearGradient.ToTag

    let toString (gradient: LinearGradient) = gradient.ToString()

module RadialGradient =
    let create id center radius stops =
        { Id = id; Center = center; Focal = None; Radius = radius; GradientUnits = None; SpreadMethod = None; GradientTransform = None; Href = None; Stops = stops }

    let withFocal focal (gradient: RadialGradient) =
        { gradient with Focal = Some focal }

    let withGradientUnits units (gradient: RadialGradient) =
        { gradient with GradientUnits = Some units }

    let withSpreadMethod method (gradient: RadialGradient) =
        { gradient with SpreadMethod = Some method }

    let withGradientTransform transform (gradient: RadialGradient) =
        { gradient with GradientTransform = Some transform }

    let withHref id (gradient: RadialGradient) =
        { gradient with Href = Some id }

    let toTag = RadialGradient.ToTag

    let toString (gradient: RadialGradient) = gradient.ToString()

module Gradient =
    let ofLinear = Linear
    let ofRadial = Radial

    let toString (gradient: Gradient) = gradient.ToString()
