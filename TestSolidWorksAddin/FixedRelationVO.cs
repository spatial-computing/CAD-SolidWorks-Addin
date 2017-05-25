using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolidWorksAddin
{
    class FixedRelationVO 
    {
        public String type = "Fixed"; //TODO: Convert to Enum
        public ArcVO entity;
        public String sketchName = "";
    }
}
