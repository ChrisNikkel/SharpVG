namespace SharpVG

module Helpers =
    let quoter = "\""
    let inline quote i =
        quoter + string i + quoter

    let addSpace needsSpace =
        (if needsSpace then " " else "")