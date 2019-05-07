using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public readonly Voting _voting;
        private readonly int _step;
        private readonly ILogger<VotingController> _logger;

        public VotingController(Voting voting, IConfiguration config, ILogger<VotingController> logger)
        {
            _voting = voting;
            _step = config.GetValue<int>("VotingStep", 1);
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public object Get()
        {
            _logger.LogWarning("Getting information");
            return _voting.GetState();
        }

        // POST api/values
        [HttpPost]
        public object Post([FromBody] string[] options)
        {
            _logger.LogWarning($"Start Voting {JsonConvert.SerializeObject(_voting.GetState())}");
            _voting.Start(options);
            return _voting.GetState();
        }

        // PUT api/values/5
        [HttpPut]
        public object Put([FromBody] string value)
        {
            _voting.Vote(value, _step);
            return _voting.GetState();
        }

        // DELETE api/values/5
        [HttpDelete]
        public object Delete()
        {
            _voting.Finish();
            return _voting.GetState();
        }
    }
}
