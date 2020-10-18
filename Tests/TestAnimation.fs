namespace SharpVG.Tests

open SharpVG
open System
open Xunit
open FsCheck.Xunit

module TestAnimation =
    [<Fact>]
    let ``create transform animation`` () =
        let lengthA, lengthB, lengthC, lengthD, lengthE, lengthF = Length.ofInt 3, Length.ofInt 1, Length.ofInt -1, Length.ofInt 3, Length.ofInt 30, Length.ofInt 40
        let transform1 = Transform.createMatrix (lengthA, lengthB, lengthC, lengthD, lengthE, lengthF)
        let transform2 = Transform.createMatrix (lengthF, lengthE, lengthD, lengthC, lengthB, lengthA)
        let timing = Timing.create (TimeSpan(1L))
        let animation = Animation.createTransform timing transform1 transform2
        printf "%s" (Animation.toString animation)
        Assert.Equal("<animateTransform attributeName=\"transform\" attributeType=\"XML\" type=\"matrix\" from=\"3 1 -1 3 30 40\" to=\"40 30 3 -1 1 3\" begin=\"1E-07s\"/>", animation |> Animation.toString)
