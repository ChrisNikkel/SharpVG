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
    let ``create bouncing effect on orange circle`` () =
        let circle = Circle.create (Point.ofInts (50, 50)) (Length.ofInt 30)
        let style = Style.empty |> Style.withFill (Name Colors.Orange)
        let targetName = "orange-circle"
        let circleWithStyle = circle |> Element.createWithName targetName |> Element.withStyle style
        let timing = Timing.create (TimeSpan(0, 0, 0, 1)) |> Timing.withDuration (TimeSpan(0, 0, 3))
        let circleAnimation = Animation.createAnimation timing AttributeType.XML "cy" "50" "250" |> Element.create |> Element.withHref ("#" + targetName)

        Assert.Equal("<circle id=\"orange-circle\" fill=\"orange\" r=\"30\" cx=\"50\" cy=\"50\"/>", circleWithStyle |> Element.toString)
        Assert.Equal("<animate href=\"#orange-circle\" attributeName=\"cy\" attributeType=\"XML\" from=\"50\" to=\"250\" begin=\"1s\" dur=\"3s\"/>", circleAnimation |> Element.toString)

    (*
    <svg viewBox="0 0 120 120" xmlns="http://www.w3.org/2000/svg">
        <circle cx="60" cy="10" r="10">
            <animate attributeName="cx" dur="4s" repeatCount="indefinite"
                values="60 ; 110 ; 60 ; 10 ; 60" keyTimes="0 ; 0.25 ; 0.5 ; 0.75 ; 1"/>
            <animate attributeName="cy" dur="4s" repeatCount="indefinite"
                values="10 ; 60 ; 110 ; 60 ; 10" keyTimes="0 ; 0.25 ; 0.5 ; 0.75 ; 1"/>
        </circle>
    </svg>
    *)

    [<Fact>]
    let ``create animation with list of key times`` () =
        let circle = Circle.create (Point.ofInts (60, 10)) (Length.ofInt 10)
        let repetition =  { RepeatCount = RepeatCountValue.Indefinite ; RepeatDuration = None }  // TODO: Create helper for Repetition to avoid need to create records
        let timing = Timing.create (TimeSpan(0, 0, 0, 0)) |> Timing.withRepetition repetition |> Timing.withDuration (TimeSpan(0, 0, 0, 4))
        let animation = Animation.createAnimation timing AttributeType.XML "cx" "60" "110" |> Animation.withKeyTimes [ 0.0; 0.25; 0.5; 0.75; 1.0 ]
        let circleElementWithAnimation = circle |> Element.create |> (Element.withAnimation animation)

        Assert.Equal("<circle r=\"10\" cx=\"60\" cy=\"10\"><animate attributeName=\"cx\" attributeType=\"XML\" from=\"60\" to=\"110\" keyTimes=\"0;0.25;0.5;0.75;1\" repeatCount=\"indefinite\" begin=\"0s\" dur=\"4s\"/></circle>", circleElementWithAnimation |> Element.toString)
