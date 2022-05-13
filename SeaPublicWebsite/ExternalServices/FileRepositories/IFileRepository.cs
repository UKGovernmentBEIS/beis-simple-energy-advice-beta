using System.Collections.Generic;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public interface IFileRepository
    {

        void Write(string relativeFilePath, string fileContents);

        string Read(string relativeFilePath);

        List<string> GetFiles(string relativeDirectoryPath);

        void Delete(string relativeFilePath);

    }
}
