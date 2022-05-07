using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICE
{

    class Cities
    {
        public City[] сitylist { get; set; }
    }


    class City
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public string name { get; set; }

    }
}
