namespace SharpVG

type Point = { X : Size; Y : Size; }

module PointHelpers =
    open Helpers
    open SizeHelpers

    let pointModifierToDescriptiveString point pre post =
        pre + "x" + post + "=" + quote (sizeToString point.X) + " " +
        pre + "y" + post + "=" + quote (sizeToString point.Y)

    let pointToDescriptiveString point =
        pointModifierToDescriptiveString point "" ""

    let pointToString point =
        sizeToString point.X + "," + sizeToString point.Y

    let pointsToString pointsToString =
        pointsToString
        |> Seq.fold (
            fun acc point ->
            acc + addSpace (acc <> "") + pointToString point
            ) ""
