using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditAPI.Model
{
    public class RadditRateLimitInfo
    {
        public int  RemainingCalls { get; set; }
        public int  Reset { get; set; }
    }
}
