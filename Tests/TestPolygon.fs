namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestPolygon =

    [<Fact>]
    let ``create polygon`` () =
        test <| <@ seq { yield Point.origin; yield Point.origin; yield Point.origin; yield Point.origin } |> Polygon.toString = "<polygon points=\"0,0 0,0 0,0 0,0\"/>" @>