using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class ArcVO: IEquatable<ArcVO>
    {
       public double radius;
       public PointVO centre;
       public PointVO start;
       public PointVO end;
       public double[] normalVector;

        public bool Equals(ArcVO other)
        {
           bool centreEqual= this.centre.Equals(other.centre);
            return (centreEqual && 
                   (this.radius == other.radius))||
                   (this.start == other.start) ||
                   (this.end == other.end) 
                   ;
        }

    }
}
