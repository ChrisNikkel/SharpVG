namespace SharpVG.Tests

open System
open SharpVG
open Xunit
open FsCheck
open FsCheck.Xunit
open BasicChecks

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
    let ``element withClasses renders all classes space-separated`` () =
        let result = Rect.create Point.origin Area.full |> Element.create |> Element.withClasses ["a"; "b"] |> Element.toString
        Assert.Contains("class=\"a b\"", result)

    [<Fact>]
    let ``element addClass prepends class to existing`` () =
        let result = Rect.create Point.origin Area.full |> Element.createWithClass "base" |> Element.addClass "extra" |> Element.toString
        Assert.Contains("class=\"extra base\"", result)

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
    let ``element createWithClasses renders all classes space-separated`` () =
        let result = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithClasses ["foo"; "bar"] |> Element.toString
        Assert.Contains("class=\"foo bar\"", result)

    [<Fact>]
    let ``element withAnimation adds animation child`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let animation = Animation.createAnimation timing AttributeType.XML "fill" "red" "blue"
        let result = Circle.create Point.origin (Length.ofInt 10) |> Element.create |> Element.withAnimation animation |> Element.toString
        Assert.Contains("<animate", result)
        Assert.Contains("attributeName=\"fill\"", result)

    [<Fact>]
    let ``element addAnimation appends to existing animations`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let animation1 = Animation.createAnimation timing AttributeType.XML "fill" "red" "blue"
        let animation2 = Animation.createAnimation timing AttributeType.XML "r" "5" "20"
        let result =
            Circle.create Point.origin (Length.ofInt 10)
            |> Element.create
            |> Element.withAnimation animation1
            |> Element.addAnimation animation2
            |> Element.toString
        Assert.Contains("attributeName=\"fill\"", result)
        Assert.Contains("attributeName=\"r\"", result)

    [<Fact>]
    let ``element withAnimations replaces animations`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 0.0) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let animation1 = Animation.createAnimation timing AttributeType.XML "fill" "red" "blue"
        let animation2 = Animation.createAnimation timing AttributeType.XML "r" "5" "20"
        let result =
            Circle.create Point.origin (Length.ofInt 10)
            |> Element.create
            |> Element.withAnimations [animation1; animation2]
            |> Element.toString
        Assert.Contains("attributeName=\"fill\"", result)
        Assert.Contains("attributeName=\"r\"", result)

    [<Fact>]
    let ``element setTo creates set animations for attribute differences`` () =
        let timing = Timing.create (TimeSpan.FromSeconds 1.0)
        let original = Circle.create Point.origin (Length.ofInt 10) |> Element.create
        let target = Circle.create Point.origin (Length.ofInt 30) |> Element.create
        let result = Element.setTo timing target original |> Element.toString
        Assert.Contains("<set", result)

    [<Fact>]
    let ``element createFull sets all fields`` () =
        let fillColor = Color.ofName Colors.Green
        let style = Style.createWithFill fillColor
        let translateX = Length.ofInt 10
        let transform = Transform.createTranslate translateX
        let result =
            Circle.create Point.origin (Length.ofInt 5)
            |> Element.createFull (Some "fullEl") ["cls"] (Some style) (Seq.singleton transform) Seq.empty
            |> Element.toString
        Assert.Contains("id=\"fullEl\"", result)
        Assert.Contains("class=\"cls\"", result)
        Assert.Contains("fill=\"green\"", result)
        Assert.Contains("transform=", result)

    [<SvgIdProperty>]
    let ``element with any safe name always contains that id`` (name: string) =
        let result = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName name |> Element.toString
        result.Contains(sprintf "id=\"%s\"" name)

    [<SvgIdProperty>]
    let ``element isNamed is always true after withName`` (name: string) =
        let element = Rect.create Point.origin Area.full |> Element.create |> Element.withName name
        Element.isNamed element

    [<SvgIdProperty>]
    let ``element tryGetName always returns Some with the given name`` (name: string) =
        let element = Circle.create Point.origin (Length.ofInt 5) |> Element.createWithName name
        Element.tryGetName element = Some name

    [<SvgProperty>]
    let ``element with style and transform always produces bodyless tag`` (x: float, y: float, r: float, r2, g, b) =
        let translateX = Length.ofFloat x
        let translateY = Length.ofFloat y
        let transform = Transform.createTranslate translateX |> Transform.withY translateY
        let style = Style.createWithFill (Color.ofValues (r2, g, b))
        let result =
            Circle.create Point.origin (Length.ofFloat r)
            |> Element.createWithStyle style
            |> Element.withTransform transform
            |> Element.toString
        checkBodylessTag "circle" result

    // Editing API — attribute helpers
    [<Fact>]
    let ``getAttribute returns Some for existing attribute`` () =
        let center = Point.ofInts (10, 10)
        let radius = Length.ofInt 5
        let element = Circle.create center radius |> Element.create
        Assert.Equal(Some "10", Element.getAttribute "cx" element)

    [<Fact>]
    let ``getAttribute returns None for missing attribute`` () =
        let center = Point.ofInts (10, 10)
        let radius = Length.ofInt 5
        let element = Circle.create center radius |> Element.create
        Assert.Equal(None, Element.getAttribute "nonexistent" element)

    [<Fact>]
    let ``withAttribute adds new attribute visible in toString`` () =
        let center = Point.ofInts (0, 0)
        let radius = Length.ofInt 5
        let element = Circle.create center radius |> Element.create |> Element.withAttribute "data-foo" "bar"
        Assert.Contains("data-foo=\"bar\"", Element.toString element)

    [<Fact>]
    let ``withAttribute updates existing attribute`` () =
        let center = Point.ofInts (10, 10)
        let radius = Length.ofInt 5
        let element =
            Circle.create center radius
            |> Element.create
            |> Element.withAttribute "r" "99"
        let output = Element.toString element
        Assert.Contains("r=\"99\"", output)
        Assert.DoesNotContain("r=\"5\"", output)

    [<Fact>]
    let ``removeAttribute removes the attribute from output`` () =
        let center = Point.ofInts (10, 10)
        let radius = Length.ofInt 5
        let element =
            Circle.create center radius
            |> Element.create
            |> Element.removeAttribute "cx"
        Assert.DoesNotContain("cx=", Element.toString element)

    // Editing API — animation helpers
    [<Fact>]
    let ``clearAnimations removes all animations`` () =
        let timing = Timing.create (TimeSpan.Zero) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let anim = Animation.createSet timing AttributeType.XML "r" "10"
        let element =
            Circle.create Point.origin (Length.ofInt 5)
            |> Element.create
            |> Element.withAnimation anim
            |> Element.clearAnimations
        Assert.DoesNotContain("<set", Element.toString element)

    [<Fact>]
    let ``removeAnimationWhere removes only matching animations`` () =
        let timing = Timing.create (TimeSpan.Zero) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let anim1 = Animation.createSet timing AttributeType.XML "r" "10"
        let anim2 = Animation.createSet timing AttributeType.XML "cx" "20"
        let element =
            Circle.create Point.origin (Length.ofInt 5)
            |> Element.create
            |> Element.withAnimation anim1
            |> Element.addAnimation anim2
            |> Element.removeAnimationWhere (fun a ->
                match a.AnimationType with
                | Set s -> s.AttributeName = "r"
                | _ -> false)
        let output = Element.toString element
        Assert.DoesNotContain("attributeName=\"r\"", output)
        Assert.Contains("attributeName=\"cx\"", output)

    [<Fact>]
    let ``mapAnimations transforms each animation`` () =
        let timing = Timing.create (TimeSpan.Zero) |> Timing.withDuration (TimeSpan.FromSeconds 1.0)
        let anim = Animation.createSet timing AttributeType.XML "r" "10"
        let element =
            Circle.create Point.origin (Length.ofInt 5)
            |> Element.create
            |> Element.withAnimation anim
            |> Element.mapAnimations (fun a -> { a with Additive = Some Additive.Sum })
        Assert.Contains("additive=\"sum\"", Element.toString element)
