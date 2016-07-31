namespace SharpVG
open System

// TODO: Would it better to do something different here so that pixels could more magically be transformed to ems or percent.  Maybe even without ems if that is hard.
type Length =
    | UserSpace of float
    | Pixel of int
    | Em of float
    | Percent of float

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Length =

    let empty =
        UserSpace 0.0

    let full =
        Percent 100.0

    let ofUserSpace =
        UserSpace

    let ofPixels =
        Pixel

    let ofFloat =
        ofUserSpace

    let ofInt =
        float >> ofUserSpace

    let ofEm =
        Em
    
    let ofPercent =
        Percent

    let toString length =
        // TODO: Not sure how smart it is to always round things, but this prevents extra un-needed data.
        let round (n:float) = Math.Round(n, 4)
        match length with
            | UserSpace u -> sprintf "%g" (round u)
            | Pixel p -> sprintf "%d" p + "px"
            | Em e -> sprintf "%g" (round e) + "em"
            | Percent p -> sprintf "%g" (round p) + "%"

    let toFloat length =
        match length with
            | UserSpace u -> u
            | Pixel p -> float p
            | Em e -> e
            | Percent p -> p

    // TODO: Make Lengths subtractable