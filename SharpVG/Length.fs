namespace SharpVG
open System

type Length =
    | UserSpace of float
    | Pixels of int
    | Em of float
    | Percent of float
    | Cm of float
    | Mm of float
    | In of float
    | Pt of float
    override this.ToString() =
        let round (n:float) = Math.Round(n, 4)
        match this with
            | UserSpace u -> sprintf "%g" (round u)
            | Pixels p -> sprintf "%d" p + "px"
            | Em e -> sprintf "%g" (round e) + "em"
            | Percent p -> sprintf "%g" (round p) + "%"
            | Cm p -> sprintf "%g" (round p) + "cm"
            | Mm p -> sprintf "%g" (round p) + "mm"
            | In p -> sprintf "%g" (round p) + "in"
            | Pt p -> sprintf "%g" (round p) + "pt"

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

    let ofCm =
        Cm
    
    let ofMm =
        Mm

    let ofIn =
        In

    let ofPt =
        Pt

    let toString (length : Length) =
        length.ToString()

    let toFloat length =
        match length with
            | UserSpace u -> u
            | Pixels p -> float p
            | Em e -> e
            | Percent p -> p
            | Cm p -> p
            | Mm p -> p
            | In p -> p
            | Pt p -> p

    let toInt length =
        match length with
            | UserSpace u -> int u
            | Pixels p -> p
            | Em e -> int e
            | Percent p -> int p
            | Cm p -> int p
            | Mm p -> int p
            | In p -> int p
            | Pt p -> int p
