using System.IO;

namespace Selenium.Core.Helper
{
    public class FileHelper
    {
        public static string ReadAllTextFromFile(string filePath)
        {
            string contents = null;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    contents = sr.ReadToEnd();
                }
            }
            return contents;
        }

        public static void WriteAllTextToFile(string filePath, string contents)
        {
            if (contents != null)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(contents);
                        fs.SetLength(contents.Length);
                    }
                }
            }
        }
    }
}
