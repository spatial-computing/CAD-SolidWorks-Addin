using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationFixerApp
{
    class RelationVO : IEquatable<RelationVO>
    {
        public int type;
        public string typeName;
        public double dimensionValue = 0;
        public List<SketchRelationEntityVO> entities = new List<SketchRelationEntityVO>();


        public override string ToString() {
            String str = @"Type: " + this.type + @" Type Name: " + this.typeName + @" Dimension: " + this.dimensionValue + @"Entities " + entities.ToString();
            return str;
        }


        public override bool Equals(object obj)
        {
            return this.Equals(obj as RelationVO);
        }


        public bool Equals(RelationVO other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.type != this.type)
            {
                return false;
            }

            if (other.typeName != this.typeName)
            {
                return false;
            }

            if(other.dimensionValue != this.dimensionValue)
            {
                return false;
            }

            if(other.entities!=null && this.entities != null)
            {
                if(other.entities.Count == this.entities.Count)
                {
                    for(int i = 0; i < this.entities.Count; i++)
                    {
                        SketchRelationEntityVO entity = this.entities.ElementAt(i);
                        if (!other.entities.Contains(entity))
                        {
                            return false;
                        }

                    }
                }
            }


            return true;
        }
    }
}
