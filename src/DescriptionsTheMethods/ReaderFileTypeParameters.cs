using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace _1CProgrammerAssistant.DescriptionsTheMethods
{
    internal class ReaderFileTypeParameters
    {
        internal ReaderFileTypeParameters(string path)
        {
            Path = path;
        }

        internal string Path { get; }

        internal bool Exist()
        {
            return new FileInfo(Path).Exists;
        }

        internal void Create(string text)
        {
            FileInfo fileInfo = new FileInfo(Path);
            using (StreamWriter streamWriter = fileInfo.CreateText())
            {
                streamWriter.Write(text);
                streamWriter.Close();
            }
        }

        internal string Read()
        {
            string textInFile = string.Empty;

            using (StreamReader reader = new StreamReader(Path))
            {
                textInFile = reader.ReadToEnd();
                reader.Close();
            }

            return textInFile;
        }
    }
}
