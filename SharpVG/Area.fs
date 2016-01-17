namespace SharpVG

type Area = { Width : Size; Height : Size; }

module AreaHelpers =
    open Helpers
    open SizeHelpers

    let areaToString area =
        "height=" + quote (sizeToString area.Height) + " " +
        "width=" + quote (sizeToString area.Width)