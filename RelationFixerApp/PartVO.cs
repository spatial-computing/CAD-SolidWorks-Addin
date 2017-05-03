using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class PartVO
    {
        public string name { get; set; }
      public  List<SketchVO> sketches = new List<SketchVO>();
    }
}
