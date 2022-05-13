using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public class AwsFileRepository : IFileRepository
    {

        private readonly VcapAwsS3Bucket vcapAwsS3Bucket;

        public AwsFileRepository(VcapAwsS3Bucket vcapAwsS3Bucket)
        {
            this.vcapAwsS3Bucket = vcapAwsS3Bucket;
        }


        public void Write(string relativeFilePath, string fileContents)
        {
            Console.WriteLine($"Writing file {relativeFilePath}");//qq:DCC
            Console.WriteLine($"To bucket {vcapAwsS3Bucket.Credentials.BucketName}");//qq:DCC
            
            using (AmazonS3Client client = CreateAmazonS3Client())
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = vcapAwsS3Bucket.Credentials.BucketName,
                    Key = DirToUrlSeparator(relativeFilePath),
                    ContentBody = fileContents
                };

                var response = client.PutObjectAsync(putRequest).Result;
                
                Console.WriteLine($"AWS response code {response.HttpStatusCode}");//qq:DCC
            }
        }

        public void Write(string relativeFilePath, byte[] fileContents)
        {
            using (AmazonS3Client client = CreateAmazonS3Client())
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(fileContents);

                var putRequest = new PutObjectRequest
                {
                    BucketName = vcapAwsS3Bucket.Credentials.BucketName,
                    Key = DirToUrlSeparator(relativeFilePath),
                    InputStream = memoryStream
                };

                client.PutObjectAsync(putRequest).Wait();
            }
        }

        public string Read(string relativeFilePath)
        {
            Console.WriteLine($"Reading file {relativeFilePath}");//qq:DCC
            Console.WriteLine($"In S3 bucket {vcapAwsS3Bucket.Credentials.BucketName}");//qq:DCC
            using (AmazonS3Client client = CreateAmazonS3Client())
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = vcapAwsS3Bucket.Credentials.BucketName,
                    Key = DirToUrlSeparator(relativeFilePath)
                };

                using (GetObjectResponse response = client.GetObjectAsync(request).Result)
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    Console.WriteLine($"AWS response code {response.HttpStatusCode}");//qq:DCC
                    string csvFileContents = reader.ReadToEnd();
                    return csvFileContents;
                }
            }
        }

        public List<string> GetFiles(string relativeDirectoryPath)
        {
            Console.WriteLine($"Finished listing files in directory ./{relativeDirectoryPath}");//qq:DCC
            Console.WriteLine($"In S3 bucket {vcapAwsS3Bucket.Credentials.BucketName}");//qq:DCC
            using (AmazonS3Client client = CreateAmazonS3Client())
            {
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = vcapAwsS3Bucket.Credentials.BucketName,
                    Prefix = DirToUrlSeparator(relativeDirectoryPath),
                    MaxKeys = 10000
                };

                var filePaths = new List<string>();
                ListObjectsV2Response response;
                do
                {
                    response = client.ListObjectsV2Async(request).Result;
                    Console.WriteLine($"AWS response code {response.HttpStatusCode}");//qq:DCC

                    foreach (S3Object entry in response.S3Objects)
                    {
                        string fileNameWithDirectory = entry.Key;
                        Console.WriteLine($"Found file {fileNameWithDirectory}");//qq:DCC

                        string fileNameWithoutDirectory =
                            (!string.IsNullOrEmpty(relativeDirectoryPath) && fileNameWithDirectory.StartsWith(relativeDirectoryPath))
                                ? fileNameWithDirectory.Substring(relativeDirectoryPath.Length + 1)
                                : fileNameWithDirectory;

                        filePaths.Add(fileNameWithoutDirectory);
                        Console.WriteLine($"File name processed to {fileNameWithoutDirectory}");//qq:DCC
                    }
                    request.ContinuationToken = response.NextContinuationToken;
                } while (response.IsTruncated);

                Console.WriteLine($"Finished listing files in S3 bucket {relativeDirectoryPath}");//qq:DCC
                return filePaths;
            }
        }

        public void Delete(string relativeFilePath)
        {
            using (AmazonS3Client client = CreateAmazonS3Client())
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = vcapAwsS3Bucket.Credentials.BucketName,
                    Key = DirToUrlSeparator(relativeFilePath)
                };

                client.DeleteObjectAsync(request).Wait();
            }
        }

        private AmazonS3Client CreateAmazonS3Client()
        {
            string accessKey = vcapAwsS3Bucket.Credentials.AwsAccessKeyId;
            string secretKey = vcapAwsS3Bucket.Credentials.AwsSecretAccessKey;

            var credentials = new BasicAWSCredentials(accessKey, secretKey);
            var amazonS3Client = new AmazonS3Client(credentials, RegionEndpoint.GetBySystemName(vcapAwsS3Bucket.Credentials.Region));

            Console.WriteLine($"Created AmazonS3Client for region {vcapAwsS3Bucket.Credentials.Region}");//qq:DCC
            return amazonS3Client;
        }

        private static string DirToUrlSeparator(string filePath)
        {
            return filePath?.Replace('\\', '/');
        }

    }
}
