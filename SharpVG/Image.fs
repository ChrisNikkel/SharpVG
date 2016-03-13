namespace SharpVG
open Helpers
open AreaHelpers
open PointHelpers

type SvgImage(image : Image, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Image(image)), style)

    member __.Image: Image = image

    override __.Name = "image"

    override __.Attributes =
        "xlink:href=" +
        quote image.Source + " " +
        pointToDescriptiveString image.UpperLeft + " " +
        areaToString image.Size