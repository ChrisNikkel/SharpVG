namespace SharpVG

module Core =

    let html title body =
        "<!DOCTYPE html>\n<html>\n<head>\n  <title>" +
        title +
        "</title>\n</head>\n<body>\n" +
        body +
        "</body>\n</html>\n"

    // TODO: 
    let style =
        "<style>
        circle {
        stroke: #006600;
        fill  : #00cc00;
        }
        circle.allBlack {
        stroke: #000000;
        fill  : #000000;
        }\n" +
        "</style>\n"

    // TODO: Make object and allow things lie fromSeq, fromList, etc.
    let svg size body =
        "<svg " + Area.toString size + ">\n  " +
        body +
        "\n</svg>\n"

    let group id transform point body =
        "<g id=" + Tag.quote id +
        Transform.toString transform +
        Point.toString point + ">" +
        body +
        "</g>"