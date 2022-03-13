using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Admin.API.Controllers.Requests;
using HelpLine.Apps.Admin.API.Meta.Migrations;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Services.Migrations;
using HelpLine.Services.Migrations.Application.Commands.RunMigration;
using HelpLine.Services.Migrations.Application.Queries.GetAwaitingMigrations;
using HelpLine.Services.Migrations.Application.Queries.GetMigrations;
using HelpLine.Services.Migrations.Application.Views;
using HelpLine.Services.Migrations.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Admin.API.Controllers
{
    [ApiController]
    [Route("v1/migrations")]
    [Authorize]
    public class MigrationsController : ControllerBase
    {
        private readonly MigrationService _migrationService;
        private readonly MigrationsParamsDescRegistry _migrationsParamsDescRegistry = new ();

        public MigrationsController(MigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<MigrationView>>> Get()
        {
            var migrations = await _migrationService.ExecuteAsync(new GetMigrationsQuery());
            return Ok(migrations);
        }


        [HttpPost]
        [Route("{migration}")]
        public async Task<ActionResult> Run(string migration, [FromBody] MigrationParamsRequest request)
        {
            _migrationService.ExecuteAsync(new RunMigrationCommand(migration, request.Params));
            return Ok();
        }

        [HttpGet]
        [Route("descriptions/params")]
        public async Task<ActionResult<Dictionary<string, Description?>>> GetRequests()
        {
            return Ok(_migrationsParamsDescRegistry);
        }
    }
}
