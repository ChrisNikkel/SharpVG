module BasicChecks

open LogHelpers
open Swensen.Unquote

let isTagEnclosed (tag:string) =
    let trimmedTag = tag.Trim()
    trimmedTag.[0..0] = "<" && trimmedTag.[(trimmedTag.Length - 2)..(trimmedTag.Length - 1)] = "/>"

let happensEvenly chr (str:string) =
    str.ToCharArray()
    |> Array.fold (fun acc c -> if chr = c then acc + 1 else acc) 0
    |> (fun x -> x % 2 = 0)

let isDepthNoMoreThanOne left right (str:string) =
    str.ToCharArray()
    |> Array.scan
        (fun acc c ->
            match c with
                | c when c = left -> acc + 1
                | c when c = right -> acc - 1
                | _ -> acc
        ) 0
    |> Array.max
    |> (>=) 1

let isMatched left right (str:string) =
    str.ToCharArray()
    |> Array.fold

        (fun acc c ->
            match c with
                | c when c = left -> acc + 1
                | c when c = right -> acc - 1
                | _ -> acc
        ) 0 = 0

let checkBodylessTag name tag =
    <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains name)
    && (isTagEnclosed tag) @>

let checkTag name tag =
    <@ (isMatched '<' '>' tag)
    && (isDepthNoMoreThanOne '<' '>' tag)
    && (happensEvenly '"' tag)
    && (happensEvenly ''' tag)
    && (tag.Contains name) @>
