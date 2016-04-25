namespace SharpVG

type area =
    {
        width : size;
        height : size;
    }

module Area =
    let toString area =
        "height=" + Tag.quote (Size.toString area.height) + " " +
        "width=" + Tag.quote (Size.toString area.width)