using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace ModifiedFiles
{
    internal class Version
    {
        /// <summary>
        /// Dictionary version: hash -> name file version
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, string>> _controlHash = new Dictionary<string, Dictionary<string, string>>();
        
        internal List<Models.Version> this[string dirVersion]
        {
            get
            {
                if (string.IsNullOrEmpty(dirVersion))
                    return null;

                if (_controlHash.ContainsKey(dirVersion))
                {
                    List<Models.Version> filesVersion = new List<Models.Version>();

                    foreach (KeyValuePair<string, string> keyHashPath in _controlHash[dirVersion])
                        filesVersion.Add(new Models.Version(keyHashPath.Value));

                    return filesVersion;
                }
                else
                    return null;
            }
        }

        internal bool CheckHash { get; set; } = true;

        internal void InitializeControlHashByDirectory(string dirVersion)
        {
            if (!_controlHash.ContainsKey(dirVersion))
                _controlHash.Add(dirVersion, InitializeHashFiles(dirVersion));
        }

        internal void CreateNewVersion(FileInfo file, string dirVersion)
        {
            string pathVersion = Path.Combine(
                dirVersion,
                $"version_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

            pathVersion = $"{pathVersion}{file.Extension}";

            FileInfo fileInfoVersion = new FileInfo(pathVersion);
            if (!fileInfoVersion.Exists)
            {
                bool copied = false;
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (CheckHash)
                            if (FileExistsByHash(dirVersion, file.FullName))
                                break;

                        file.CopyTo(pathVersion);
                        copied = true;
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(1000);
                    }

                    if (copied)
                        break;
                }
            }
        }

        private bool FileExistsByHash(string dirVersion, string path)
        {
            InitializeControlHashByDirectory(dirVersion);

            string currentHash = GetMD5(path);

            if (string.IsNullOrEmpty(currentHash))
                return true;

            if (_controlHash[dirVersion].ContainsKey(currentHash))
                return true;
            else
            {
                _controlHash[dirVersion].Add(
                    currentHash,
                    path);
                return false;
            }
        }

        private Dictionary<string, string> InitializeHashFiles(string dirVersion)
        {
            Dictionary<string, string> hashFiles = new Dictionary<string, string>();

            foreach (FileInfo file in new DirectoryInfo(dirVersion).GetFiles())
            {
                string currentHash = GetMD5(file.FullName);
                if (!hashFiles.ContainsKey(currentHash))
                    hashFiles.Add(currentHash, file.FullName);
            }
            
            return hashFiles;
        }

        private string GetMD5(string path)
        {
            string hash = string.Empty;

            if (new FileInfo(path).Exists)
            {
                try
                {
                    using (MD5 md5 = MD5.Create())
                    {
                        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            byte[] hashByte = md5.ComputeHash(stream);
                            hash = BitConverter.ToString(hashByte).Replace("-", "").ToLowerInvariant();
                            stream.Close();
                        }

                        md5.Clear();
                    }
                }
                catch (FileNotFoundException)
                {
                }
                catch (IOException ex)
                {
                    new IOException("Файл источник 'обрабатывается' другим приложением.", ex);
                }
            }

            return hash;
        }

    }
}
