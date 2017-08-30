namespace SharpVG
open System

type Length =
    | UserSpace of float
    | Pixels of int
    | Em of float
    | Percent of float

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Length =

    let empty =
        UserSpace 0.0

    let full =
        Percent 100.0

    let half =
        Percent 50.0

    let one =
        UserSpace 1.0

    let ofUserSpace =
        UserSpace

    let ofPixels =
        Pixels

    let ofFloat =
        ofUserSpace

    let ofInt =
        float >> ofUserSpace

    let ofEm =
        Em

    let ofPercent =
        Percent

    let toString length =
        let round (n:float) = Math.Round(n, 4)
        match length with
            | UserSpace u -> sprintf "%g" (round u)
            | Pixels p -> sprintf "%d" p + "px"
            | Em e -> sprintf "%g" (round e) + "em"
            | Percent p -> sprintf "%g" (round p) + "%"

    let toFloat length =
        match length with
            | UserSpace u -> u
            | Pixels p -> float p
            | Em e -> e
            | Percent p -> p
