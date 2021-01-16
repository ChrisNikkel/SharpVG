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
