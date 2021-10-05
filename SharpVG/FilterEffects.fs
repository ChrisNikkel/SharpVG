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

type ColorMatrix =
    | Matrix of int list // TODO: Convert top a 5x4 matrix object of some sort
    | Saturate of float // TODO: Constrain to 0 to 1.0 or make type for percent
    | HueRotate of float // TODO: Make type for degrees
    | LuminanceToAlpha

type Composite =
    | Over
    | In
    | Out
    | Atop
    | Xor
    | Lighter
    | Arithmetic

type DiffuseLighting =
    {
        SurfaceScale : float
        DiffuseConstant : float
        KernelUnitLength : float option
    }

type Flood =
    {
        Color : Color
        Opacity : float option // TODO: Constrain to 0 to 1.0 or make type for percent
    }

type GaussianBlur =
    {
        StandardDeviation : float
        EdgeMode : EdgeMode
    }

type FilterEffects =
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
    static member ToTag filterEffects =
        match filterEffects with
            | Blend (blend) -> Tag.create "feBlend" |> Tag.withAttribute (Attribute.createXML "mode" (blend.ToString()))
            | ColorMatrix (colorMatrix) -> Tag.create "feColorMatrix" // TODO: Add attributes
            | Composite (composite) -> Tag.create "feComposite" // TODO: Add attributes
            | DiffuseLighting (diffuseLighting) -> Tag.create "feDiffuseLighting" // TODO: Add attributes
            | Flood (flood) -> Tag.create "Flood" // TODO: Add attributes
            | GaussianBlur (gaussianBlur) -> Tag.create "feGaussianBlur" // TODO: Add attributes
            | Image (image) -> Tag.create "feImage" // TODO: Add attributes
            | Offset (offset) -> Tag.create "feOffset" // TODO: Add attributes

    override this.ToString() =
        this |> FilterEffects.ToTag |> Tag.toString

module FilterEffects =
    let empty : string =
        invalidOp "Not Implemented"

    let create = empty // TODO: Figure out what the most useful create function would be (which are likely inner tags)
