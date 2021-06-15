using Microsoft.AspNetCore.Mvc;
using OutingsApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OutingsApi.Controllers
{
    public class ScheduledOutingsController : ControllerBase
    {
        private readonly IWriteOutingsForProcessing _processor;

        public ScheduledOutingsController(IWriteOutingsForProcessing processor)
        {
            _processor = processor;
        }

        [HttpPost("scheduledoutings")]
        public async Task<ActionResult> ScheduleAnOuting([FromBody] PostOutingCreateRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // 1. write it to our "inside" database (i own this)

            // 2. actually process the thing, do something with it...

            // 3. Log it out somewhere for someone else to deal with later
            // write to a messager queue, for us that will be kafka
            await _processor.SendOuting(request);
            return Accepted();

        }
    }

    public record PostOutingCreateRequest
    {
        [Required]
        public string ParkId { get; init; }
        [Required]
        public DateTime When { get; init; }
        [Required]
        public string Who { get; init; }
        [Required]
        public string Notes { get; init; }
        public int? NumberOfPeople { get; init; }
    }
}
