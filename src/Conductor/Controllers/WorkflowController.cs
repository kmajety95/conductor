﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Conductor.Auth;
using Conductor.Domain.Interfaces;
using Conductor.Domain.Models;
using Conductor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Newtonsoft.Json.Linq;
using WorkflowCore.Interface;

namespace Conductor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowController _workflowController;
        private readonly IPersistenceProvider _persistenceProvider;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public WorkflowController(IWorkflowController workflowController, IPersistenceProvider persistenceProvider, IMapper mapper, ILogger<WorkflowController> logger)
        {
            _workflowController = workflowController;
            _persistenceProvider = persistenceProvider;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Policies.Viewer)]
        public async Task<ActionResult<WorkflowInstance>> Get(string id)
        {
            _logger.LogDebug("Hello there");
            var result = await _persistenceProvider.GetWorkflowInstance(id);
            if (result == null)
                return NotFound();

            return Ok(_mapper.Map<WorkflowInstance>(result));
        }

        [HttpPost("{id}")]
        [Authorize(Policy = Policies.Controller)]
        public async Task<ActionResult<WorkflowInstance>> Post(string id, [FromBody] ExpandoObject data)
        {
            var instanceId = await _workflowController.StartWorkflow(id, data);
            var result = await _persistenceProvider.GetWorkflowInstance(instanceId);

            return Created(instanceId, _mapper.Map<WorkflowInstance>(result));
        }

        [HttpPut("{id}/suspend")]
        [Authorize(Policy = Policies.Controller)]
        public async Task Suspend(string id)
        {
            var result = await _workflowController.SuspendWorkflow(id);
            if (result)
                Response.StatusCode = 200;
            else
                Response.StatusCode = 400;
        }

        [HttpPut("{id}/resume")]
        [Authorize(Policy = Policies.Controller)]
        public async Task Resume(string id)
        {
            var result = await _workflowController.ResumeWorkflow(id);
            if (result)
                Response.StatusCode = 200;
            else
                Response.StatusCode = 400;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Controller)]
        public async Task Terminate(string id)
        {
            var result = await _workflowController.TerminateWorkflow(id);
            if (result)
                Response.StatusCode = 200;
            else
                Response.StatusCode = 400;
        }


    }
}