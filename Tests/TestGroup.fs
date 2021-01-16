namespace SharpVG.Tests

open SharpVG
open Xunit

module TestGroup =

    [<Fact>]
    let ``create group`` () =
        let body = [ Circle.create Point.origin Length.empty |> Element.create ]
        let groupElement = body |> Group.ofList |> Element.createWithName "name"
        Assert.Equal("<g id=\"name\"><circle r=\"0\" cx=\"0\" cy=\"0\"/></g>", groupElement |> Element.toString)

    [<Fact>]
    let ``create empty group and get styles`` () =
        let body = [ ]
        let styles = body |> Group.ofList |> fun grp -> grp.Body |> Body.toStyles
        Assert.Equal("<style type=\"text/css\"><![CDATA[]]></style>", styles |> Styles.toString)
