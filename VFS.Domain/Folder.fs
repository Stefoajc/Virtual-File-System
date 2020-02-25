module Folder
open Container
open File
open WrappedString

    type public Folder(name:StringFilePath, path:StringFilePath, parent: Folder option) = 
        inherit ContainerBase(name, path)    
       
        let rec getFolder (folders:Folder list) (folderName:StringFilePath) :Folder option=
            match folders with
            | [] -> None
            | x::[] -> if x.Name = folderName then Some x else None 
            | x::xs -> if x.Name = folderName then Some x else getFolder xs folderName

        let rec getFile (folders:File list) (fileName:StringFilePath) :File option=
            match folders with
            | [] -> None
            | x::[] -> if x.Name = fileName then Some x else None 
            | x::xs -> if x.Name = fileName then Some x else getFile xs fileName

        let rec addNestedFolder(currentParentFolder: Folder) (folderNames: StringFilePath list) =
            match folderNames with
            | [] -> currentParentFolder
            | x::[] -> 
                let folderToBeCreated = getFolder currentParentFolder.ChildFolders x
                match folderToBeCreated with
                | None -> 
                    let newFolder = Folder(x, StringFilePath (System.IO.Path.Combine((x |> value), (currentParentFolder.Path |> value))), Some currentParentFolder)
                    currentParentFolder.AddFolder(newFolder)
                    newFolder
                | Some f -> f
            | x::xs ->
                let folderToBeCreated = getFolder currentParentFolder.ChildFolders x
                match folderToBeCreated with
                | None -> 
                    let newParentFolder = Folder(x, StringFilePath (System.IO.Path.Combine((x |> value), (currentParentFolder.Path |> value))), Some currentParentFolder)                    
                    currentParentFolder.AddFolder(newParentFolder);
                    addNestedFolder newParentFolder xs
                | Some f -> addNestedFolder f xs
            

        let mutable childFolders:Folder list  = []
        let mutable childFiles:File.File list = []
        member this.Parent = parent
        member this.ChildFiles 
            with get() = childFiles 
        member this.ChildFolders 
            with get() = childFolders 

        member internal this.AddFolder(folder: Folder) =
            childFolders <- childFolders @ [folder]

        member this.AddFolder(folderPath: StringFilePath) =
            let (StringFilePath unwrappedFolderPath) = folderPath
            let folders = 
                unwrappedFolderPath.Split('/', '\\') 
                |> Array.toList 
                |> List.map (fun p -> StringFilePath p )
            let newFolder = addNestedFolder this folders
            ()

        member this.AddFile(fileName: StringFilePath, data: byte array) =            
            let file = File.File(fileName, StringFilePath (System.IO.Path.Combine(this.Path |> value, fileName |> value)), data)
            childFiles <- this.ChildFiles @ [file]
            ()

        member this.GetSubContainers() =
            let foldersAsContainers = this.ChildFolders |> List.map (fun f -> f:>ContainerBase)
            let filesAsContainers = this.ChildFiles |> List.map (fun f -> f:>ContainerBase)
            foldersAsContainers @ filesAsContainers

        member this.GetFolder(folderName: StringFilePath) =
            getFolder this.ChildFolders folderName

        member this.GetFile(fileName: StringFilePath) =
            getFile this.ChildFiles fileName

        override this.GetSizeInBytes() = 0
        
        override this.ToString() =
            let createdOnAsString = this.CreatedOn.ToString("dd.MM.yyyy hh:mm.ss")        
            let fileSizeInBytes = this.GetSizeInBytes().ToString()
            sprintf "%s\t%s\t%sbytes" (this.Name |> value) createdOnAsString fileSizeInBytes