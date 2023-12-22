using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public File(string? name, string? path, string? type, DateTime lastEdited, long size)
        {
            Name = name;
            Path = path;
            Type = type;
            LastEdited = lastEdited;
            Size = size;
        }
    }
}
