namespace Shelf
{
    /// <summary>
    /// A class for files
    /// </summary>
    /// <seealso cref="Shelf.SystemFile"/>
    public class File : SystemFile
    {
        public File()
        {
        }

        /// <summary>
        /// A Greedy constructor for the File class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="content"></param>
        /// <param name="lastEdited"></param>
        /// <param name="size"></param>
        public File(string? name, string? path, string? type, string? content, DateTime lastEdited, long size)
        {
            Name = name;
            Path = path;
            Type = type;
            Content = content;
            LastEdited = lastEdited;
            Size = size;
        }

        //methods

        public void AppendText()
        {
            string? toAppend;

            Console.WriteLine("Write the text you wish to append to the file below");
            toAppend = Console.ReadLine();

            Content += toAppend;
        }

        public void DeleteText()
        {
            string? toDelete;
            int indexToDelete;

            Console.WriteLine("Write the text you wish to append to the file below");
            toDelete = Console.ReadLine();

            if (Content == null || toDelete == null) return;
            indexToDelete = Content.IndexOf(toDelete, StringComparison.Ordinal);
            Content = Content.Remove(indexToDelete, toDelete.Length);
        }
    }
}