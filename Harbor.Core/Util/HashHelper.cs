using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace Harbor.Core.Util
{
    public class HashHelper
    {
        public static Dictionary<string, string> GetHashForFileList(List<string> filelist)
        {
            Dictionary<string, string> results = new();
            using var sha256 = SHA256.Create();
            foreach (var f in filelist.Where(File.Exists))
            {
                using var stream = File.Open(f, FileMode.Open);
                stream.Position = 0;
                var hashValue = sha256.ComputeHash(stream);
                var hashStr = ByteToString(hashValue);
                results.Add(f, hashStr);
            }

            return results;
        }

        private static string ByteToString(byte[] bytes)
        {
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }

        public static Dictionary<string, string> GetLocalHashStorage(string prefix)
        {
            var localHashFile = Path.Combine(System.Environment.CurrentDirectory, ".harbor", prefix+"_lasthash.json");
            if (!File.Exists(localHashFile)) return null;
            var content = File.ReadAllText(localHashFile);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
        }

        public static void SaveLocalHash(Dictionary<string, string> hash, string prefix)
        {
            if (hash == null)
            {
                return;
            }
            var localHarbor = Path.Combine(System.Environment.CurrentDirectory, ".harbor");
            if (!Directory.Exists(localHarbor))
            {
                Directory.CreateDirectory(localHarbor);
            }
            var localHashFile = Path.Combine(localHarbor, prefix+"_lasthash.json");
            var json = JsonConvert.SerializeObject(hash);
            File.WriteAllText(localHashFile, json);
        }

        public static bool CheckTwoHash(Dictionary<string, string> local, Dictionary<string, string> newHash)
        {
            if (local == null || newHash == null)
            {
                return false;
            }
            foreach (var (key, nH) in newHash)
            {
                if (local.ContainsKey(key))
                {
                    var lH = local[key];
                    if (lH != nH)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static (bool match, Dictionary<string,string> newHash) Check(List<string> filelist, string prefix)
        {
            var newHash = GetHashForFileList(filelist);
            var localHash = GetLocalHashStorage(prefix);
            return (CheckTwoHash(localHash, newHash), newHash);
        }
    }
}
