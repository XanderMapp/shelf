namespace Shelf
{
    public class Splitter : Turtle
    {
        public File? CreateFileSplice(File aFile, string whichWay)
        {
            string readFileText, properFileText;
            int lengthOfSlice;
            long newFileSize;
            File? tempFile = null;

            readFileText = DocumentDataRead(aFile);
            lengthOfSlice = (readFileText.Length / 2);

            switch (whichWay)
            {
                case "UP":
                    properFileText = readFileText[..lengthOfSlice];
                    newFileSize = properFileText.Length * sizeof(char);
                    tempFile = new File($"split-up-file-{DateTime.Now}",
                        "C:\\Users\\$USER\\shelf-output",
                        ".txt",
                        properFileText,
                        DateTime.Now,
                        newFileSize);
                    break;
                case "DOWN":
                    properFileText = readFileText[lengthOfSlice..];
                    newFileSize = properFileText.Length * sizeof(char);
                    tempFile = new File($"split-down-file-{DateTime.Now}",
                        "C:\\Users\\$USER\\shelf-output",
                        ".txt",
                        properFileText,
                        DateTime.Now,
                        newFileSize);
                    break;
            }

            return tempFile;
        }

        public Folder? CreateFolderSplice(Folder aFolder, string whichWay)
        {
            DirectoryInfo? tempDirectoryInfo = null;
            int lengthOfSlice = 0;
            Folder? tempFolder = null;
            string[] filesToAdd; //limit amount of files that can be split in a folder

            if (aFolder.Path != null)
            {
                tempDirectoryInfo = new DirectoryInfo(aFolder.Path); 
                lengthOfSlice = (tempDirectoryInfo.GetDirectories().Length / 2);

                switch (whichWay)
                {
                    case "UP":
                        filesToAdd = Directory.GetFiles(aFolder.Path);
                        filesToAdd = filesToAdd[..lengthOfSlice];
                        tempFolder = new Folder($"split-up-folder-{DateTime.Now}", "C:\\Users\\$USER\\shelf-output",
                            "DIR",
                            filesToAdd, DateTime.Now);
                        break;
                    case "DOWN":
                        filesToAdd = Directory.GetFiles(aFolder.Path);
                        filesToAdd = filesToAdd[..lengthOfSlice];
                        tempFolder = new Folder($"split-down-folder-{DateTime.Now}", "C:\\Users\\$USER\\shelf-output",
                            "DIR", filesToAdd, DateTime.Now);
                        break;
                }
            }
            return tempFolder;
        }
    }
}