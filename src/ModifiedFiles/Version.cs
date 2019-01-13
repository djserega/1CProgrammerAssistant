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

        internal bool CheckHash { get; set; } = true;

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
                    catch (Exception)
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
            if (!_controlHash.ContainsKey(dirVersion))
                _controlHash.Add(dirVersion, new Dictionary<string, string>());

            string currentHash = GetMD5(path);

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

        private string GetMD5(string path)
        {
            string hash = string.Empty;

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
            catch (IOException)
            {
            }

            return hash;
        }

    }
}
