using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Services.Files;
using HelpLine.Services.Files.Application.Commands.CreateUploadLink;
using HelpLine.Services.Files.Application.DTO;
using HelpLine.Services.Files.Application.Queries.GetFiles;
using HelpLine.Services.Files.Application.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.External.API.Controllers
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
        public async Task<ActionResult<IReadOnlyDictionary<string, FileView>>> GetFiles(IEnumerable<string> filesIds)
        {
            var files = await _filesService.ExecuteAsync(new GetFilesQuery(filesIds, TimeSpan.FromMinutes(30)));
            return Ok(files);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<IReadOnlyDictionary<string, UploadView>>> CreateUploading(IEnumerable<FileDto> files)
        {
            var result = await _filesService.ExecuteAsync(new CreateUploadLinksCommand(files));
            return Ok(result);
        }
    }
}
