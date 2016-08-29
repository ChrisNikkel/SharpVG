namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

module TestArea =

    [<Fact>]
    let ``create area`` () =
        test <| <@ (Area.create Length.empty Length.empty |> Area.toString) = "0,0" @>

    [<Fact>]
    let ``create area of floats`` () =
        test <| <@ (Area.ofFloats (0.0,0.0) |> Area.toString) = "0,0" @>

    [<Fact>]
    let ``create area of ints`` () =
        test <| <@ (Area.ofInts (0,0) |> Area.toString) = "0,0" @>

    [<Fact>]
    let ``create area from points`` () =
        test <| <@ (Area.fromPoints Point.origin Point.origin |> Area.toString) = "0,0" @>

    [<Fact>]
    let ``transform point to attribute`` () =
        test <| <@ (Area.create Length.empty Length.empty |> Area.toAttributes |> List.map Attribute.toString |> String.concat " ") = "height=\"0\" width=\"0\"" @>

    [<Fact>]
    let ``create full area`` () =
        test <| <@ (Area.full |> Area.toString) = "100%,100%" @>

