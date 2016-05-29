namespace SharpVG

type length =
    | Pixels of double
    | Ems of double
    | Percent of double

module Length =
    let initWithPixels =
        Pixels

    let initWithEms =
        Ems
    
    let initWithPercent =
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
            | Ems e -> double e
            | Percent p -> double p