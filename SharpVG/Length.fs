namespace SharpVG
open System

// TODO: Would it better to do something different here so that pixels could more magically be transformed to ems or percent.  Maybe even without ems if that is hard.
type Length =
    | Pixel of float
    | Em of float
    | Percent of float

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Length =

    let empty =
        Pixel 0.0

    let full =
        Percent 100.0

    let ofPixels =
        Pixel

    let ofFloat =
        ofPixels

    let ofInt =
        float >> ofPixels

    let ofEm =
        Em
    
    let ofPercent =
        Percent

    let toString length =
        // TODO: Not sure how smart it is to always round things, but this prevents extra un-needed data.
        let round (n:float) = Math.Round(n, 4)
        match length with
            | Pixel p -> sprintf "%g" (round p)
            | Em e -> sprintf "%g" (round e) + "em"
            | Percent p -> sprintf "%g" (round p) + "%"

    let toFloat length =
        match length with
            | Pixel p -> float p
            | Em e -> e
            | Percent p -> p

    let toDouble length =
        match length with
            | Pixel p -> p
            | Em e -> float e
            | Percent p -> float p

        // TODO: Make Lengths subtractable