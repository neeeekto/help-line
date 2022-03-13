using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Services.Files;
using HelpLine.Services.Files.Application.Commands.CreateUploadLink;
using HelpLine.Services.Files.Application.Commands.Rotate;
using HelpLine.Services.Files.Application.DTO;
using HelpLine.Services.Files.Application.Queries.GetFiles;
using HelpLine.Services.Files.Application.Queries.GetFilesLink;
using HelpLine.Services.Files.Application.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Client.API.Features
{
    [ApiController]
    [Route("v1/files")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly FilesService _filesService;

        public FilesController(FilesService filesService)
        {
            _filesService = filesService;
        }

        [HttpPost]
        [Route("get")]
        public async Task<ActionResult<IReadOnlyDictionary<string, FileView>>> GetFiles(IEnumerable<string> filesIds, [FromQuery] TimeSpan? duration)
        {
            var files = await _filesService.ExecuteAsync(new GetFilesQuery(filesIds, duration ?? TimeSpan.FromMinutes(30)));
            return Ok(files);
        }

        [HttpPost]
        [Route("get/simple")]
        public async Task<ActionResult<IReadOnlyDictionary<string, string>>> GetSimpleFiles(IEnumerable<string> filesIds, [FromQuery] TimeSpan? duration)
        {
            var files = await _filesService.ExecuteAsync(new GetFilesLinkQuery(filesIds, duration ?? TimeSpan.FromMinutes(30)));
            return Ok(files);
        }

        [HttpGet]
        [Route("get/simple/{fileId}")]
        public async Task<ActionResult> GetSimpleFiles(string fileId, [FromQuery] TimeSpan? duration)
        {
            var file = await _filesService.ExecuteAsync(new GetFilesLinkQuery(new [] {fileId}, duration ?? TimeSpan.FromHours(1)));
            return Ok(file[fileId]);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<IReadOnlyDictionary<string, UploadView>>> CreateUploading(IEnumerable<FileDto> files)
        {
            var result = await _filesService.ExecuteAsync(new CreateUploadLinksCommand(files));
            return Ok(result);
        }

        [HttpPost]
        [Route("rotate")]
        public async Task<ActionResult> Rotate(IEnumerable<RotateDto> rotates)
        {
            await _filesService.ExecuteAsync(new RotateCommand(rotates));
            return Ok();
        }
    }
}
