using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_Company.Models
{
    public class EmailQueueItem
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string To { get; set; }
    }
}
