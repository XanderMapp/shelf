using Microsoft.Data.Sqlite;

namespace Shelf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SqliteConnection? dbConn;
            bool userHasQuit = false;
            while (!userHasQuit)
            {
                try
                {
                    char[] properMutators = new char[5];

                    //Upon start of program, do three things
                    //1. Load the dictionary of actions to be used.
                    Dictionary<char, string> mutatorActions = new Dictionary<char, string>();
                    LoadActions(mutatorActions);

                    //2. Create a connection to the SQLite database. (the shelfdata.sqlite file.)
                    dbConn = CreateConnection();

                    //3. Instantiate a new ShelfParser
                    Parser shelfParser = new Parser();

                    Console.Write("What would you like to do? " +
                                  "\n(A) - New Directory" +
                                  "\n(B) - Directories in Database" +
                                  "\n(C) - Equation entry" +
                                  "\n(Q) - Quit\n");
                    var alterType = GetValidChar("What would you like to do? ");
                    switch (char.ToUpper(alterType))
                    {
                        case 'A':
                        {
                            Console.Write("What is the path to the folder you wish to upload? ");
                            string? path = Console.ReadLine();

                            if (path != null)
                            {
                                List<File> newDirListFiles = MakeFileListFromDirectory(path);
                                List<Folder> newDirListFolders = MakeFolderListFromDirectory(path);
                                InsertBulkData(dbConn, newDirListFiles, newDirListFolders);
                            }
                            dbConn.Close();
                            break;
                        }
                        case 'B':
                        {
                            ReadData(dbConn);
                            dbConn.Close();
                            break;
                        }
                        case 'C':
                        {
                            string? filePath1, filePath2, dataQuery1, dataQuery2, mutators;
                            File? file1 = null, file2 = null;
                            Folder? folder1 = null;
                            int option;
                            
                            Console.Write("(1) - Single File" +
                                          "\n(2) - Two Files" +
                                          "\n(3) - Folder\n");
                            option = GetValidInt("Which would you like to work with: ");

                            switch (option)
                            {
                                case 1:
                                    Console.Write("Enter First File Path: ");
                                    filePath1 = Console.ReadLine();
                                    dataQuery1 = $"SELECT FileID, Name, Path, Type, Content, LastEdited, Size FROM Files WHERE Path = '+{filePath1}+'";

                                    file1 = FileFromQuery(dbConn, dataQuery1);
                                    
                                    Console.Write("Enter File Equation (5 operands max!): ");
                                    mutators = Console.ReadLine();

                                    if (mutators != null && file1 != null && file2 != null)
                                    {
                                        properMutators = mutators.ToCharArray();
                                        shelfParser.Parse(dbConn, properMutators, mutatorActions, file1);
                                    }
                                    
                                    break;
                                case 2:
                                    Console.Write("Enter First File Path: ");
                                    filePath1 = Console.ReadLine();
                                    dataQuery1 = $"SELECT FileID, Name, Path, Type, Content, LastEdited, Size FROM Files WHERE Path = {filePath1}";
                                    
                                    file1 = FileFromQuery(dbConn, dataQuery1);
                                    
                                    Console.Write("Enter Second File Path: ");
                                    filePath2 = Console.ReadLine();
                                    dataQuery2 = $"SELECT FileID, Name, Path, Type, Content, LastEdited, Size FROM Files WHERE Path = {filePath2}";
                                    
                                    file2 = FileFromQuery(dbConn, dataQuery2);
                                    
                                    Console.Write("Enter File Equation (5 operands max!): ");
                                    mutators = Console.ReadLine();
                                    
                                    if (mutators != null && file1 != null && file2 != null)
                                    {
                                        properMutators = mutators.ToCharArray();
                                        shelfParser.Parse(dbConn, properMutators, mutatorActions, file1, file2);
                                    }
                                    break;
                                case 3:
                                    Console.Write("Enter Path to folder: ");
                                    filePath1 = Console.ReadLine();
                                    dataQuery1 = $"SELECT FolderID, Name, Path, Type, Content, LastEdited, Size FROM Files WHERE Path = {filePath1}";
                                    
                                    folder1 = FolderFromQuery(dbConn,dataQuery1);
                                    
                                    Console.Write("Enter File Equation (5 operands max!): ");
                                    mutators = Console.ReadLine();
                                    
                                    if (mutators != null && file1 != null && file2 != null)
                                    {
                                        properMutators = mutators.ToCharArray();
                                        shelfParser.Parse(dbConn, properMutators, mutatorActions, folder1);
                                    }
                                    break;
                            }
                            break;
                        }
                        case 'Q':
                        {
                            userHasQuit = true;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        public static char GetValidChar(string prompt)
        {
            string? userInput;
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
        public static int GetValidInt(string prompt)
        {
            string? userInput;
            int validInt;

            Console.Write(prompt);
            userInput = Console.ReadLine();

            while (!int.TryParse(userInput, out validInt))
            {
                Console.Write($"{userInput} is not a valid integer! {prompt} ");
                userInput = Console.ReadLine();
            }
            return validInt;
        }

        public static List<File> MakeFileListFromDirectory(string path)
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
                tempFile = new File(tempFileInfo.Name,tempFileInfo.DirectoryName,tempFileInfo.Extension,tempFileInfo.OpenText().ToString(),tempFileInfo.LastWriteTime,tempFileInfo.Length);

                files.Add(tempFile);
            }

            return files;
        }

        public static List<Folder> MakeFolderListFromDirectory(string path)
        {
            Folder tempFolder;
            DirectoryInfo tempFolderInfo;
            List<Folder> folders = new List<Folder>();
            string[] folderNames, fileNames;

            folderNames = Directory.GetDirectories(path);
            fileNames = Directory.GetFiles(path);

            var combinedToAdd = folderNames.Concat(fileNames).ToArray();

            foreach (string aFolderName in folderNames)
            {
                //Get folder info
                tempFolderInfo = new DirectoryInfo(aFolderName);

                //Creation of new folder, with info encoded into it.
                tempFolder = new Folder(tempFolderInfo.Name, tempFolderInfo.FullName, tempFolderInfo.Extension,combinedToAdd,tempFolderInfo.LastWriteTime);

                folders.Add(tempFolder);
            }

            return folders;
        }

        public static SqliteConnection CreateConnection()
        {
            SqliteConnection dbConnection;
            dbConnection = new SqliteConnection(@"Data Source=C:\Users\xande\source\repos\shelf\shelfdata.sqlite");
            try
            {
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Creating Connection: {ex.Message}");
            }

            return dbConnection;
        }

       public static void InsertBulkData(SqliteConnection connection, List<File> filesToWrite, List<Folder> foldersToWrite)
        {
            SqliteCommand dbCommand;
            dbCommand = connection.CreateCommand();

            foreach (File aFile in filesToWrite)
            {
                dbCommand.CommandText = $"INSERT INTO Files Name,Path,Type,Content,LastEdited,Size VALUES '+{aFile.Name}+','+{aFile.Path}+','+{aFile.Type}+','+{aFile.Content}+','+{aFile.LastEdited}+',{aFile.Size};";
                dbCommand.ExecuteNonQuery();
            }

            foreach (Folder aFolder in foldersToWrite)
            {
                dbCommand.CommandText = $"INSERT INTO Folders Name,Path,Type,Subcontent,LastEdited,Size VALUES '+{aFolder.Name}+','+{aFolder.Path}+','+{aFolder.Type}+','+{aFolder.Content}+','+{aFolder.LastEdited}+',{aFolder.Size};";
                dbCommand.ExecuteNonQuery();
            }
        }
        public static void ReadData(SqliteConnection connection)
        {
            SqliteDataReader dbDatareader;
            SqliteCommand dbCommand;
            string toBuild;
            
            dbCommand = connection.CreateCommand();

            dbCommand.CommandText = "SELECT FileID,Name,Path,Type,Content,LastEdited,Size FROM Files UNION SELECT FolderID,Name,Path,Type,Subcontent,LastEdited,Size FROM Folders;";
            
            dbDatareader = dbCommand.ExecuteReader();
            while (dbDatareader.Read())
            {
                toBuild = dbDatareader.GetString(0) + dbDatareader.GetString(1) + dbDatareader.GetString(2) + dbDatareader.GetString(3) + dbDatareader.GetString(4) + dbDatareader.GetString(5) + dbDatareader.GetString(6);
                string myReader = toBuild;
                Console.WriteLine(myReader);
            }
        }

        public static File? FileFromQuery(SqliteConnection conn, string? query)
        {
            SqliteCommand dbCommand = conn.CreateCommand();
            SqliteDataReader dbDatareader;
            File? tempFile = null;
            
            dbCommand.CommandText = query;
            dbDatareader = dbCommand.ExecuteReader();

            while (dbDatareader.Read())
            {
                tempFile = new File(dbDatareader.GetString(1),
                    dbDatareader.GetString(2),
                    dbDatareader.GetString(3),
                    dbDatareader.GetString(4),
                    dbDatareader.GetDateTime(5),
                    dbDatareader.GetInt64(6));
            }
            return tempFile;
        }
        public static Folder? FolderFromQuery(SqliteConnection conn, string? query)
        {
            SqliteCommand dbCommand = conn.CreateCommand();
            SqliteDataReader dbDatareader;
            Folder? tempFolder = null;
            
            dbCommand.CommandText = query;
            dbDatareader = dbCommand.ExecuteReader();
            
            

            while (dbDatareader.Read())
            {
                tempFolder = new Folder(dbDatareader.GetString(1),
                    dbDatareader.GetString(2),
                    dbDatareader.GetString(3),
                    dbDatareader.GetString(4).Split("(?!^)"),
                    dbDatareader.GetDateTime(5));
                
            }
            return tempFolder;
        }
        public static void LoadActions(Dictionary<char, string> mutatorList)
        {
            mutatorList.Add('|',"MERGE");
            mutatorList.Add('+',"APPEND");
            mutatorList.Add('-',"DELETE");
            mutatorList.Add('/',"SPLIT UP");
            mutatorList.Add('\\',"SPLIT DOWN");
        }
    }
}