open System
open FileNavigator
open WrappedString

let rec loop (navigator: FileNavigator.Navigator) :unit =
    printf "%s>" (navigator.CurrentFolder.Path |> value)
    let consoleInput = Console.ReadLine()
    if(consoleInput = "exit") then ()
    let command = Helpers.getFirstWord(consoleInput)
    let arguments = Helpers.removeFirstWord(consoleInput)

    match command with
    | "info" -> 
        printfn "info command" 
        loop(navigator)
    | "dir" -> 
        for folder in navigator.CurrentFolder.GetSubContainers() do
            printfn "%s" (folder.ToString())
        loop(navigator)
    | "cd" ->
        match arguments with
        | ".." -> navigator.Back()
        | "/" -> navigator.BackToRoot()        
        | "" -> printfn "not recognised command"
        | path -> 
            let wrappedPath = stringOneWord path
            match wrappedPath with
            | Some p -> navigator.EnterFolder(p)
            | None -> printfn "cd argument should be name/path to folder"  
        loop(navigator)
    | "mkdir" -> 
        let wrappedFolderName = stringOneWord arguments
        match wrappedFolderName with
        | Some f -> navigator.CurrentFolder.AddFolder(f)
        | None -> printfn "mkdir argument should be one word"        
        loop(navigator)
    | "exit" -> ()
    | _ -> 
        printfn "unrecognised command"
        loop(navigator)

[<EntryPoint>]
let main argv =
    let navigator = new Navigator()    
    loop(navigator)
    
    0 // return an integer exit code


