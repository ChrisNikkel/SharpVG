namespace SharpVG

// TODO: Allow multi transforms for static transformations using a seq/list
type Transform =
    | Matrix of A: Length * B: Length * C: Length * D: Length * E: Length * F: Length
    | Translate of X: Length * Y: Length option
    | Scale of X: Length * Y: Length option
    | Rotate of Angle: float * Point: ((Length * Length) option)
    | SkewX of Angle: float
    | SkewY of Angle: float

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Transform =

    let createMatrix matrix =
        Matrix matrix

    let createTranslate x =
        Translate (x, None)

    let createScale x =
        Scale (x, None)
    
    let createRotate angle =
        Rotate (angle, None)

    let createSkewX angle =
        SkewX angle
    
    let createSkewY angle =
        SkewY angle

    let getTypeName transform =
        match transform with
            | Matrix _ -> "matrix"
            | Translate _ -> "translate"
            | Scale _ -> "scale"
            | Rotate _ -> "rotate"
            | SkewX _ -> "skewX"
            | SkewY _ -> "skewY"

    let withX x transform =
        match transform with
            | Rotate (a, None) -> Rotate (a, Some (x, Length.empty))
            | Rotate (a, Some (_, y)) -> Rotate (a, Some (x, y))
            | _ -> failwith ("Not able to set x with transform of type: " + getTypeName transform)

    let withY y transform =
        match transform with
            | Translate (x, _) -> Translate (x, Some y)
            | Scale (x, _) -> Scale (x, Some y)
            | Rotate (a, None) -> Rotate (a, Some (Length.empty, y))
            | Rotate (a, Some (x, _)) -> Rotate (a, Some (x, y))
            | _ -> failwith ("Not able to set y with transform of type: " + getTypeName transform)

    let toStringWithSeparator separator transform =
        let s = separator
        match transform with
            | Matrix (a, b, c, d, e, f) ->  Length.toString a + s + Length.toString b + s + Length.toString c + s + Length.toString d + s + Length.toString e + s + Length.toString f 
            | Translate (x, None) | Scale (x, None) -> Length.toString x
            | Translate (x, Some y) | Scale (x, Some y) -> Length.toString x + s + Length.toString y
            | Rotate (a, None) | SkewX a | SkewY a -> string a
            | Rotate (a, Some (x, y)) -> string a + "," + Length.toString x + s + Length.toString y

    let toString =
        toStringWithSeparator " "

    let toStringWithStyle transform =
        getTypeName transform + "(" + toStringWithSeparator "," transform + ")"

    let toAttribute transform =
        Attribute.create "transform" (toStringWithStyle transform)

module Transforms =
    let toAttribute transforms =
        Attribute.create "transform" (transforms |> Seq.map Transform.toStringWithStyle |> String.concat " ")