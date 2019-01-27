using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1CProgrammerAssistant.ModifiedFiles.Models
{
    public class Version
    {
        public static string FileNameVersion { get; } = "description.json";

        public Version(string path)
        {
            Path = path;
            DateVersion = new FileInfo(path).LastWriteTime;
        }

        [JsonConstructor]
        public Version(string path, string description) : this(path)
        {
            Description = description ?? string.Empty;
        }

        public string Path { get; set; }
        public int NumberVersion { get; set; }
        public DateTime DateVersion { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
