namespace SharpVG

type Area =
    {
        Width : Length;
        Height : Length;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Area =
    let create width height =
        { Width = width; Height = height }

    let toDescriptiveString area =
        "height=" + Tag.quote (Length.toString area.Height) + " " +
        "width=" + Tag.quote (Length.toString area.Width)

    let toString area =
        Length.toString area.Height + "," + Length.toString area.Width
