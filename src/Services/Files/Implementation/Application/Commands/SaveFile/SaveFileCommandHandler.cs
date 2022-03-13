using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using HelpLine.Services.Files.Models;
using MediatR;

namespace HelpLine.Services.Files.Application.Commands.SaveFile
{
    internal class SaveFileCommandHandler : IRequestHandler<SaveFileCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly AwsSettings _settings;


        public SaveFileCommandHandler(IMediator mediator, AwsSettings settings)
        {
            _mediator = mediator;
            _settings = settings;
        }


        public async Task<string> Handle(SaveFileCommand request, CancellationToken cancellationToken)
        {
            var fileKey = Guid.NewGuid().ToString();
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);
            using var transfer = new TransferUtility(client);
            var req = new TransferUtilityUploadRequest
            {
                BucketName = _settings.S3BucketName,
                Key = fileKey,
                InputStream = request.Content,
                StorageClass = S3StorageClass.Standard,
                CannedACL = S3CannedACL.AuthenticatedRead,
                ContentType = request.ContentType
            };
            req.Metadata.Add("name", request.Name);
            await transfer.UploadAsync(req, cancellationToken);
            return fileKey;
        }
    }
}
