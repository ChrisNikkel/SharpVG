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
        Opacity : float option // TODO: Make constrain to 0 to 1.0 or make type for percent
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
    | Blend of BlendMode * FilterEffectSource option * FilterEffectSource option
    | ColorMatrix of ColorMatrix * FilterEffectSource option
    // TODO: Implement ComponentTransfer
    | Composite of Composite * FilterEffectSource option * FilterEffectSource option
    // TODO: Implement ConvolveMatrix
    | DiffuseLighting of DiffuseLighting * FilterEffectSource option
    // TODO: Implement DisplacementMap
    | Flood of Flood
    | GaussianBlur of GaussianBlur * FilterEffectSource option
    | Image of string // TODO: Create type to use here that can reference an ElementId or contain an Href
    // TODO: Implement Merge
    // TODO: Implement MergeNode
    // TODO: Implement Morphology
    | Offset of Point * FilterEffectSource option
    // TODO: Implement SpecularLighting
    // TODO: Implement Tile
    // TODO: Implement Turbulence
and FilterEffect =
    {
        FilterEffectType : FilterEffectType
        Offset : Point option
        Scale : Area option
    }
with
    static member ToTag filterEffect =

        let inputsToAttributes input1 input2 =
            [
                input1 |> Option.map (fun x -> (x.ToString()) |> Attribute.createXML "in")
                input2 |> Option.map (fun x -> (x.ToString()) |> Attribute.createXML "in2")
            ] |> List.choose id

        let additionalAttributes filterEffect =
            [
                filterEffect.Offset |> Option.map Point.toAttributes
                filterEffect.Scale |> Option.map Area.toAttributes
            ] |> List.choose id |> List.concat

        match filterEffect.FilterEffectType with
            | Blend (blend, input1, input2) -> Tag.create "feBlend" |> Tag.addAttributes ((Attribute.createXML "mode" (blend.ToString())) :: inputsToAttributes input1 input2)
            | ColorMatrix (colorMatrix, input) -> ColorMatrix.ToTag colorMatrix |> Tag.addAttributes (inputsToAttributes input None)
            | Composite (composite, input1, input2) ->
                Tag.create "feComposite" |> Tag.addAttributes ((Attribute.createXML "operator" (composite.ToString())) :: inputsToAttributes input1 input2)
            | DiffuseLighting (diffuseLighting, input) -> DiffuseLighting.ToTag diffuseLighting |> Tag.addAttributes (inputsToAttributes input None)
            | Flood (flood) -> Flood.ToTag flood
            | GaussianBlur (gaussianBlur, input) -> GaussianBlur.ToTag gaussianBlur |> Tag.addAttributes (inputsToAttributes input None)
            | Image (image) -> Tag.createWithAttribute "feImage" (Attribute.createXML "xlink:href" image)
            | Offset (offset, input) -> Tag.createWithAttributes "feOffset" (Point.toAttributesWithModifier "d" "" offset) |> Tag.addAttributes (inputsToAttributes input None)
        |> Tag.addAttributes (additionalAttributes filterEffect)

    override this.ToString() =
        this |> FilterEffect.ToTag |> Tag.toString

and NamedFilterEffect =
    {
        FilterEffect : FilterEffect
        Name : string
    }
with
    static member ToTag namedFilterEffect =
        FilterEffect.ToTag namedFilterEffect.FilterEffect |> Tag.addAttribute (Attribute.createXML "result" namedFilterEffect.Name)

    override this.ToString() =
        this |> NamedFilterEffect.ToTag |> Tag.toString

and FilterEffectSource =
    | SourceGraphic
    | SourceAlpha
    | BackgroundImage
    | BackgroundAlpha
    | FillPaint
    | StrokePaint
    | NamedFilterEffect of NamedFilterEffect
    override this.ToString() =
        match this with
            | SourceGraphic -> "SourceGraphic"
            | SourceAlpha -> "SourceAlpha"
            | BackgroundImage -> "BackgroundImage"
            | BackgroundAlpha -> "BackgroundAlpha"
            | FillPaint -> "FillPaint"
            | StrokePaint -> "StrokePaint"
            | NamedFilterEffect namedFilterEffect -> namedFilterEffect.Name

