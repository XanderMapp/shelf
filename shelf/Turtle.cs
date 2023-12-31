using Microsoft.Data.Sqlite;
using SysIO = System.IO;

namespace Shelf
{
    public class Turtle
    {
        private bool _isOn;

        public SqliteConnection? DbConn { get; set; }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (!FinishedJob)
                {
                    throw new Exception("How can one be ON after finishing their day?");
                }
                else
                {
                    _isOn = value;
                }
            }
        }

        public bool FinishedJob { get; set; }

        public static string DocumentDataRead(File? aFile)
        {
            string docText = "";

            if (SysIO.File.Exists($"{aFile}"))
            {
                try
                {
                    var reader = new StreamReader($"{aFile}");

                    while (!reader.EndOfStream)
                    {
                        docText += reader.ReadLine();
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
            else
            {
                throw new Exception("NO FILE?");
            }

            return docText;
        }

        public static void InsertSingleFileData(SqliteConnection? connection, File aFile)
        {
            SqliteCommand dbCommand = connection.CreateCommand();

            dbCommand.CommandText =
                $"INSERT INTO Files(FileID,Name,Path,Type,Content,LastEdited,Size) VALUES(ROWID,{aFile.Name},{aFile.Path},{aFile.Type},{aFile.Content},{aFile.LastEdited},{aFile.Size})";

            dbCommand.ExecuteNonQuery();
        }

        public static void InsertSingleFolderData(SqliteConnection connection, Folder aFolder)
        {
            SqliteCommand dbCommand = connection.CreateCommand();

            dbCommand.CommandText =
                $"INSERT INTO Folders(FolderID,Name,Path,Type,Subcontent,LastEdited,Size) VALUES(ROWID,{aFolder.Name},{aFolder.Path},{aFolder.Type},{aFolder.Content},{aFolder.LastEdited},{aFolder.Size})";

            dbCommand.ExecuteNonQuery();
        }
    }
}