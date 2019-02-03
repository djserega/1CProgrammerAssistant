using _1CProgrammerAssistant.Additions;
using System;
using System.IO;

namespace _1CProgrammerAssistant.ModifiedFiles.Models
{
    public class File
    {
        public File(string path)
        {
            Path = path;

            FileInfo fileInfo = new FileInfo(path);

            FileName = fileInfo.Name;
            Extension = fileInfo.Extension;
            FileNameWithoutExtension = FileName.Left(FileName.Length - Extension.Length);
            DateVersion = fileInfo.LastWriteTime;
        }

        #region Properties

        public string Path { get; }
        public string FileName { get; }
        public string Extension { get; }
        public string FileNameWithoutExtension { get; }
        public ulong Size { get; private set; }
        public string SizeString { get; private set; }
        public uint Version { get; private set; }
        public DateTime DateVersion { get; private set; }
        public string DirectoryVersion { get; internal set; }
        public string Description { get; set; }

        #endregion
    }
}
