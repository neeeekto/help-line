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

namespace HelpLine.Services.Files.Application.Queries.GetFiles
{
    internal class GetFilesQueryHandler : IRequestHandler<GetFilesQuery, IReadOnlyDictionary<string, FileView>>
    {
        private readonly AwsSettings _settings;

        public GetFilesQueryHandler(AwsSettings settings)
        {
            _settings = settings;
        }

        public async Task<IReadOnlyDictionary<string, FileView>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
            var result = new Dictionary<string, FileView>();
            foreach (var fileId in request.FilesIds)
            {
                var target = await client.GetObjectAsync(new GetObjectRequest()
                {
                    Key = fileId,
                    BucketName = _settings.S3BucketName,
                }, cancellationToken);
                if (target == null)
                    continue;


                var preview = await client.GetObjectAsync(new GetObjectRequest()
                {
                    Key = fileId,
                    BucketName = _settings.S3BucketPreviewName,

                });

                var path = client.GetPreSignedURL(new GetPreSignedUrlRequest()
                {
                    Key = fileId,
                    Expires = DateTime.UtcNow.Add(request.Duration),
                    BucketName = _settings.S3BucketName
                });
                result.Add(fileId, new FileView()
                {
                    FileId = fileId,
                    Name = target.Metadata["name"],
                    DownloadUri = new Uri(path),
                    PreviewUri = preview != null ? new Uri(client.GetPreSignedURL(new GetPreSignedUrlRequest()
                    {
                        Key = fileId,
                        Expires = DateTime.UtcNow.Add(request.Duration),
                        BucketName = _settings.S3BucketPreviewName
                    })) : null,
                    Size = target.Headers.ContentLength,
                    Type = target.Headers.ContentType,
                    LastModified = target.LastModified,
                    Rotate = target.Metadata["rotate"]
                });
            }

            return result;
        }
    }
}
