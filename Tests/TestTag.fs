namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestTag =

    let testTag (name:string) (tag:string) =
        test <| <@ tag.Contains name @>
        test <| <@ isMatched '<' '>' tag @>
        test <| <@ isDepthNoMoreThanOne '<' '>' tag @>
        test <| <@ happensEvenly '"' tag @>
        test <| <@ tag.Contains "/" @>
        test <| <@ tag.Contains "<" @>
        test <| <@ tag.Contains ">" @>

    [<Fact>]
    let ``create tag`` ()=
        let name = "name"
        testTag name ((Tag.create name) |> Tag.toString)

    [<Fact>]
    let ``create tag with attribute`` ()=
        let name = "name"
        let attribute = Attribute.createXML "name" "value"
        testTag name ((Tag.create name) |> Tag.withAttribute attribute |> Tag.toString)

    [<Fact>]
    let ``create tag with body`` ()=
        let name, body = "name", "body"
        testTag name ((Tag.create name) |> Tag.withBody body |> Tag.toString)
