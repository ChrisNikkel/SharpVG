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
        let timing = Timing.create (TimeSpan(0, 0, 1))
        let animation = Animation.createTransform timing transform1 transform2
        printf "%s" (Animation.toString animation)
        Assert.Equal("<animateTransform attributeName=\"transform\" attributeType=\"XML\" type=\"matrix\" from=\"3 1 -1 3 30 40\" to=\"40 30 3 -1 1 3\" begin=\"1s\"/>", animation |> Animation.toString)


    [<Fact>]
    let ``create bouncing effect on orangge circle`` () =
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        let style = Style.empty |> Style.withFill (Name Colors.Orange)
        let targetName = "orange-circle"
        let circleWithStyle = circle |> Element.createWithName targetName |> Element.withStyle style
        let timing = Timing.create (TimeSpan(0, 0, 0, 1)) |> Timing.withDuration (TimeSpan(0, 0, 3))
        let circleAnimation = Animation.createAnimation timing AttributeType.XML "cy" "50" "250" |> Element.create |> Element.withHref ("#" + targetName)

        Assert.Equal("<circle id=\"orange-circle\" fill=\"orange\" r=\"30\" cx=\"50\" cy=\"50\"/>", circleWithStyle |> Element.toString)
        Assert.Equal("<animate href=\"#orange-circle\" attributeName=\"cy\" attributeType=\"XML\" from=\"50\" to=\"250\" begin=\"1s\" dur=\"3s\"/>", circleAnimation |> Element.toString)
