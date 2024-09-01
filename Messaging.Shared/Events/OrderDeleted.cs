using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Shared.Events
{
    public class OrderDeleted
    {
        public string OrderId { get; set; }
    }
}
