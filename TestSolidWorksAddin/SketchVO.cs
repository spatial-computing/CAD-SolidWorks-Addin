using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolidWorksAddin
{
    class SketchVO
    {
       public string name { get; set; }
        public List<RelationVO> relations = new List<RelationVO>();

       [NonSerialized] public Sketch swSketch;

    }
}
