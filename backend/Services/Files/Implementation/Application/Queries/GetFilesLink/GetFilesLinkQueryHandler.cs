using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using HelpLine.Services.Files.Models;
using MediatR;

namespace HelpLine.Services.Files.Application.Queries.GetFilesLink
{
    internal class GetFilesLinkQueryHandler : IRequestHandler<GetFilesLinkQuery, IReadOnlyDictionary<string, string>>
    {
        private readonly AwsSettings _settings;

        public GetFilesLinkQueryHandler(AwsSettings settings)
        {
            _settings = settings;
        }

        public async Task<IReadOnlyDictionary<string, string>> Handle(GetFilesLinkQuery request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
            var result = new Dictionary<string, string>();
            foreach (var fileId in request.FilesIds)
            {
                var path = client.GetPreSignedURL(new GetPreSignedUrlRequest()
                {
                    Key = fileId,
                    Expires = DateTime.UtcNow.Add(request.Duration),
                    BucketName = _settings.S3BucketName
                });
                result.Add(fileId, path);
            }

            return result;
        }
    }
}
