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
            | Normal -> "normal"
            | Multiply -> "multiply"
            | Screen -> "screen"
            | Overlay -> "overlay"
            | Darken -> "darken"
            | Lighten -> "lighten"
            | ColorDodge -> "color-dodge"
            | ColorBurn -> "color-burn"
            | HardLight -> "hard-light"
            | SoftLight -> "soft-light"
            | Difference -> "difference"
            | Exclusion -> "exclusion"
            | Hue -> "hue"
            | Saturation -> "saturation"
            | Color -> "color"
            | Luminosity -> "luminosity"

type EdgeMode =
    | Duplicate
    | Wrap
    | NoEdge
    override this.ToString() =
        match this with
            | Duplicate -> "duplicate"
            | Wrap -> "wrap"
            | NoEdge -> "none"

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
                | LuminanceToAlpha -> Tag.withAttributes [Attribute.createXML "type" "luminanceToAlpha"]

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
            | Over -> "over"
            | In -> "in"
            | Out -> "out"
            | Atop -> "atop"
            | Xor -> "xor"
            | Lighter -> "lighter"
            | Arithmetic -> "arithmetic"

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

type TurbulenceType =
    | FractalNoise
    | TurbulenceNoise
with
    override this.ToString() =
        match this with FractalNoise -> "fractalNoise" | TurbulenceNoise -> "turbulence"

type MorphologyOperator =
    | Erode
    | Dilate
with
    override this.ToString() =
        match this with Erode -> "erode" | Dilate -> "dilate"

type ChannelSelector =
    | RedChannel
    | GreenChannel
    | BlueChannel
    | AlphaChannel
with
    override this.ToString() =
        match this with RedChannel -> "R" | GreenChannel -> "G" | BlueChannel -> "B" | AlphaChannel -> "A"

type TransferFuncType =
    | IdentityTransfer
    | TableValues of float list
    | DiscreteValues of float list
    | LinearTransfer of slope: float * intercept: float
    | GammaTransfer of amplitude: float * exponent: float * offset: float
with
    static member ToAttributes func =
        match func with
        | IdentityTransfer -> [Attribute.createXML "type" "identity"]
        | TableValues values -> [Attribute.createXML "type" "table"; Attribute.createXML "tableValues" (values |> List.map string |> String.concat " ")]
        | DiscreteValues values -> [Attribute.createXML "type" "discrete"; Attribute.createXML "tableValues" (values |> List.map string |> String.concat " ")]
        | LinearTransfer (slope, intercept) -> [Attribute.createXML "type" "linear"; Attribute.createXML "slope" (string slope); Attribute.createXML "intercept" (string intercept)]
        | GammaTransfer (amplitude, exponent, offset) -> [Attribute.createXML "type" "gamma"; Attribute.createXML "amplitude" (string amplitude); Attribute.createXML "exponent" (string exponent); Attribute.createXML "offset" (string offset)]

type LightSource =
    | DistantLight of azimuth: float * elevation: float
    | PointLight of x: float * y: float * z: float
    | SpotLight of x: float * y: float * z: float * pointsAtX: float * pointsAtY: float * pointsAtZ: float * specularExponent: float option
with
    override this.ToString() =
        match this with
        | DistantLight (az, el) ->
            Tag.createWithAttributes "feDistantLight" [Attribute.createXML "azimuth" (string az); Attribute.createXML "elevation" (string el)] |> Tag.toString
        | PointLight (x, y, z) ->
            Tag.createWithAttributes "fePointLight" [Attribute.createXML "x" (string x); Attribute.createXML "y" (string y); Attribute.createXML "z" (string z)] |> Tag.toString
        | SpotLight (x, y, z, pax, pay, paz, specExp) ->
            Tag.createWithAttributes "feSpotLight"
                ([Attribute.createXML "x" (string x); Attribute.createXML "y" (string y); Attribute.createXML "z" (string z)
                  Attribute.createXML "pointsAtX" (string pax); Attribute.createXML "pointsAtY" (string pay); Attribute.createXML "pointsAtZ" (string paz)]
                 @ (specExp |> Option.map (fun e -> [Attribute.createXML "specularExponent" (string e)]) |> Option.defaultValue []))
            |> Tag.toString

