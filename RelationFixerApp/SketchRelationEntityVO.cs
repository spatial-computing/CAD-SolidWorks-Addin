using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class SketchRelationEntityVO
    {
        public string name { get; set; }
        public string sketchName { get; set; }
        public byte[] id { get; set; }
        public int type { get; set; }
        public string typeName { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as SketchRelationEntityVO);
        }


        public bool Equals(SketchRelationEntityVO other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.name != other.name)
            {
                return false;
            }

            if (this.sketchName != other.sketchName)
            {
                return false;
            }

            if (!this.id.SequenceEqual(other.id))
            {
                return false;
            }

            if (this.type != other.type)
            {
                return false;
            }

            if (this.typeName != other.typeName)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            String str = @"Name: " + this.name+ @"SketchName: " + this.sketchName + @" Id: " + this.id + @" TypeName: " + this.typeName + @" Type:" + this.type;
            return str;
        }
    }
}