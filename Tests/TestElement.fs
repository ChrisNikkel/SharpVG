namespace SharpVG.Tests

open SharpVG
open Xunit

module TestElement =

    [<Fact>]
    let ``element with name`` () =
        let result = Circle.create Point.origin (Length.ofInt 10) |> Element.createWithName "myCircle" |> Element.toString
        Assert.Contains("id=\"myCircle\"", result)
        Assert.Contains("<circle", result)

    [<Fact>]
    let ``element isNamed is false without name`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        Assert.False(Element.isNamed element)

    [<Fact>]
    let ``element isNamed is true with name`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "c"
        Assert.True(Element.isNamed element)

    [<Fact>]
    let ``element tryGetName returns Some when named`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName "myId"
        Assert.Equal(Some "myId", Element.tryGetName element)

    [<Fact>]
    let ``element tryGetName returns None when unnamed`` () =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.create
        Assert.Equal(None, Element.tryGetName element)

    [<Fact>]
    let ``element with class`` () =
        let result = Rect.create Point.origin Area.full |> Element.createWithClass "highlight" |> Element.toString
        Assert.Contains("class=\"highlight\"", result)

    [<Fact>]
    let ``element withClasses renders first class`` () =
        // SVG attributes are deduplicated by name; only the first class in the sequence is rendered
        let result = Rect.create Point.origin Area.full |> Element.create |> Element.withClasses ["a"; "b"] |> Element.toString
        Assert.Contains("class=\"a\"", result)

    [<Fact>]
    let ``element addClass prepends new class`` () =
        // addClass prepends, so the newest class wins after attribute deduplication
        let result = Rect.create Point.origin Area.full |> Element.createWithClass "base" |> Element.addClass "extra" |> Element.toString
        Assert.Contains("class=\"extra\"", result)

    [<Fact>]
    let ``element withStyle via pipeline`` () =
        let fillColor = Color.ofName Colors.Blue
        let style = Style.createWithFill fillColor
        let result = Rect.create Point.origin Area.full |> Element.create |> Element.withStyle style |> Element.toString
        Assert.Contains("fill=\"blue\"", result)

    [<Fact>]
    let ``element withTransform`` () =
        let translateX = Length.ofInt 50
        let translateY = Length.ofInt 30
        let transform = Transform.createTranslate translateX |> Transform.withY translateY
        let result = Circle.create Point.origin (Length.ofInt 10) |> Element.create |> Element.withTransform transform |> Element.toString
        Assert.Contains("transform=\"translate(50,30)\"", result)

    [<Fact>]
    let ``element withTransforms multiple`` () =
        let translateX = Length.ofInt 10
        let translateY = Length.ofInt 20
        let scaleX = Length.ofInt 2
        let translate = Transform.createTranslate translateX |> Transform.withY translateY
        let scale = Transform.createScale scaleX
        let result = Rect.create Point.origin Area.full |> Element.create |> Element.withTransforms [translate; scale] |> Element.toString
        Assert.Contains("transform=", result)
        Assert.Contains("translate", result)
        Assert.Contains("scale", result)

    [<Fact>]
    let ``element withHref`` () =
        let result = Rect.create Point.origin Area.full |> Element.create |> Element.withHref "#anchor" |> Element.toString
        Assert.Contains("href=\"#anchor\"", result)

    [<Fact>]
    let ``element createWithClasses renders first class`` () =
        let result = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithClasses ["foo"; "bar"] |> Element.toString
        Assert.Contains("class=\"foo\"", result)
