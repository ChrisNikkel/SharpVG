namespace SharpVG

type Size =
    | Pixels of int
    | Ems of double
    | Percent of int

module SizeHelpers =
    let sizeToString size =
        match size with
        | Pixels p -> string p
        | Ems e -> string e + "em"
        | Percent p -> string p + "%"