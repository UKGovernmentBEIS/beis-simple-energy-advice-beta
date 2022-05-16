using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public class VcapServices
    {
        [JsonProperty("aws-s3-bucket")]
        public List<VcapAwsS3Bucket> AwsS3Bucket { get; set; }
    }

    public class VcapAwsS3Bucket
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("credentials")]
        public VcapAwsS3Credentials Credentials { get; set; }
    }

    public class VcapAwsS3Credentials
    {
        [JsonProperty("bucket_name")]
        public string BucketName { get; set; }

        [JsonProperty("aws_access_key_id")]
        public string AwsAccessKeyId { get; set; }

        [JsonProperty("aws_secret_access_key")]
        public string AwsSecretAccessKey { get; set; }

        [JsonProperty("aws_region")]
        public string Region { get; set; }
    }
}
