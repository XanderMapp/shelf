namespace Shelf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string? mutators, path;
                char alterType;
                char[] properMutators = new char[5];
                bool userHasQuit = false;
                
                //Instantiate a new ShelfParser
                Parser shelfParser = new Parser();
                
                while (!userHasQuit)
                {
                    alterType = GetValidChar("What would you like to alter:" +
                                                "(A) - Files" +
                                                "(B) - Folders ");
                    switch (alterType)
                    {
                        case 'A':
                            //Creates two new files for storing info into.
                            File[] filesToMutate = new File[2];
                            
                            Console.Write("What is the path to your files? ");
                            path = Console.ReadLine();
                            if (path != null)
                            {
                                MakeFileListFromDirectory(path);
                            }
                            else
                            {
                                throw new DirectoryNotFoundException();
                            }

                            break;
                        case 'B' :
                            //Creates two new folders for storing info into.
                            Folder[] foldersToMutate = new Folder[2];
                            
                            Console.Write("What is the path to your folders? ");
                            path = Console.ReadLine();
                            if (path != null)
                            {
                               foldersToMutate = MakeFolderListFromDirectory(path);
                            }
                            else
                            {
                                throw new DirectoryNotFoundException();
                            }
                            break;
                    }
                    
                    Console.Write("Enter File Equation (5 operands max!): ");
                    mutators = Console.ReadLine();
                    if (mutators != null)
                    {
                        properMutators = mutators.ToCharArray();
                    }
                    shelfParser.Parse(properMutators);
                    
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static char GetValidChar(string prompt)
        {
            string userInput;
            char validChar;

            Console.Write(prompt);
            userInput = Console.ReadLine();

            while (!char.TryParse(userInput, out validChar))
            {
                Console.Write($"{userInput} is not a valid character! {prompt} ");
                userInput = Console.ReadLine();
            }
            return validChar;
        }
        static void GetNames()
        {
            string file1, file2;
        }
        static List<File> MakeFileListFromDirectory(string path)
        {
            File tempFile;
            FileInfo tempFileInfo;
            List<File> files = new List<File>();
            string[] fileNames;
            
            fileNames = Directory.GetFiles(path);

            foreach (string aFileName in fileNames)
            {
                //get file info
                tempFileInfo = new FileInfo(aFileName);
                
                //Creation of new file, with info encoded into it.
                tempFile = new File(tempFileInfo.Name,tempFileInfo.DirectoryName,tempFileInfo.Extension,tempFileInfo.LastWriteTime,tempFileInfo.Length);
                
                files.Add(tempFile);
            }
            return files;
        }
        static List<Folder> MakeFolderListFromDirectory(string path)
        {
            Folder tempFolder;
            DirectoryInfo tempFolderInfo;
            List<Folder> folders = new List<Folder>();
            string[] folderNames;
            
            folderNames = Directory.GetDirectories(path);

            foreach (string aFolderName in folderNames)
            {
                //get folder info
                tempFolderInfo = new DirectoryInfo(aFolderName);
                
                //Creation of new folder, with info encoded into it.
                tempFolder = new Folder(tempFolderInfo.Name,tempFolderInfo.FullName,tempFolderInfo.Extension,tempFolderInfo.LastWriteTime);
                
                folders.Add(tempFolder);
            }
            return folders;
        }
    }
}
