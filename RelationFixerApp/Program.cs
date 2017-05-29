using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace RelationFixerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();
            String folderPath = args[0];
            String fileName = args[1];
            String mode = args[2];
            System.IO.StreamWriter file; file = new StreamWriter(folderPath+"\\" +fileName+ ".fixSketch.json");
            List<RelationVO> validRelations = new List<RelationVO>();
            List<RelationVO> invalidRelations = new List<RelationVO>();
            string text = File.ReadAllText(folderPath+"\\" + fileName+".valid.json", Encoding.UTF8);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            PartVO validPart = new JavaScriptSerializer().Deserialize<PartVO>(text);
            //Extract Relations from Part
            foreach (SketchVO sketchVO in validPart.sketches)
            {
                validRelations.AddRange(sketchVO.relations);
            }

         


            if (mode.Equals("0"))
            {   
                string text2 = File.ReadAllText(folderPath + "\\" + fileName + ".invalid.json", Encoding.UTF8);
                PartVO invalidPart = new JavaScriptSerializer().Deserialize<PartVO>(text2);
                //Original Code to compare fully defined relations to find the changes
                //Extract Relations From Part
                /*foreach (SketchVO sketchVO in invalidPart.sketches)
                {
                    invalidRelations.AddRange(sketchVO.relations);
                }
                //    jsonFile.Close();
                List<RelationVO> relationsMissing = new List<RelationVO>();
                for (int i = 0; i < validRelations.Count; i++)
                {
                    if (!invalidRelations.Contains(validRelations.ElementAt(i)))
                    {
                        //   file.WriteLine("Valid Relation");
                        relationsMissing.Add(validRelations.ElementAt(i));


                    }
                }

                if (relationsMissing.Count > 0)
                {
                    var json = new JavaScriptSerializer().Serialize(relationsMissing);
                    file.WriteLine(json);
                }*/




            }
            else
            {
                //Mode 1
                //Comapre Geometry
                //Read another Valid File
                string textValid2 = File.ReadAllText(folderPath + "\\" + fileName + ".valid2.json", Encoding.UTF8);
               
                PartVO validPart2 = new JavaScriptSerializer().Deserialize<PartVO>(textValid2);
                //Compare both Valid Geometries
                int validSketch2Index = 0;
                List<FixedRelationVO> fixedRelations = new List<FixedRelationVO>();
                foreach(SketchVO sketchVO in validPart.sketches)
                {
                    SketchGeometry skg1 = sketchVO.geometry;
                    SketchGeometry skg2 = validPart2.sketches.ElementAt(validSketch2Index).geometry; // sketchVO.geometry;

                    //Compare all arcs
                    foreach(ArcVO arc in skg1.arcs)
                    {
                        if (skg2.arcs.Contains(arc)){
                            FixedRelationVO fixedRelationVO = new FixedRelationVO();
                            fixedRelationVO.entity = arc;
                            fixedRelations.Add(fixedRelationVO);
                        }
                    }
                }

                var json = new JavaScriptSerializer().Serialize(fixedRelations);
                file.WriteLine(json);

            }
            /*     for (int i = 0; i < invalidRelations.Count; i++)
                 {
                     if (!validRelations.Contains(invalidRelations.ElementAt(i)))
                     {
                         var json = new JavaScriptSerializer().Serialize(invalidRelations.ElementAt(i));
                       //  file.WriteLine("Invalid Relation");

                       //  file.WriteLine(json);

                     }
                 }*/

            file.Close();

        }
    }
}
