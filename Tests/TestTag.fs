namespace SharpVG.Tests
open SharpVG
open Xunit
open BasicChecks

module TestTag =

    let testTag (name:string) (tag:string) =
        Assert.True(tag.Contains name)
        Assert.True(isMatched '<' '>' tag)
        Assert.True(isDepthNoMoreThanOne '<' '>' tag)
        Assert.True(happensEvenly '"' tag)
        Assert.True(tag.Contains "/")
        Assert.True(tag.Contains "<")
        Assert.True(tag.Contains ">")

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
