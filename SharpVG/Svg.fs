namespace SharpVG
open Helpers
open PointHelpers
open AreaHelpers
open TransformHelpers

module Core =

    let baseElementToSvg baseElement style =
        match baseElement with
            | Circle c -> SvgCircle(c, style) :> SvgElement
            | Ellipse e -> SvgEllipse(e, style) :> SvgElement
            | Image i -> SvgImage(i, style) :> SvgElement
            | Line l -> SvgLine(l, style) :> SvgElement
            | Polygon p -> SvgPolygon(p, style) :> SvgElement
            | Polyline p -> SvgPolyline(p, style) :> SvgElement
            | Rect r -> SvgRect(r, style) :> SvgElement
            | Text t -> SvgText(t, style) :> SvgElement

    let elementToSvg =
        function
            | StyledElement(b, s) -> baseElementToSvg b (Some(s))
            | PlainElement(b) -> baseElementToSvg b None


    let html title body =
        "<!DOCTYPE html>\n<html>\n<head>\n  <title>" +
        title +
        "</title>\n</head>\n<body>\n" +
        body +
        "</body>\n</html>\n"

    let style =
        "<style>
        circle {
        stroke: #006600;
        fill  : #00cc00;
        }
        circle.allBlack {
        stroke: #000000;
        fill  : #000000;
        }\n" +
        "</style>\n"

    // TODO: Make object and allow things lie fromSeq, fromList, etc.
    let svg size body =
        "<svg " + areaToString size + ">\n  " +
        body +
        "\n</svg>\n"

    let group id transform point body =
        "<g id=" + quote id +
        transformToString transform +
        pointToString point + ">" +
        body +
        "</g>"