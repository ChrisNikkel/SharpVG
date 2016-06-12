namespace SharpVG

type length =
    | Pixels of float
    | Ems of float
    | Percent of float

module Length =
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