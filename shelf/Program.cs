using Microsoft.Data.Sqlite;
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
                SqliteConnection dbConn;


                dbConn = CreateConnection();

                //Instantiate a new ShelfParser
                Parser shelfParser = new Parser();

                while (!userHasQuit)
                {
                    List<File> newDirListFiles;
                    List<Folder> newDirListFolders;
                    
                    alterType = GetValidChar("What would you like to do? " +
                                             "(A) - New Directory" +
                                             "(B) - Directories in Database");
                    switch (char.ToUpper(alterType))
                    {
                        case 'A':
                            Console.Write("What is the path to the folder you wish to upload? ");
                            path = Console.ReadLine();

                            if (path != null)
                            {
                                newDirListFiles = MakeFileListFromDirectory(path);
                                newDirListFolders = MakeFolderListFromDirectory(path);
                                InsertData(dbConn,newDirListFiles,newDirListFolders);
                            }
                            break;
                        case 'B':
                            ReadData(dbConn);
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
                tempFile = new File(tempFileInfo.Name, tempFileInfo.DirectoryName, tempFileInfo.Extension,
                    tempFileInfo.LastWriteTime, tempFileInfo.Length);

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
                //Get folder info
                tempFolderInfo = new DirectoryInfo(aFolderName);

                //Creation of new folder, with info encoded into it.
                tempFolder = new Folder(tempFolderInfo.Name, tempFolderInfo.FullName, tempFolderInfo.Extension,tempFolderInfo.LastWriteTime);

                folders.Add(tempFolder);
            }
            return folders;
        }

        static SqliteConnection CreateConnection()
        {
            SqliteConnection dbConnection;
            dbConnection = new SqliteConnection("Data Source=shelfdata.sqlite,Version=3,New=True,Compress=True");

            try
            {
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return dbConnection;
        }

        static void InsertData(SqliteConnection connection, List<File> filesToWrite, List<Folder> foldersToWrite)
        {
            SqliteCommand dbCommand;
            dbCommand = connection.CreateCommand();
            string valueString;

            foreach (File aFile in filesToWrite)
            {
                valueString = $"({aFile.Path},{aFile.Name},{aFile.Type},{aFile.LastEdited},{aFile.Size}";
                const string fileQuery = "INSERT INTO FILES(Path,Name,Type,LastEdited,Size) VALUES(valueString)";
                dbCommand.CommandText = fileQuery;
                dbCommand.ExecuteNonQuery();
            }

            foreach (Folder aFolder in foldersToWrite)
            {
                valueString = $"({aFolder.Path},{aFolder.Name},{aFolder.Type},{aFolder.LastEdited})";
                const string folderQuery = "INSERT INTO Folders(Path,Name,Type,LastEdited) VALUES(valueString)";
                dbCommand.CommandText = folderQuery;
                dbCommand.ExecuteNonQuery();
            }
        }
        static void ReadData(SqliteConnection connection)
        {
            SqliteDataReader dbDatareader;
            SqliteCommand dbCommand;
            dbCommand = connection.CreateCommand();

            const string dataQuery = "SELECT * FROM Files UNION SELECT * FROM Folders";
            dbCommand.CommandText = dataQuery;

            dbDatareader = dbCommand.ExecuteReader();
            while (dbDatareader.Read())
            {
                string myreader = dbDatareader.GetString(0);
                Console.WriteLine(myreader);
            }
            connection.Close();
        }
    }
}