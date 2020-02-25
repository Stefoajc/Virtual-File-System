module Helpers

    let getFirstWord (text:string) =
        let words = text.Split(" ")
        match Array.tryHead words with
        | Some word -> word
        | None -> ""

    let removeFirstWord (text:string) =
        let words = text.Split(" ")
        match words.[1..] |> Array.toList with
        | [] -> ""
        | x -> x |> String.concat " "
