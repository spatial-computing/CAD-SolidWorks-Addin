using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class ArcVO
    {
       public double radius;
       public PointVO centre;
       public PointVO start;
       public PointVO end;
       public double[] normalVector;

        public bool Equals(ArcVO other)
        {
            return (this.centre == other.centre)||
                   (this.start == other.start) ||
                   (this.end == other.end) ||
                   (this.radius == other.radius);
        }

    }
}
