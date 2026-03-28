namespace SharpVG

type AspectRatioAlign =
    | XMinYMin | XMidYMin | XMaxYMin
    | XMinYMid | XMidYMid | XMaxYMid
    | XMinYMax | XMidYMax | XMaxYMax
with
    override this.ToString() =
        match this with
        | XMinYMin -> "xMinYMin" | XMidYMin -> "xMidYMin" | XMaxYMin -> "xMaxYMin"
        | XMinYMid -> "xMinYMid" | XMidYMid -> "xMidYMid" | XMaxYMid -> "xMaxYMid"
        | XMinYMax -> "xMinYMax" | XMidYMax -> "xMidYMax" | XMaxYMax -> "xMaxYMax"

type AspectRatioScale =
    | Meet
    | Slice
with
    override this.ToString() =
        match this with
        | Meet -> "meet"
        | Slice -> "slice"

type PreserveAspectRatio =
    | NoPreservation
    | Uniform of AspectRatioAlign * AspectRatioScale option
with
    override this.ToString() =
        match this with
        | NoPreservation -> "none"
        | Uniform (align, None) -> align.ToString()
        | Uniform (align, Some scale) -> align.ToString() + " " + scale.ToString()

module PreserveAspectRatio =
    let create align = Uniform (align, None)
    let createWithScale align scale = Uniform (align, Some scale)
    let none = NoPreservation
    let toAttribute (par: PreserveAspectRatio) =
        Attribute.createXML "preserveAspectRatio" (par.ToString())
    let toString (par: PreserveAspectRatio) = par.ToString()