// TODO: make it easy to string things together so that if result or inputs aren't specified random ids are created and linked together.

module FilterEffect =

    let createBlend blend =
        { FilterEffectType = Blend (blend, None, None); Offset = None; Scale = None }

    let createBlendWithInput blend input =
        { FilterEffectType = Blend (blend, Some(input), None); Offset = None; Scale = None }

    let createBlendWithInputs blend input1 input2 =
        { FilterEffectType = Blend (blend, Some(input1), Some(input2)); Offset = None; Scale = None }

    let createColorMatrix colorMatrix =
        { FilterEffectType = ColorMatrix (colorMatrix, None); Offset = None; Scale = None }

    let createColorMatrixWithInput colorMatrix input =
        { FilterEffectType = ColorMatrix (colorMatrix, input); Offset = None; Scale = None }

    let createComposite composite =
        { FilterEffectType = Composite (composite, None, None); Offset = None; Scale = None }

    let createCompositeWithInput composite input =
        { FilterEffectType = Composite (composite, Some input, None); Offset = None; Scale = None }

    let createCompositeWithInputs composite input1 input2 =
        { FilterEffectType = Composite (composite, Some input1, Some input2); Offset = None; Scale = None }

    let createDiffuseLighting surfaceScale diffuseConstant kernelUnitLength =
        { FilterEffectType = DiffuseLighting ({ SurfaceScale = surfaceScale; DiffuseConstant = diffuseConstant; KernelUnitLength = kernelUnitLength }, None); Offset = None; Scale = None }

    let createDiffuseLightingWithInput surfaceScale diffuseConstant kernelUnitLength input=
        { FilterEffectType = DiffuseLighting ({ SurfaceScale = surfaceScale; DiffuseConstant = diffuseConstant; KernelUnitLength = kernelUnitLength }, Some input); Offset = None; Scale = None }

    let createFlood color =
        { FilterEffectType = Flood { Color = color; Opacity = None }; Offset = None; Scale = None }

    let createFloodWithOpacity color opacity =
        { FilterEffectType = Flood { Color = color; Opacity = Some opacity }; Offset = None; Scale = None }

    let createGaussianBlur standardDeviation =
        { FilterEffectType = GaussianBlur ({ StandardDeviation = standardDeviation; EdgeMode = None }, None); Offset = None; Scale = None }

    let createGaussianBlurWithEdgeMode standardDeviation edgeMode =
        { FilterEffectType = GaussianBlur ({ StandardDeviation = standardDeviation; EdgeMode = Some(edgeMode) }, None); Offset = None; Scale = None }

    let createGaussianBlurWithInput standardDeviation input =
        { FilterEffectType = GaussianBlur ({ StandardDeviation = standardDeviation; EdgeMode = None }, Some input); Offset = None; Scale = None }

    let createGaussianBlurWithEdgeModeAndInput standardDeviation edgeMode input =
        { FilterEffectType = GaussianBlur ({ StandardDeviation = standardDeviation; EdgeMode = Some(edgeMode) }, Some input); Offset = None; Scale = None }

    let createImage image =
        { FilterEffectType = Image image; Offset = None; Scale = None }

    let createOffset offset =
        { FilterEffectType = Offset (offset, None); Offset = None; Scale = None }

    let createOffsetWithInput offset input =
        { FilterEffectType = Offset (offset, Some input); Offset = None; Scale = None }

    let withName filterEffect result =
        { FilterEffect = filterEffect; Name = result }

    let withOffset filterEffect offset =
        { filterEffect with Offset = offset }

    let withScale filterEffect scale =
        { filterEffect with Scale = scale }

    let toString filterEffect =
        filterEffect.ToString()
