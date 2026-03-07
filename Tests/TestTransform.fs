namespace SharpVG.Tests

open SharpVG
open Xunit
open FsCheck.Xunit

module TestTransform =
    [<Property>]
    let ``create matrix transforms`` (a, b, c, d, e, f) =
        let lengthA, lengthB, lengthC, lengthD, lengthE, lengthF = Length.ofInt a, Length.ofInt b, Length.ofInt c, Length.ofInt d, Length.ofInt e, Length.ofInt f
        let transform = Transform.createMatrix (lengthA, lengthB, lengthC, lengthD, lengthE, lengthF) |> Transform.toString
        Assert.StartsWith("matrix(", transform)
        Assert.DoesNotContain(" ", transform)
        Assert.Contains(",", transform)
        Assert.EndsWith(")", transform)

    [<Fact>]
    let ``create a few transforms`` () =
        let lengthA, lengthB, lengthC, lengthD, lengthE, lengthF = Length.ofInt 3, Length.ofInt 1, Length.ofInt -1, Length.ofInt 3, Length.ofInt 30, Length.ofInt 40
        let matrixTransform = Transform.createMatrix (lengthA, lengthB, lengthC, lengthD, lengthE, lengthF)
        let translateTransform = Transform.createTranslate lengthA
        let scaleTransform = Transform.createScale lengthE
        let transforms = seq { matrixTransform; translateTransform; scaleTransform }
        let transformsString = transforms |> Transforms.toAttribute |> Attribute.toString
        Assert.Equal("transform=\"matrix(3,1,-1,3,30,40) translate(3) scale(30)\"", transformsString)

    // Wiki: Transform example
    [<Fact>]
    let ``Transform wiki - group with transform example`` () =
        let center = Point.ofInts (25, 25)
        let radius = Length.ofInt 20
        let position = Point.ofInts (0, 0)
        let area = Area.ofInts (30, 30)
        let circle = Circle.create center radius |> Element.create
        let rect = Rect.create position area |> Element.create
        let translateX = Length.ofInt 100
        let translateY = Length.ofInt 50
        let transform = Transform.createTranslate translateX |> Transform.withY translateY
        let group = [ circle; rect ] |> Group.ofList |> Group.withTransform transform
        let groupElement = group |> Element.create
        let output = groupElement |> Element.toString
        Assert.Contains("<g ", output)
        Assert.Contains("transform=\"translate(100,50)\"", output)
        Assert.Contains("<circle ", output)
        Assert.Contains("<rect ", output)
