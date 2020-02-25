module FileNavigator     
open Folder
open WrappedString

    type Navigator() = 
        let rec findFolder (currentFolder:Folder) (folderNames:StringFilePath list) :Folder option=
            match folderNames with
            | [] -> None
            | x::[] -> 
                currentFolder.GetFolder(x)                
            | x::xs -> 
                let folderToEnter = currentFolder.GetFolder(x)
                match folderToEnter with
                | Some folder -> findFolder folder xs
                | None -> None

        
        let mutable currentFolder = Folder.Folder(StringFilePath "root", StringFilePath "root", None)
        let root = currentFolder

        member this.CurrentFolder 
            with get() = currentFolder 
            and private set(value) = currentFolder <- value
        

        member public this.EnterFolder(path:StringFilePath) :unit =
            let (StringFilePath folderPath) = path
            let folders = 
                folderPath.Split('/', '\\') 
                |> Array.toList 
                |> List.map (fun p -> StringFilePath p )
                

            let endFolder = findFolder currentFolder folders
            match endFolder with
            | Some folder -> 
                currentFolder <- folder
            | None -> ()
            
        member public this.Back() =
            match currentFolder.Parent with
            | Some parentFolder -> currentFolder <- parentFolder
            | None -> ()

        member public this.BackToRoot() =
            currentFolder <- root
