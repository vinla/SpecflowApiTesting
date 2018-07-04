using System;

namespace Bdr.Api.Models
{
    public class Reminder
    {        
        public Reminder(int days, string message)
        {
            Days = days;
            Message = message;
        }

        public string Message {get;set;}

        public int Days {get;set;}
    }
}
