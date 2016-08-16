namespace SharpVG.Tests
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks
open Swensen.Unquote

module TestAttribute =

    let testAttribute (name:string) (value:string) (attribute:string) =
        <@ (attribute.Contains name)
        && (attribute.Contains value)
        && (happensEvenly '"' attribute)
        && (attribute.Contains "=") @>

    [<Fact>]
    let ``create attribute`` () =
        let name, value = "name", "value"
        test <| testAttribute name value ((Attribute.create AttributeType.CSS name value) |> Attribute.toString)

    [<Fact>]
    let ``create XML attribute`` () =
        let name, value = "name", "value"
        test <| testAttribute name value ((Attribute.createXML name value) |> Attribute.toString)

    [<Fact>]
    let ``create CSS attribute`` () =
        let name, value = "name", "value"
        test <| testAttribute name value ((Attribute.createCSS name value) |> Attribute.toString)
