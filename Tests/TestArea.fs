namespace SharpVG.Tests

namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestArea =

    [<Fact>]
    let ``create area`` () =
        test <| <@ (Area.create Length.empty Length.empty |> Area.toString) = "0,0" @>

    [<Fact>]
    let ``transform point to attribute`` () =
        test <| <@ (Area.create Length.empty Length.empty |> Area.toAttributes |> List.map Attribute.toString |> String.concat " ") = "height=\"0\" width=\"0\"" @>
