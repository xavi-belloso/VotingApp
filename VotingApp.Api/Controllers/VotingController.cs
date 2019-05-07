using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyWebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VotingApp.Lib;

namespace VotingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotingController : ControllerBase
    {
        private readonly Voting _voting;
        private readonly int _step;
        private readonly ILogger<VotingController> _logger;
        private readonly IWebSocketPublisher _wsPublisher;

        public VotingController(Voting voting, IConfiguration config, ILogger<VotingController> logger, IWebSocketPublisher wsPublisher)
        {
            _voting = voting;
            _step = config.GetValue<int>("VotingStep", 1);
            _logger = logger;
            _wsPublisher = wsPublisher;
        }
        // GET api/values
        [HttpGet]
        public object Get() => _voting.GetState();

        // POST api/values
        [HttpPost]
        public async Task<object> Post([FromBody] string[] options) =>
            await ExecuteCommand(() => _voting.Start(options));

        // PUT api/values/5
        [HttpPut]
        public async Task<object> Put([FromBody] string value) =>
            await ExecuteCommand(() => _voting.Vote(value, _step));

        // DELETE api/values/5
        [HttpDelete]
        public async Task<object> Delete() =>
            await ExecuteCommand(() => _voting.Finish());

        private async Task<object> ExecuteCommand(Action command)
        {
            _logger.LogInformation($"Start command {JsonConvert.SerializeObject(_voting.GetState())}");
            command();
            _logger.LogInformation($"End Command {JsonConvert.SerializeObject(_voting.GetState())}");
            var status = _voting.GetState();
            await _wsPublisher.SendMessageToAllAsync(status);
            return status;
        }

    }
}
