﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModifiedFiles.Models
{
    public class File
    {
        public File(string path)
        {
            Path = path;

            FileInfo fileInfo = new FileInfo(path);
            FileName = fileInfo.Name;
            Extension = fileInfo.Extension;
        }

        public string Path { get; }
        public string FileName { get; }
        public string Extension { get; }
        public ulong Size { get; private set; }
        public string SizeString { get; private set; }
        public uint Version { get; private set; }
        public DateTime DateVersion { get; private set; }
    }
}