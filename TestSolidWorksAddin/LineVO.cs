using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolidWorksAddin
{
    class LineVO : IEquatable<LineVO>
    {
      // public long id;
       public  PointVO start;
       public PointVO end;
       public string name;
        public String key;

        public LineVO() { }

        public LineVO(PointVO start, PointVO end)
        {
            this.start = start;
            this.end   = end;

        }

        public LineVO(double x,double y, double z, double a, double b, double c)
        {
            this.start = new PointVO(x,y,z);
            this.end = new PointVO(a, b, c);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LineVO);
        }

        public bool Equals(LineVO other)
        {
            return (this.start.Equals(other.start) && this.end.Equals(other.end)) || (this.start.Equals(other.end) && this.end.Equals(other.start));
        }

        public bool Equals(LineVO x, LineVO y)
        {
            return x.Equals(y);
        }

      /*  public int GetHashCode(LineVO obj)
        {
            int xdif = (int) (this.start.x - obj.start.x + this.end.x - obj.end.x);
            int ydif = (int) (this.start.x - obj.start.x + this.end.x - obj.end.x);
            int zdif = (int) (this.start.x - obj.start.x + this.end.x - obj.end.x);
            return xdif + 59 * ydif + 127 * zdif;
        }*/
    }
}
