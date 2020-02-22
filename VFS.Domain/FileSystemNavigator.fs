module FileSystemNavigator

    type Navigator() = 
        let root :Folder.Folder = default
        let currentFolder :obj = new System.Object()

