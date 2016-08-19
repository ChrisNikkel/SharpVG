namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestPoint =

    [<Fact>]
    let ``create point`` () =
        test <| <@ (Point.create Length.empty Length.empty |> Point.toString) = "0,0" @>

    [<Fact>]
    let ``create origin`` () =
        test <| <@ (Point.origin |> Point.toString) = "0,0" @>

    [<Fact>]
    let ``transform point to attribute`` () =
        test <| <@ (Point.origin |> Point.toAttributes |> List.map Attribute.toString |> String.concat " ") = "x=\"0\" y=\"0\"" @>

    [<Fact>]
    let ``transform point to attribute with modifier`` () =
        test <| <@ (Point.origin |> Point.toAttributesWithModifier "a" "b" |> List.map Attribute.toString |> String.concat " ") = "axb=\"0\" ayb=\"0\"" @>

    [<Fact>]
    let ``transform point to attribute with separator`` () =
        test <| <@ (Point.origin |> Point.toStringWithSeparator " ") = "0 0" @>

    [<Fact>]
    let ``points to string`` () =
        test <| <@ (seq { yield Point.origin; yield Point.origin } |> Points.toString) = "0,0 0,0" @>
