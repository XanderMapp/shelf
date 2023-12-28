namespace Shelf
{
    /// <summary>
    /// A folder class
    /// </summary>
    /// <seealso cref="Shelf.SystemFile"/>
    public class Folder : SystemFile
    {
        public Folder()
        {
            
        }
        public Folder(string? name, string? path, string? type, string[]files, DateTime lastEdited)
        {
            Name = name;
            Path = path;
            Type = type;
            LastEdited = lastEdited;
        }
    }
}
