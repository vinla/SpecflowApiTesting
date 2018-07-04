using Bdr.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bdr.Api.Controllers
{
    [Route("api/reminder")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private static List<Reminder> reminders = new List<Reminder>()
        { 
            new Reminder(2, "Hello"),
            new Reminder(3, "Goodbye"),
        };

        [HttpGet]
        public IEnumerable<Reminder> Get(int amount)
        {
            return reminders.OrderBy(r => r.Days).Take(amount);
        }

        [HttpPost]        
        [IgnoreAntiforgeryToken]
        public void Post([FromBody]Reminder reminder)
        {
            reminders.Add(reminder);
        }
    }
}