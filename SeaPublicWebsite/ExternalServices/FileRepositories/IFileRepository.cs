using System.Collections.Generic;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    public interface IFileRepository
    {

        void Write(string relativeFilePath, string fileContents);

        string Read(string relativeFilePath);

        List<string> GetFiles(string relativeDirectoryPath);

        void Delete(string relativeFilePath);

    }
}
