using System.IO;

namespace StoreManagement.API.Utils
{
    public class FileReader : IFileReader
    {
        public string Read(string filePath)
        {
            StreamReader productsReader = new StreamReader(filePath);
            return productsReader.ReadToEnd();
        }
    }
}
