using Microsoft.Data.Sqlite;

namespace Shelf
{
    /// <summary>
    /// A ShelfParser Class
    /// </summary>
    public class Parser
    {
        public void Parse(SqliteConnection dbConn, IEnumerable<char> parsedChars, Dictionary<char, string> mutators, File? file1)
        {
            Splitter parserSplitter = new Splitter(dbConn);
            Merger parserMerger = new Merger(dbConn);

            foreach (char aChar in parsedChars)
            {
                if (parserSplitter != null && parserMerger != null)
                {
                    switch (aChar)
                    {
                        case '+':
                        {
                            file1?.AppendText();
                            break;
                        }
                        case '-':
                        {
                            file1?.DeleteText();
                            break;
                        }
                        case '\\':
                        {
                            parserSplitter.CreateFileSplice(file1, "DOWN");
                            break;
                        }
                        case '/':
                        {
                            parserSplitter.CreateFileSplice(file1, "UP");
                            break;
                        }
                        default:
                        {
                            throw new ArgumentException($"No valid actions can be taken with this input {aChar}");
                        }
                    }
                }
            }
        }
        public void Parse(SqliteConnection dbConn, IEnumerable<char> parsedChars, Dictionary<char, string> mutators, Folder? aFolder)
        {
            Splitter parserSplitter = new Splitter(dbConn);
            Merger parserMerger = new Merger(dbConn);

            foreach (char aChar in parsedChars)
            {
                if (parserSplitter != null && parserMerger != null && aFolder != null)
                {
                    switch (aChar)
                    {
                        case '\\':
                        {
                            parserSplitter.CreateFolderSplice(aFolder, "DOWN");
                            break;
                        }
                        case '/':
                        {
                            parserSplitter.CreateFolderSplice(aFolder, "UP");
                            break;
                        }
                        default:
                        {
                            throw new ArgumentException($"No valid actions can be taken with this input {aChar}");
                        }
                    }
                }
            }
        }

        public void Parse(SqliteConnection dbConn, IEnumerable<char> parsedChars, Dictionary<char, string> mutators,
            File? file1, File? file2)
        {
            Splitter parserSplitter = new Splitter(dbConn);
            Merger parserMerger = new Merger(dbConn);

            foreach (char aChar in parsedChars)
            {
                if (parserSplitter != null && parserMerger != null)
                {
                    switch (aChar)
                    {
                        case '|':
                        {
                            parserMerger.CreateMergedFile(file1, file2);
                            break;
                        }
                        case '+':
                        {
                            file1?.AppendText();
                            file2?.AppendText();
                            break;
                        }
                        case '-':
                        {
                            file1?.DeleteText();
                            file2?.DeleteText();
                            break;
                        }
                        case '\\':
                        {
                            parserSplitter.CreateFileSplice(file1, "DOWN");
                            parserSplitter.CreateFileSplice(file2, "DOWN");
                            break;
                        }
                        case '/':
                        {
                            parserSplitter.CreateFileSplice(file1, "UP");
                            parserSplitter.CreateFileSplice(file2, "UP");
                            break;
                        }
                        default:
                        {
                            throw new ArgumentException($"No valid actions can be taken with this input {aChar}");
                        }
                    }
                }
            }
        }
    }
}