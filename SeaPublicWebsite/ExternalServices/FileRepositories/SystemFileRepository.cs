using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public class SystemFileRepository : IFileRepository
    {

        private readonly DirectoryInfo rootDir;

        public SystemFileRepository()
        {
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            rootDir = new DirectoryInfo(rootPath);
        }

        public void Write(string relativeFilePath, string fileContents)
        {
            string fullFilePath = GetFullPath(relativeFilePath);

            string directory = Path.GetDirectoryName(fullFilePath);

            // Create the folder (if it's missing)
            if (!GetDirectoryExists(directory))
            {
                CreateDirectory(directory);
            }

            File.WriteAllText(fullFilePath, fileContents);
        }

        public void Write(string relativeFilePath, byte[] fileContents)
        {
            string fullFilePath = GetFullPath(relativeFilePath);

            string directory = Path.GetDirectoryName(fullFilePath);

            // Create the folder (if it's missing)
            if (!GetDirectoryExists(directory))
            {
                CreateDirectory(directory);
            }

            File.WriteAllBytes(fullFilePath, fileContents);
        }

        public string Read(string relativeFilePath)
        {
            string fullFilePath = GetFullPath(relativeFilePath);

            return File.ReadAllText(fullFilePath);
        }

        public List<string> GetFiles(string relativeDirectoryPath)
        {
            string directoryFilePath = GetFullPath(relativeDirectoryPath);

            string[] filePaths = Directory.GetFiles(directoryFilePath);

            List<string> fileNames = filePaths.Select(fp => Path.GetFileName(fp)).ToList();

            return fileNames;
        }

        public void Delete(string relativeFilePath)
        {
            string fullFilePath = GetFullPath(relativeFilePath);

            File.Delete(fullFilePath);
        }

        private string GetFullPath(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!Path.IsPathRooted(filePath))
            {
                filePath = Path.Combine(rootDir.FullName, filePath);
            }

            return filePath;
        }

        private void CreateDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!Path.IsPathRooted(directoryPath))
            {
                directoryPath = Path.Combine(rootDir.FullName, directoryPath);
            }

            Directory.CreateDirectory(directoryPath);
        }

        private bool GetDirectoryExists(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (!Path.IsPathRooted(directoryPath))
            {
                directoryPath = Path.Combine(rootDir.FullName, directoryPath);
            }

            return Directory.Exists(directoryPath);
        }

    }
}
