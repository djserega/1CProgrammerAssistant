using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1CProgrammerAssistant.ModifiedFiles.Models
{
    public class Version
    {
        public Version(string path)
        {
            Path = path;
            DateVersion = new FileInfo(path).LastWriteTime;
        }

        public string Path { get; set; }
        public int NumberVersion { get; set; }
        public DateTime DateVersion { get; set; }
        public string Description { get; set; }
    }
}
