namespace SharpVG

type Size =
    | Pixels of int
    | Ems of float
    | Percent of float

module SizeHelpers =
    let sizeToString size =
        match size with
        | Pixels p -> string p
        | Ems e -> string e + "em"
        | Percent p -> string p + "%"

    let sizeToFloat size =
        match size with
        | Pixels p -> float p
        | Ems e -> e
        | Percent p -> p

    let sizeToInt size =
        match size with
        | Pixels p -> p
        | Ems e -> int e
        | Percent p -> int p