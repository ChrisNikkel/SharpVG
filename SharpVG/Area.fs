namespace SharpVG

type area =
    {
        width : length;
        height : length;
    }

module Area =

    let toDescriptiveString area =
        "height=" + Tag.quote (Length.toString area.height) + " " +
        "width=" + Tag.quote (Length.toString area.width)

    let toString area =
        Length.toString area.height + "," + Length.toString area.width
