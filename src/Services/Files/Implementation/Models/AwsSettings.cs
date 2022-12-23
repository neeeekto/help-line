namespace HelpLine.Services.Files.Models
{
    public class AwsSettings
    {
        public string S3AccessKey { get; set; }
        public string S3SecretKey { get; set; }
        public string S3BucketName { get; set; }
        public string S3BucketPreviewName { get; set; }
    }
}
