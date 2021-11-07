namespace SharpVG
open System

type BlendMode =
    | Normal
    | Multiply
    | Screen
    | Overlay
    | Darken
    | Lighten
    | ColorDodge
    | ColorBurn
    | HardLight
    | SoftLight
    | Difference
    | Exclusion
    | Hue
    | Saturation
    | Color
    | Luminosity
    override this.ToString() =
        match this with
            | Normal -> "Normal"
            | Multiply -> "Multiply"
            | Screen -> "Screen"
            | Overlay -> "Overlay"
            | Darken -> "Darken"
            | Lighten -> "Lighten"
            | ColorDodge -> "ColorDodge"
            | ColorBurn -> "ColorBurn"
            | HardLight -> "HardLight"
            | SoftLight -> "SoftLight"
            | Difference -> "Difference"
            | Exclusion -> "Exclusion"
            | Hue -> "Hue"
            | Saturation -> "Saturation"
            | Color -> "Color"
            | Luminosity -> "Luminosity"

type EdgeMode =
    | Duplicate
    | Wrap
    | NoEdge
    override this.ToString() =
        match this with
            | Duplicate -> "Duplicate"
            | Wrap -> "Wrap"
            | NoEdge -> "NoEdge"

type ColorMatrix =
    | Matrix of int list // TODO: Convert top a 5x4 matrix object of some sort
    | Saturate of float // TODO: Constrain to 0 to 1.0 or make type for percent
    | HueRotate of float // TODO: Make type for degrees
    | LuminanceToAlpha
    with
        static member ToTag colorMatrix =
            Tag.create "feColorMatrix" |>
            match colorMatrix with
                | Matrix(matrix) -> Tag.withAttributes [(Attribute.createXML "type" "matrix"); Attribute.createXML "values" (matrix |> List.map string |> String.concat " ")]
                | Saturate(saturate) -> Tag.withAttributes [(Attribute.createXML "type" "saturate"); Attribute.createXML "values" (string saturate)]
                | HueRotate(hueRotate) -> Tag.withAttributes [(Attribute.createXML "type" "hueRotate"); Attribute.createXML "values" (string hueRotate)]
                | LuminanceToAlpha -> id

        override this.ToString() =
            this |> ColorMatrix.ToTag |> Tag.toString

type Composite =
    | Over
    | In
    | Out
    | Atop
    | Xor
    | Lighter
    | Arithmetic
    override this.ToString() =
        match this with
            | Over -> "Over"
            | In -> "In"
            | Out -> "Out"
            | Atop -> "Atop"
            | Xor -> "Xor"
            | Lighter -> "Lighter"
            | Arithmetic -> "Arithmetic"
            
type DiffuseLighting =
    {
        SurfaceScale : float
        DiffuseConstant : float
        KernelUnitLength : float // TODO: Make optional
    }
    with
        static member ToTag diffuseLighting =
            let attributes =
                [
                    Some(Attribute.createXML "surfaceScale" (string diffuseLighting.SurfaceScale))
                    Some(Attribute.createXML "diffuseConstant" (string diffuseLighting.DiffuseConstant))
                    Some(Attribute.createXML "kernelUnitLength" (string diffuseLighting.KernelUnitLength))
                ] |> List.choose id

            Tag.createWithAttributes "feDiffuseLighting" attributes
type Flood =
    {
        Color : Color
        Opacity : float option// TODO: Make optional and onstrain to 0 to 1.0 or make type for percent
    }
    with
        static member ToTag flood =
            let attributes =
                [
                    Some(Attribute.createXML "flood-color" (flood.Color.ToString()))
                    flood.Opacity |> Option.map (string >> (Attribute.createXML "flood-opacity"))
                ] |> List.choose id

            Tag.createWithAttributes "feFlood" attributes

type GaussianBlur =
    {
        StandardDeviation : float
        EdgeMode : EdgeMode option
    }
    with
        static member ToTag gaussianBlur=
            let attributes =
                [
                    Some(Attribute.createXML "stdDeviation" (string gaussianBlur.StandardDeviation))
                    gaussianBlur.EdgeMode |> Option.map (fun edgeMode -> Attribute.createXML "edgeMode" (edgeMode.ToString())) 
                ] |> List.choose id

            Tag.createWithAttributes "feGaussianBlur" attributes

