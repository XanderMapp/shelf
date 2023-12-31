namespace Shelf
{
    public class SystemFile
    {
        private DateTime _lastEdited;
        private long _size;

        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Type { get; set; }
        public string? Content { get; set; }
        public DateTime LastEdited
        {
            get
            {
                return _lastEdited;
            }
            set
            {
                _lastEdited = value;
            }
        }
        public long Size
        {
            get
            {
                return _size;
            } 
            set
            {
                if (value <= 0)
                {
                    throw new Exception("File does not exist (File size less than 0)");
                }
                else
                {
                    _size = value;
                }
            }
        }
        /// <summary>
        /// Default FileFolder constructor
        /// </summary>
        protected SystemFile()
        {

        }
        /// <summary>
        /// Overload 1 for SystemFile constructor, needs everything to construct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="type"></param>
        /// <param name="lastEdited"></param>
        /// <param name="size"></param>
        /// <param name="content"></param>
        public SystemFile(string? name, string? path, string? type, string? content, DateTime lastEdited, long size)
        {
            Name = name;
            Path = path;
            Type = type;
            Content = content;
            LastEdited = lastEdited;
            Size = size;
        }
    }
}