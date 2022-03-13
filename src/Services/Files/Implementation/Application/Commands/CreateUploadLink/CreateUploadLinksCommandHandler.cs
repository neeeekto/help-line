using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using HelpLine.Services.Files.Application.Views;
using HelpLine.Services.Files.Models;
using MediatR;

namespace HelpLine.Services.Files.Application.Commands.CreateUploadLink
{
    internal class CreateUploadLinksCommandHandler : IRequestHandler<CreateUploadLinksCommand, IReadOnlyDictionary<string, UploadView>>
    {
        private readonly AwsSettings _settings;

        public CreateUploadLinksCommandHandler(AwsSettings settings)
        {
            _settings = settings;
        }

        public async Task<IReadOnlyDictionary<string, UploadView>> Handle(CreateUploadLinksCommand request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);

            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);

            var result = new Dictionary<string, UploadView>();
            foreach (var fileDto in request.Files)
            {
                var fileKey = Guid.NewGuid().ToString();
                var awsReq = new GetPreSignedUrlRequest()
                {
                    Key = fileKey,
                    Expires = DateTime.UtcNow.AddDays(10),
                    BucketName = _settings.S3BucketName,
                    Verb = HttpVerb.PUT,
                    ContentType = fileDto.ContentType,
                };
                awsReq.Metadata.Add("name", fileDto.Key); // CLIENT NEED SEND x-amz-meta-name header with file name!!!
                var uploadPath = client.GetPreSignedURL(awsReq);
                result.Add(fileDto.Key, new UploadView()
                {
                    FileId = fileKey,
                    UploadUrl = uploadPath
                });
            }
            return result;
        }
    }
}
