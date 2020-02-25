module public Container

open System
open WrappedString 
[<AbstractClass>]
type public ContainerBase(name: StringFilePath, path:StringFilePath) =
    member val Name = name with get
    member val Path = path with get
    member val ModifiedOn = DateTime.Now with get
    member val CreatedOn = DateTime.Now with get
        
    abstract member  GetSizeInBytes : unit -> int