namespace SharpVG

type ElementId = string

type HRef =
    | IdRef of ElementId
    | UrlRef of string

module HRef =
    let ofId (id: ElementId) : HRef = IdRef id
    let ofUrl (url: string) : HRef = UrlRef url
    let toString href =
        match href with
        | IdRef id -> "#" + id
        | UrlRef url -> url