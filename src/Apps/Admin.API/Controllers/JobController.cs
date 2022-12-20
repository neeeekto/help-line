using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.Apps.Admin.API.Meta;
using HelpLine.Apps.Admin.API.Meta.Jobs;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Services.Jobs;
using HelpLine.Services.Jobs.Application.Commands.CreateJob;
using HelpLine.Services.Jobs.Application.Commands.DeleteJob;
using HelpLine.Services.Jobs.Application.Commands.FireJob;
using HelpLine.Services.Jobs.Application.Commands.ToggleJob;
using HelpLine.Services.Jobs.Application.Commands.UpdateJob;
using HelpLine.Services.Jobs.Application.DTO;
using HelpLine.Services.Jobs.Application.Queries.GetJobs;
using HelpLine.Services.Jobs.Application.Queries.GetJobsTriggerState;
using HelpLine.Services.Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpLine.Apps.Admin.API.Controllers
{
    [ApiController]
    [Route("v1/jobs")]
    [Authorize]
    public class JobController : ControllerBase
    {
        private readonly JobsService _jobsService;
        private readonly JobTasksDescRegistry _tasksDescRegistry = new ();

        public JobController(JobsService jobsService)
        {
            _jobsService = jobsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = await _jobsService.ExecuteAsync(new GetJobsQuery());
            return Ok(jobs);
        }

        [HttpGet]
        [Route("{jobId:guid}")]
        public async Task<ActionResult<Job>> GetJob(Guid jobId)
        {
            var job = await _jobsService.ExecuteAsync(new GetJobQuery(jobId));
            return Ok(job);
        }

        [HttpPost]
        [Route("state")]
        public async Task<ActionResult<Dictionary<Guid, JobTriggerState>>> GetJobsTriggersState(IEnumerable<Guid> jobIds)
        {
            var triggerStates = await _jobsService.ExecuteAsync(new GetJobsTriggerStateQuery(jobIds));
            return Ok(triggerStates);
        }

        [HttpPost]
        [Route("{task}")]
        public async Task<ActionResult<Guid>> Add(JobDataDto request, string task)
        {
            var jobId = await _jobsService.ExecuteAsync(new CreateJobCommand(request, task));
            return Ok(jobId);
        }

        [HttpPatch]
        [Route("{jobId:guid}")]
        public async Task<ActionResult> Update(JobDataDto request, Guid jobId)
        {
            await _jobsService.ExecuteAsync(new UpdateJobCommand(jobId, request));
            return Ok();
        }

        [HttpDelete]
        [Route("{jobId:guid}")]
        public async Task<ActionResult> Delete(Guid jobId)
        {
            await _jobsService.ExecuteAsync(new DeleteJobCommand(jobId));
            return Ok();
        }

        [HttpPost]
        [Route("{jobId:guid}/toggle")]
        public async Task<ActionResult> Toggle([FromBody] bool enable, Guid jobId)
        {
            await _jobsService.ExecuteAsync(new ToggleJobCommand(jobId, enable));
            return Ok();
        }

        [HttpGet]
        [Route("tasks")]
        public async Task<ActionResult<Dictionary<string, Description?>>> GetTasks()
        {
            return Ok(_tasksDescRegistry);
        }

        [HttpPost]
        [Route("{jobId:guid}/fire")]
        public async Task<ActionResult> Fire(Guid jobId)
        {
            await _jobsService.ExecuteAsync(new FireJobManualCommand(jobId));
            return Ok();
        }
    }
}
