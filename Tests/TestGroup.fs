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

    // Wiki: Group — Creating groups example (tuple API)
    [<Fact>]
    let ``Group wiki - Creating groups example uses tuple API`` () =
        let center = Point.ofInts (50, 50)
        let radius = Length.ofInt 20
        let position = Point.ofInts (10, 10)
        let area = Area.ofInts (40, 30)
        let circle = Circle.create center radius |> Element.create
        let rect = Rect.create position area |> Element.create
        let group = Group.ofList [ circle; rect ]
        let groupElement = group |> Element.create
        let output = groupElement |> Element.toString
        Assert.Contains("<g>", output)
        Assert.Contains("<circle ", output)
        Assert.Contains("r=\"20\"", output)
        Assert.Contains("cx=\"50\"", output)
        Assert.Contains("cy=\"50\"", output)
        Assert.Contains("<rect ", output)
        Assert.Contains("width=\"40\"", output)
        Assert.Contains("height=\"30\"", output)

    // Wiki: Group example
    [<Fact>]
    let ``Group wiki - named group with transform example`` () =
        let center = Point.ofInts (25, 25)
        let radius = Length.ofInt 20
        let position = Point.ofInts (0, 0)
        let area = Area.ofInts (30, 30)
        let circle = Circle.create center radius |> Element.create
        let rect = Rect.create position area |> Element.create
        let translateX = Length.ofInt 100
        let translateY = Length.ofInt 50
        let transform = Transform.createTranslate translateX |> Transform.withY translateY
        let group = [ circle; rect ] |> Group.ofList |> Group.withName "myGroup" |> Group.withTransform transform
        let groupElement = group |> Element.create
        let output = groupElement |> Element.toString
        Assert.Contains("myGroup", output)
        Assert.Contains("transform=\"translate(100,50)\"", output)
        Assert.Contains("<circle ", output)
        Assert.Contains("r=\"20\"", output)
        Assert.Contains("<rect ", output)
        Assert.Contains("width=\"30\"", output)
        Assert.Contains("height=\"30\"", output)
