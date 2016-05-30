namespace SharpVG

type transform = {
    Matrix: (double * double * double * double * double * double) option
    Translate: (double * double) option
    Scale: (double * double) option
    Rotate: (double * double * double) option
    SkewX: double option
    SkewY: double option
 }

module Transform =
    let empty = { Matrix = None; Translate = None; Scale = None; Rotate = None; SkewX = None; SkewY = None }

    let withMatrix matrix transform =
        { transform with Matrix = Some matrix }

    let withTranslate translate transform =
        { transform with Translate = Some translate }

    let withScale scale transform =
        { transform with Scale = Some scale }
    
    let withRotate rotate transform =
        { transform with Rotate = Some rotate }

    let withSkewX skewX transform =
        { transform with SkewX = Some skewX }
    
    let withSkewY skewY transform =
        { transform with SkewY = Some skewY }

    let initWithMatrix matrix =
        empty |> withMatrix matrix

    let initWithTranslate translate =
        empty |> withTranslate translate

    let initWithScale scale =
        empty |> withScale scale

    let initWithRotate rotate =
        empty |> withRotate rotate

    let initWithSkewX skewX =
        empty |> withSkewX skewX

    let initWithSkewY skewY =
        empty |> withSkewY skewY

    let toString transform =
        "transform=" +
        Tag.quote (
            seq {
                yield match transform.Matrix with | Some(a, b, c, d, e, f) -> Some("matrix(" + string a + ","  + string b + ","  + string c + ","  + string d + ","  + string e + ","  + string f + ")") | None -> None
                yield match transform.Translate with | Some(x, y) -> Some("translate(" + string x + "," + string y + ")") | None -> None
                yield match transform.Scale with | Some(x, y) -> Some("scale(" + string x + "," + string y + ")") | None -> None
                yield match transform.Rotate with | Some(a, x, y) -> Some("rotate(" + string a + "," + string x + "," + string y + ")") | None -> None
                yield match transform.SkewX with | Some(a) -> Some("skewX(" + string a + ")") | None -> None
                yield match transform.SkewY with | Some(a) -> Some("skewY(" + string a + ")") | None -> None
            }
            |> Seq.choose id
            |> String.concat " "
        )