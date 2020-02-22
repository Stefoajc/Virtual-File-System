module TestContainer

open System

    [<AbstractClass>]
    type public ContainerBase(name:string, path:string) =
        member val Name = name with get
        member val Path = path with get
        member val ModifiedOn = DateTime.Now with get
        member val CreatedOn = DateTime.Now with get

        //member this.Name with get() = name and set nameVal = name <- nameVal
        //member this.Path with get() = path and set pathVal = path <- pathVal
        //member this.ModifiedOn with get() = modifiedOn and set modifierOnVal = modifiedOn <- modifierOnVal

        
        abstract member  GetSizeInBytes : unit -> int

