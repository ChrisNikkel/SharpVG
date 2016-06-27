namespace SharpVG

type Length =
    | Pixels of float
    | Ems of float
    | Percent of float

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Length =

    let empty =
        Pixels 0.0

    let createWithPixels =
        Pixels

    let createWithEms =
        Ems
    
    let createWithPercent =
        Percent

    let toString length =
        match length with
            | Pixels p -> string p
            | Ems e -> string e + "em"
            | Percent p -> string p + "%"

    let toFloat length =
        match length with
            | Pixels p -> float p
            | Ems e -> e
            | Percent p -> p

    let toDouble length =
        match length with
            | Pixels p -> p
            | Ems e -> float e
            | Percent p -> float p