type FilterEffectType =
    | Blend of BlendMode * FilterEffectSource option * FilterEffectSource option
    | ColorMatrix of ColorMatrix * FilterEffectSource option
    | ComponentTransfer of rFunc: TransferFuncType * gFunc: TransferFuncType * bFunc: TransferFuncType * aFunc: TransferFuncType * FilterEffectSource option
    | Composite of Composite * FilterEffectSource option * FilterEffectSource option
    | ConvolveMatrix of order: int * kernelMatrix: float list * divisor: float option * bias: float option * preserveAlpha: bool * FilterEffectSource option
    | DiffuseLighting of DiffuseLighting * FilterEffectSource option
    | DisplacementMap of scale: float * xSelector: ChannelSelector * ySelector: ChannelSelector * FilterEffectSource option * FilterEffectSource option
    | Flood of Flood
    | GaussianBlur of GaussianBlur * FilterEffectSource option
    | Image of string
    | Merge of FilterEffectSource list
    | Morphology of MorphologyOperator * radius: float * FilterEffectSource option
    | Offset of Point * FilterEffectSource option
    | SpecularLighting of surfaceScale: float * specularConstant: float * specularExponent: float * LightSource * FilterEffectSource option
    | Turbulence of TurbulenceType * baseFrequency: float * numOctaves: int * seed: int option
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
            | ComponentTransfer (rFunc, gFunc, bFunc, aFunc, input) ->
                let funcToTag name func =
                    Tag.createWithAttributes ("feFunc" + name) (TransferFuncType.ToAttributes func) |> Tag.toString
                let body = funcToTag "R" rFunc + funcToTag "G" gFunc + funcToTag "B" bFunc + funcToTag "A" aFunc
                Tag.create "feComponentTransfer"
                |> Tag.addAttributes (inputsToAttributes input None)
                |> Tag.withBody body
            | Composite (composite, input1, input2) ->
                Tag.create "feComposite" |> Tag.addAttributes ((Attribute.createXML "operator" (composite.ToString())) :: inputsToAttributes input1 input2)
            | ConvolveMatrix (order, kernelMatrix, divisor, bias, preserveAlpha, input) ->
                Tag.create "feConvolveMatrix"
                |> Tag.addAttributes
                    ([Attribute.createXML "order" (string order)
                      Attribute.createXML "kernelMatrix" (kernelMatrix |> List.map string |> String.concat " ")]
                     @ (divisor |> Option.map (fun d -> [Attribute.createXML "divisor" (string d)]) |> Option.defaultValue [])
                     @ (bias |> Option.map (fun b -> [Attribute.createXML "bias" (string b)]) |> Option.defaultValue [])
                     @ [Attribute.createXML "preserveAlpha" (if preserveAlpha then "true" else "false")])
                |> Tag.addAttributes (inputsToAttributes input None)
            | DiffuseLighting (diffuseLighting, input) -> DiffuseLighting.ToTag diffuseLighting |> Tag.addAttributes (inputsToAttributes input None)
            | DisplacementMap (scale, xSel, ySel, input1, input2) ->
                Tag.create "feDisplacementMap"
                |> Tag.addAttributes
                    [Attribute.createXML "scale" (string scale)
                     Attribute.createXML "xChannelSelector" (xSel.ToString())
                     Attribute.createXML "yChannelSelector" (ySel.ToString())]
                |> Tag.addAttributes (inputsToAttributes input1 input2)
            | Flood (flood) -> Flood.ToTag flood
            | GaussianBlur (gaussianBlur, input) -> GaussianBlur.ToTag gaussianBlur |> Tag.addAttributes (inputsToAttributes input None)
            | Image (image) -> Tag.createWithAttribute "feImage" (Attribute.createXML "xlink:href" image)
            | Merge inputs ->
                let body = inputs |> List.map (fun src -> Tag.createWithAttribute "feMergeNode" (Attribute.createXML "in" (src.ToString())) |> Tag.toString) |> String.concat ""
                Tag.create "feMerge" |> Tag.withBody body
            | Morphology (op, radius, input) ->
                Tag.create "feMorphology"
                |> Tag.addAttributes [Attribute.createXML "operator" (op.ToString()); Attribute.createXML "radius" (string radius)]
                |> Tag.addAttributes (inputsToAttributes input None)
            | Offset (offset, input) -> Tag.createWithAttributes "feOffset" (Point.toAttributesWithModifier "d" "" offset) |> Tag.addAttributes (inputsToAttributes input None)
            | SpecularLighting (surfaceScale, specConst, specExp, lightSource, input) ->
                Tag.create "feSpecularLighting"
                |> Tag.addAttributes
                    [Attribute.createXML "surfaceScale" (string surfaceScale)
                     Attribute.createXML "specularConstant" (string specConst)
                     Attribute.createXML "specularExponent" (string specExp)]
                |> Tag.addAttributes (inputsToAttributes input None)
                |> Tag.withBody (lightSource.ToString())
            | Turbulence (turbType, baseFreq, numOctaves, seed) ->
                Tag.create "feTurbulence"
                |> Tag.addAttributes
                    ([Attribute.createXML "type" (turbType.ToString())
                      Attribute.createXML "baseFrequency" (string baseFreq)
                      Attribute.createXML "numOctaves" (string numOctaves)]
                     @ (seed |> Option.map (fun s -> [Attribute.createXML "seed" (string s)]) |> Option.defaultValue []))
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

    let createComponentTransfer rFunc gFunc bFunc aFunc =
        { FilterEffectType = ComponentTransfer (rFunc, gFunc, bFunc, aFunc, None); Offset = None; Scale = None }

    let createComponentTransferWithInput rFunc gFunc bFunc aFunc input =
        { FilterEffectType = ComponentTransfer (rFunc, gFunc, bFunc, aFunc, Some input); Offset = None; Scale = None }

    let createConvolveMatrix order kernelMatrix =
        { FilterEffectType = ConvolveMatrix (order, kernelMatrix, None, None, false, None); Offset = None; Scale = None }

    let createConvolveMatrixFull order kernelMatrix divisor bias preserveAlpha =
        { FilterEffectType = ConvolveMatrix (order, kernelMatrix, divisor, bias, preserveAlpha, None); Offset = None; Scale = None }

    let createDisplacementMap scale xSelector ySelector =
        { FilterEffectType = DisplacementMap (scale, xSelector, ySelector, None, None); Offset = None; Scale = None }

    let createDisplacementMapWithInputs scale xSelector ySelector input1 input2 =
        { FilterEffectType = DisplacementMap (scale, xSelector, ySelector, Some input1, Some input2); Offset = None; Scale = None }

    let createMerge inputs =
        { FilterEffectType = Merge inputs; Offset = None; Scale = None }

    let createMorphology op radius =
        { FilterEffectType = Morphology (op, radius, None); Offset = None; Scale = None }

    let createMorphologyWithInput op radius input =
        { FilterEffectType = Morphology (op, radius, Some input); Offset = None; Scale = None }

    let createSpecularLighting surfaceScale specularConstant specularExponent lightSource =
        { FilterEffectType = SpecularLighting (surfaceScale, specularConstant, specularExponent, lightSource, None); Offset = None; Scale = None }

    let createSpecularLightingWithInput surfaceScale specularConstant specularExponent lightSource input =
        { FilterEffectType = SpecularLighting (surfaceScale, specularConstant, specularExponent, lightSource, Some input); Offset = None; Scale = None }

    let createTurbulence turbType baseFrequency numOctaves =
        { FilterEffectType = Turbulence (turbType, baseFrequency, numOctaves, None); Offset = None; Scale = None }

    let createTurbulenceWithSeed turbType baseFrequency numOctaves seed =
        { FilterEffectType = Turbulence (turbType, baseFrequency, numOctaves, Some seed); Offset = None; Scale = None }

    let withName filterEffect result =
        { FilterEffect = filterEffect; Name = result }

    let withOffset (filterEffect: FilterEffect) offset =
        { filterEffect with Offset = offset }

    let withScale (filterEffect: FilterEffect) scale =
        { filterEffect with Scale = scale }

    let toString filterEffect =
        filterEffect.ToString()
