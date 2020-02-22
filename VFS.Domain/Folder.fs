module Folder

    type public Folder(name:string, path:string, parent: Folder option) = 
        inherit Container.ContainerBase(name, path)    
       
        member val Parent = parent with get
        member val ChildFiles :File.File list = [] with get
        member val ChildFolders :Folder list = [] with get

        override this.GetSizeInBytes() = 0
        
        override this.ToString() =
            let createdOnAsString = this.CreatedOn.ToString("dd.MM.yyyy hh:mm.ss")        
            let fileSizeInBytes = this.GetSizeInBytes().ToString()
            sprintf "%s\t%s\t%sbytes" this.Name createdOnAsString fileSizeInBytes