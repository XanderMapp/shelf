using Microsoft.Data.Sqlite;
using SysIO = System.IO;
namespace Shelf
{
    public class Merger : Turtle
    {
        public void CreateMergedFile(File? file1, File? file2)
        {
            string readFileText, readFileText2;
            long newFileSize;
            File? tempFile = null;
            
            readFileText = DocumentDataRead(file1);
            readFileText2 = DocumentDataRead(file2);
            var fullText = readFileText + readFileText2;
            
            newFileSize = fullText.Length * sizeof(char);

            tempFile = new File($"split-up-file-{DateTime.Now}",
                @"C:\Users\$USER\shelf-output",
                ".txt",
                fullText,
                DateTime.Now,
                newFileSize);

            if (tempFile?.Path == null) return;
            SysIO.File.Create(tempFile.Path);
            InsertSingleFileData(DbConn,tempFile);
        }
        public Merger(SqliteConnection dbConn)
        {
        }
    }
}