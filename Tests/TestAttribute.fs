module TestAttribute
open SharpVG
open BasicChecks
open Xunit
open FsCheck
open FsCheck.Xunit
open Swensen.Unquote

let testAttribute (name:string) (value:string) (attribute:string) =
    <@ (attribute.Contains name)
    && (attribute.Contains value)
    && (happensEvenly '"' attribute)
    && (attribute.Contains "=") @>

[<Property>]
let ``create attribute`` =
    let name, value = "name", "value"
    testAttribute name value ((Attribute.create AttributeType.CSS name value) |> Attribute.toString)

[<Property>]
let ``create XML attribute`` =
    let name, value = "name", "value"
    testAttribute name value ((Attribute.createXML name value) |> Attribute.toString)

[<Property>]
let ``create CSS attribute`` =
    let name, value = "name", "value"
    testAttribute name value ((Attribute.createCSS name value) |> Attribute.toString)
