namespace SharpVG

type size =
    | Pixels of int
    | Ems of float
    | Percent of float

module Size =
    let toString size =
        match size with
            | Pixels p -> string p
            | Ems e -> string e + "em"
            | Percent p -> string p + "%"

    let toFloat size =
        match size with
            | Pixels p -> float p
            | Ems e -> e
            | Percent p -> p

    let toInt size =
        match size with
            | Pixels p -> p
            | Ems e -> int e
            | Percent p -> int p