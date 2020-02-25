module TestFile

open HeyRed.Mime

    type public File(name:string, path:string, data: byte array) = 
        inherit Container.ContainerBase(name, path)        
        member val Data = data with get
        member val MimeType = MimeTypesMap.GetMimeType(name) with get

        override this.GetSizeInBytes() = this.Data.Length

        override this.ToString() =
            let createdOnAsString = this.CreatedOn.ToString("dd.MM.yyyy hh:mm.ss")        
            let fileSizeInBytes = this.GetSizeInBytes().ToString()
            sprintf "%s\t%s\t%sbytes" this.Name createdOnAsString fileSizeInBytes