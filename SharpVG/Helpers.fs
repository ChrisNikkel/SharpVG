namespace SharpVG

module Helpers =
    let quoter = "\""
    let inline quote i =
        quoter + string i + quoter

    let addSpace needsSpace =
        (if needsSpace then " " else "")

    let emptyTagToString name attribute = "<" + name + " " + attribute + "/>"

    let tagToString name attribute body = "<" + name + " " + attribute + ">" + body + "</" + name + ">"

    let addSpaces strings = (strings |> Seq.reduce (fun acc str -> acc + " " + str)).TrimStart()
