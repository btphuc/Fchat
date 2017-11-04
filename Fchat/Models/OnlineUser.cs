using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fchat.Models
{
    public class OnlineUser
    {
        public string name { get; set; }
        public string connectionID { get; set; }
        public bool isConnect { get; set; }
    }
}