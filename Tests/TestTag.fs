namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestTag =

    let testTag (name:string) (tag:string) =
        <@ (tag.Contains name)
        && (isMatched '<' '>' tag)
        && (isDepthNoMoreThanOne '<' '>' tag)
        && (happensEvenly '"' tag)
        && (tag.Contains "/")
        && (tag.Contains "<")
        && (tag.Contains ">")
        @>

    [<Fact>]
    let ``create tag`` ()=
        let name = "name"
        test <| testTag name ((Tag.create name) |> Tag.toString)

    [<Fact>]
    let ``create tag with attribute`` ()=
        let name = "name"
        let attribute = Attribute.createXML "name" "value"
        test <| testTag name ((Tag.create name) |> Tag.withAttribute attribute |> Tag.toString)

    [<Fact>]
    let ``create tag with body`` ()=
        let name, body = "name", "body"
        test <| testTag name ((Tag.create name) |> Tag.withBody body |> Tag.toString)
