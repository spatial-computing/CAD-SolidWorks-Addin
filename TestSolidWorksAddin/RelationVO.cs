using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSolidWorksAddin
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

        public RelationVO()
        {

        }

        public RelationVO(SketchRelation swSkRel,String sketchName,ModelDoc2 swModel)
        {
            
            int j;
            SketchPoint swSkPt = null;
            SketchSegment swSkSeg = null;
                typeName = Enum.GetName(typeof(swConstraintType_e), swSkRel.GetRelationType());

                type = swSkRel.GetRelationType();

                //Dimensions need to check if required
               DisplayDimension dispDim = (DisplayDimension)swSkRel.GetDisplayDimension();
                if (dispDim != null)
                {
                    dimensionValue = dispDim.GetDimension().GetSystemValue2(""); 
                }

               int[] vEntTypeArr = (int[])swSkRel.GetEntitiesType();
               object[] vEntArr = (object[])swSkRel.GetEntities();
             
                object[] vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if ((vDefEntArr == null))
                {
                }

            if ((vEntTypeArr != null) & (vEntArr != null))
                {

                    if (vEntTypeArr.GetUpperBound(0) == vEntArr.GetUpperBound(0))
                    {
                        j = 0;

                        foreach (swSketchRelationEntityTypes_e vType in vEntTypeArr)
                        {
                            SketchRelationEntityVO entity = new SketchRelationEntityVO();
                            entity.typeName = "" + vType;

                            entity.type = (int)vType;

                            switch (vType)
                            {
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Unknown:
                                 
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_SubSketch:
                                 
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point:
                                swSkPt = (SketchPoint)vEntArr[j];
                                
                                entity.name = swSkPt.X + "," + swSkPt.Y + "," + swSkPt.Z;
                                
                                    entity.sketchName = sketchName;

                                    entity.id = swModel.Extension.GetPersistReference3(swSkPt);
                                  //  bRet = swSkPt.Select4(false, swSelData);
                              
                                    break;

                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Line:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Arc:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Ellipse:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Parabola:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Spline:

                                    swSkSeg = (SketchSegment)vEntArr[j];
                                    Sketch sk = (Sketch)swSkSeg.GetSketch();
                                    // sk.
                           
                                    entity.id = swModel.Extension.GetPersistReference3(swSkSeg);
                                    entity.name = swSkSeg.GetName();
                                    entity.sketchName = sketchName;
                           
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Hatch:
                         
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Text:
                         
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Plane:
                         
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Cylinder:
                               
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Sphere:
                             
                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Surface:

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Dimension:

                                    break;
                                default:

                                    break;
                            }

                            j = j + 1;

                            entities.Add(entity);
                        }
                    }
                }
           
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
