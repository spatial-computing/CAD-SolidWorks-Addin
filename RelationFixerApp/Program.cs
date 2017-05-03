using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RelationFixerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String folderPath = args[0];
            String fileName = args[1];
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

            string text2 = File.ReadAllText(folderPath + "\\" + fileName + ".invalid.json", Encoding.UTF8);
            PartVO invalidPart = new JavaScriptSerializer().Deserialize<PartVO>(text2);

            //Extract Relations From Part
            foreach (SketchVO sketchVO in invalidPart.sketches)
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