type FilterEffectType =
    | Blend of BlendMode
    | ColorMatrix of ColorMatrix
    // TODO: Implement ComponentTransfer
    | Composite of Composite
    // TODO: Implement ConvolveMatrix
    | DiffuseLighting of DiffuseLighting
    // TODO: Implement DisplacementMap
    | Flood of Flood
    | GaussianBlur of GaussianBlur
    | Image of string  // TODO: Create type to use here that can reference an ElementId or contain an Href
    // TODO: Implement Merge
    // TODO: Implement MergeNode
    // TODO: Implement Morphology
    | Offset of Point
    // TODO: Implement SpecularLighting
    // TODO: Implement Tile
    // TODO: Implement Turbulence


type FilterEffect =
    {
        Type : FilterEffectType
        Input : string
        Input2 : string
        Result : string
    }
with
    static member ToTag filterEffect =
        let attributes =
            [
                if String.IsNullOrEmpty(filterEffect.Input) then None else Some(Attribute.createXML "in" filterEffect.Input)
                if String.IsNullOrEmpty(filterEffect.Input2) then None else Some(Attribute.createXML "in2" filterEffect.Input2)
                if String.IsNullOrEmpty(filterEffect.Result) then None else Some(Attribute.createXML "result" filterEffect.Result)
            ] |> List.choose id

        match filterEffect.Type with
            | Blend (blend) -> Tag.create "feBlend" |> Tag.withAttribute (Attribute.createXML "mode" (blend.ToString()))
            | ColorMatrix (colorMatrix) -> ColorMatrix.ToTag colorMatrix
            | Composite (composite) -> Tag.create "feComposite" |> Tag.withAttribute (Attribute.createXML "operator" (composite.ToString()))
            | DiffuseLighting (diffuseLighting) -> DiffuseLighting.ToTag diffuseLighting
            | Flood (flood) -> Flood.ToTag flood
            | GaussianBlur (gaussianBlur) -> GaussianBlur.ToTag gaussianBlur
            | Image (image) -> Tag.createWithAttribute "feImage" (Attribute.createXML "xlink:href" image)
            | Offset (offset) -> Tag.createWithAttributes "feOffset" (Point.toAttributesWithModifier "d" "" offset)

        |> Tag.addAttributes attributes

    override this.ToString() =
        this |> FilterEffect.ToTag |> Tag.toString


// TODO: make it easy to string things together so that if result or inputs aren't specified random ids are created and linked together.  

module FilterEffect =

    let createBlend blend = 
        { Type = Blend blend; Input = ""; Input2 = ""; Result = "" }

    let createColorMatrix colorMatrix = 
        { Type = ColorMatrix colorMatrix; Input = ""; Input2 = ""; Result = "" }

    let createComposite composite = 
        { Type = Composite composite ; Input = ""; Input2 = ""; Result = "" }

    let createDiffuseLighting surfaceScale diffuseConstant kernelUnitLength = 
        { Type = DiffuseLighting { SurfaceScale = surfaceScale; DiffuseConstant = diffuseConstant; KernelUnitLength = kernelUnitLength }; Input = ""; Input2 = ""; Result = "" }

    let createFlood color opacity = 
        { Type = Flood { Color = color; Opacity = opacity }; Input = ""; Input2 = ""; Result = "" }

    let createGaussianBlur standardDeviation = 
        { Type = GaussianBlur { StandardDeviation = standardDeviation; EdgeMode = None }; Input = ""; Input2 = ""; Result = "" }

    let createGaussianBlurWithEdgeMode standardDeviation edgeMode = 
        { Type = GaussianBlur { StandardDeviation = standardDeviation; EdgeMode = Some(edgeMode) }; Input = ""; Input2 = ""; Result = "" }

    let createImage image = 
        { Type = Image image; Input = ""; Input2 = ""; Result = "" }

    let createOffset offset =
        { Type = Offset offset; Input = ""; Input2 = ""; Result = "" }

    let withInput filterEffect input =
        { filterEffect with Input = input }

    let withInput2 filterEffect input =
        { filterEffect with Input2 = input }

    let withResult filterEffect result =
        { filterEffect with Result = result }

    let toString filterEffect =
        filterEffect.ToString()
