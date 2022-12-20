using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using HelpLine.Services.Files.Models;
using MediatR;

namespace HelpLine.Services.Files.Application.Commands.DeleteFiles
{
    internal class DeleteFilesCommandHandler : IRequestHandler<DeleteFilesCommand>
    {
        private readonly AwsSettings _settings;

        public DeleteFilesCommandHandler(AwsSettings settings)
        {
            _settings = settings;
        }

        public async Task<Unit> Handle(DeleteFilesCommand request, CancellationToken cancellationToken)
        {
            var credentials = new BasicAWSCredentials(_settings.S3AccessKey, _settings.S3SecretKey);
            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);

            await client.DeleteObjectsAsync(new DeleteObjectsRequest()
            {
                Objects = request.FilesIds.Select(x => new KeyVersion() {Key = x}).ToList(),
                BucketName = _settings.S3BucketName,
            }, cancellationToken);
            return Unit.Value;
        }
    }
}
