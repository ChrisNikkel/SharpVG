namespace SharpVG

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

type Flood =
    {
        Color : Color
        Opacity : float // TODO: Make optional and onstrain to 0 to 1.0 or make type for percent
    }

type GaussianBlur =
    {
        StandardDeviation : float
        EdgeMode : EdgeMode
    }

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
with
    static member ToTag filterEffect =
        match filterEffect with
            | Blend (blend) -> Tag.create "feBlend" |> Tag.withAttribute (Attribute.createXML "mode" (blend.ToString()))
            | ColorMatrix (colorMatrix) -> ColorMatrix.ToTag colorMatrix
            | Composite (composite) -> Tag.create "feComposite" |> Tag.withAttribute (Attribute.createXML "operator" (composite.ToString()))
            | DiffuseLighting (diffuseLighting) -> Tag.create "feDiffuseLighting" // TODO: Add attributes
            | Flood (flood) -> Tag.create "Flood" |> Tag.withAttributes [Attribute.createXML "flood-color" (flood.Color |> Color.toString); Attribute.createXML "flood-opacity" (string flood.Opacity)]
            | GaussianBlur (gaussianBlur) -> Tag.create "feGaussianBlur" // TODO: Add attributes
            | Image (image) -> Tag.create "feImage" // TODO: Add attributes
            | Offset (offset) -> Tag.create "feOffset" // TODO: Add attributes

    override this.ToString() =
        this |> FilterEffectType.ToTag |> Tag.toString

type FilterEffect =
    {
        Type : FilterEffectType
        Input : string
        Input2 : string
        Result : string
    }

// TODO: make it easy to string things together so that if result or inputs aren't specified random ids are created and linked together.  

module FilterEffect =

    let create filterEffectType = 
        { Type = filterEffectType; Input = ""; Input2 = ""; Result = "" }
    
    let toString filterEffect =
        filterEffect.ToString()
