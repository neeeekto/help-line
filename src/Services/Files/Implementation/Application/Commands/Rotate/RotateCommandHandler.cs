using System;
using System.Globalization;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using HelpLine.Services.Files.Models;
using MediatR;

namespace HelpLine.Services.Files.Application.Commands.Rotate
{
    internal class RotateCommandHandler : IRequestHandler<RotateCommand>
    {
        private readonly AwsSettings _settings;

        public RotateCommandHandler(AwsSettings settings)
        {
            _settings = settings;
        }


        public async Task<Unit> Handle(RotateCommand request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
            foreach (var rotate in request.Rotates)
            {
                var file = await client.GetObjectMetadataAsync(new GetObjectMetadataRequest()
                {
                    Key = rotate.FileId,
                    BucketName = _settings.S3BucketName
                }, cancellationToken);

                if (file == null)
                    continue;

                file.Metadata.Add("rotate", rotate.Angle.ToString(CultureInfo.InvariantCulture));
                var awsReq = new CopyObjectRequest()
                {
                    SourceBucket = _settings.S3BucketName,
                    DestinationBucket = _settings.S3BucketName,
                    SourceKey = rotate.FileId,
                    DestinationKey = rotate.FileId,
                    MetadataDirective = S3MetadataDirective.REPLACE,
                    ContentType = file.Headers.ContentType
                };
                foreach (var metadataKey in file.Metadata.Keys)
                {
                    awsReq.Metadata.Add(metadataKey, file.Metadata[metadataKey]);
                }

                try
                {
                    await client.CopyObjectAsync(awsReq, cancellationToken);
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return Unit.Value;
        }
    }
}
