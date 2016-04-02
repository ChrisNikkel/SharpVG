module BasicChecks

let isTagEnclosed (tag:string) =
    let trimmedTag = tag.Trim()
    trimmedTag.[0..0] = "<" && trimmedTag.[(trimmedTag.Length - 2)..(trimmedTag.Length - 1)] = "/>"

let happensEvenly chr (str:string) =
    str.ToCharArray()
    |> Array.fold
        (fun acc c -> if chr = c then acc + 1 else acc) 0
    |> (%) 2 = 0

let isMatched left right (str:string) =
    str.ToCharArray()
    |> Array.fold
        (fun acc c ->
            match c with
                | c when c = left -> acc + 1
                | c when c = right -> acc - 1
                | _ -> acc
        ) 0 = 0