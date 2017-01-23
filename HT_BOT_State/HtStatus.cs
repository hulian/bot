using bgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hooks
{
    class HtStatus
    {
        public string mode {  set; get; }
        public Map<string, Button> buttons = new Map<string, Button>();
    }
}
