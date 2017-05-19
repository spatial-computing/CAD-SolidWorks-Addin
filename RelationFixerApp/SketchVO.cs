
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class SketchVO
    {
       public string name { get; set; }
        public Object swSketch { get; set; }
        public List<RelationVO> relations = new List<RelationVO>();
        public SketchGeometry geometry;
    }
}
