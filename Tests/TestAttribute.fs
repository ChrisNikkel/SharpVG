namespace SharpVG.Tests
open SharpVG
open Xunit
open BasicChecks

module TestAttribute =

    let testAttribute (name:string) (value:string) (attribute:string) =
        Assert.True(attribute.Contains name)
        Assert.True(attribute.Contains value)
        Assert.True(happensEvenly '"' attribute)
        Assert.True(attribute.Contains "=")

    [<Fact>]
    let ``create attribute`` () =
        let name, value = "name", "value"
        testAttribute name value ((Attribute.create AttributeType.CSS name value) |> Attribute.toString)

    [<Fact>]
    let ``create XML attribute`` () =
        let name, value = "name", "value"
        testAttribute name value ((Attribute.createXML name value) |> Attribute.toString)

    [<Fact>]
    let ``create CSS attribute`` () =
        let name, value = "name", "value"
        testAttribute name value ((Attribute.createCSS name value) |> Attribute.toString)
