using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class PartVO
    {
        public String name;
        public List<SketchVO> sketches = new List<SketchVO>();
        public List<SketchBodyVO> sketchBodies;
    }
}
