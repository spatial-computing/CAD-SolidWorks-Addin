using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolidWorksAddin
{
    class PointVO : IEquatable<PointVO>
    {
       public byte[] id;
       public String key;
       public double x;
       public double y;
       public double z;
    
        public PointVO() { }

        public PointVO(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            key = "x:" + x + "y:"+y + "z:" + z;
        }

        public bool Equals(PointVO other)
        {
            return ((this.x == other.x)) &&
                   ((this.y == other.y)) &&
                   ((this.z == other.z)) ;
        }



    }
}
