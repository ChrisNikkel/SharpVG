namespace SharpVG

type area =
    {
        Width : length;
        Height : length;
    }

module Area =
    let create width height =
        { Width = width; Height = height }

    let toDescriptiveString area =
        "height=" + Tag.quote (Length.toString area.Height) + " " +
        "width=" + Tag.quote (Length.toString area.Width)

    let toString area =
        Length.toString area.Height + "," + Length.toString area.Width
