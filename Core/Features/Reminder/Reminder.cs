using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Reminder
    {
        [BsonId]
        public string Id { get; set; }
        public int Index { get; set; }
        public DateTime DateTime { get; set; }
    }
}
