using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Runtime.Remoting;
using System.Runtime.InteropServices;
using System.IO;

using System.Threading;

namespace TestSolidWorksAddin
{
    [ComVisible(true)]
    [ProgId(SWTASKPANE_PROGID)]
   
    public partial class SWTaskpaneHost : UserControl
    {
        public const string SWTASKPANE_PROGID = "TestSolidWorksAddin.SWTaskPane_SwAddin_AJ";
        public SldWorks mSWApplication;
        System.IO.StreamWriter file;
        public SWTaskpaneHost()
        {
            InitializeComponent();
      //      button1.Click += Button_Click;
        }


        
  private List<SketchVO> getAllSketches(PartDoc swPart)
        {
            List<SketchVO> sketches = new List<SketchVO>();
           
            Feature swFeat = (Feature)swPart.FirstFeature();
            while (swFeat != null)
            {
                String featureName = swFeat.Name;
                if (swFeat.GetTypeName2() == "HoleWzd")
                {
                    Feature subFeat = swFeat.GetFirstSubFeature();
                    while (subFeat != null && subFeat.GetTypeName2() == "ProfileFeature")
                    {
                        SketchVO skVO = new SketchVO();
                        skVO.name = subFeat.Name;
                        skVO.swSketch = (Sketch)subFeat.GetSpecificFeature2();
                        sketches.Add(skVO);
                        subFeat = subFeat.GetNextSubFeature();
                    }
                }
                else
                        if (swFeat.GetTypeName2() == "ProfileFeature")
                {
                   
                    SketchVO skVO = new SketchVO();
                    skVO.name = swFeat.Name;
                    skVO.swSketch = (Sketch)swFeat.GetSpecificFeature2();
                  dynamic children =  swFeat.GetChildren();
                    sketches.Add(skVO);
                }
                swFeat = swFeat.GetNextFeature();
           

     
            }
            return sketches;
        }

        private List<SketchBodyVO> getAllSketchesWithBody(PartDoc swPart)
        {
            List<SketchBodyVO> sketchBodies = new List<SketchBodyVO>();
            
            ModelDoc2 swModel = (ModelDoc2)swPart;
          
            Feature swFeat = (Feature)swPart.FirstFeature();
            while (swFeat != null)
            {
                List<SketchGeometry> sketchBodyList = new List<SketchGeometry>();
                SketchBodyVO sketchBody = new SketchBodyVO();
                if (swFeat.GetTypeName2() == "HoleWzd")
                {
                    object[] parents = (object[])swFeat.GetParents();
                    if (parents != null)
                    {
                        foreach (Feature parent in parents)
                        {
                            object[] facesArray = parent.GetFaces();
                            if (facesArray != null && facesArray.Length > 0)
                            {
                                Face parentFace = (Face)facesArray[0];
                                if (parentFace != null)
                                {
                                    //  Feature p = parent.GetFaces();
                                    Body2 body = parentFace.GetBody();
                                    if (body != null)
                                    {
                                        //      sketchBodyList.Add(getBodyFromFace(parentFace, swModel));
                                        sketchBodyList.Add(getBodyFromSketch(body, swModel));

                                    }
                                }
                            }
                        }

                    }
                    Feature subFeat = swFeat.GetFirstSubFeature();
                    while (subFeat != null && subFeat.GetTypeName2() == "ProfileFeature")
                    {

                        sketchBody.name = subFeat.Name;
                        sketchBody.swSketch = (Sketch)subFeat.GetSpecificFeature2();
                        sketchBody.sketchGeometry = getGeometryFromSketch(sketchBody);

                        //Add code to get All parent body
  
            
                        sketchBody.sketchBodies = sketchBodyList;
                        sketchBodies.Add(sketchBody);
                        subFeat = subFeat.GetNextSubFeature();
                    }
                }
                else
                        if (swFeat.GetTypeName2() == "ProfileFeature")
                {

                    // SketchVO skVO = new SketchVO();
                    sketchBody.name = swFeat.Name;
                    sketchBody.swSketch = (Sketch)swFeat.GetSpecificFeature2();
                    sketchBody.sketchGeometry = getGeometryFromSketch(sketchBody);

                    /*        Feature parents = swFeat.GetOwnerFeature();
                            if (parents != null)
                            {
                              //  SwConst.swSelectType_e

                              /*  if (selected)
                                {
                                //    SelectionMgr swSelMgr = swModel.SelectionManager;
                                    swSelMgr.GetSelectedObjectType(1);
                                   // swModel.ClearSelection2(true);
                                  //  dynamic v = b.get;

                                }*/
                    /*       object[] facesArray = parents.GetFaces();
                           //Add code to get All parent body
                           for (int i = 0; i < facesArray.Length-(facesArray.Length - 1); i++)
                           {
                               Face parentFace = (Face)facesArray[i];


                              // Feature p2 = swFeat.GetOwnerFeature();
                               if (parentFace != null)
                               {
                                 //  Feature p = parent.GetFaces();
                                   Body2 body = parentFace.GetBody();
                                   if (body != null)
                                          {
                                       //      sketchBodyList.Add(getBodyFromFace(parentFace, swModel));
                                       sketchBodyList.Add(getBodyFromSketch(body, swModel));

                                    }
                               }
                           }
                       }*/
                    object[] parents = (object[])swFeat.GetParents();
                    if (parents != null)
                    {
                        foreach (Feature parent in parents)
                        {
                            object[] facesArray = parent.GetFaces();
                            if (facesArray != null && facesArray.Length > 0)
                            {
                                Face parentFace = (Face)facesArray[0];
                                if (parentFace != null)
                                {
                                    //  Feature p = parent.GetFaces();
                                    Body2 body = parentFace.GetBody();
                                    if (body != null)
                                    {
                                        //      sketchBodyList.Add(getBodyFromFace(parentFace, swModel));
                                        sketchBodyList.Add(getBodyFromSketch(body, swModel));

                                    }
                                }
                            }
                        }

                    }
                    sketchBody.sketchBodies = sketchBodyList;
                    sketchBodies.Add(sketchBody);

                }
                
                swFeat = swFeat.GetNextFeature();
            }
            return sketchBodies;
        }

        private SketchVO getSketchWithName(PartDoc swPart,String sketchName)
        {
         //   SketchVO sketche = new SketchVO();

            Feature swFeat = (Feature)swPart.FirstFeature();
            while (swFeat != null)
            {
                if (swFeat.GetTypeName2() == "HoleWzd")
                {
                    Feature subFeat = swFeat.GetFirstSubFeature();
                    while (subFeat != null && subFeat.GetTypeName2() == "ProfileFeature" )
                    {
                        if (subFeat.Name.Equals(sketchName))
                        {
                            SketchVO skVO = new SketchVO();
                            skVO.name = subFeat.Name;
                            skVO.swSketch = (Sketch)subFeat.GetSpecificFeature2();
                            return skVO;// sketches.Add(skVO);
                        }
                        subFeat = subFeat.GetNextSubFeature();
                    }
                }
                else
                        if (swFeat.GetTypeName2() == "ProfileFeature" && swFeat.Name.Equals(sketchName))
                {
                    SketchVO skVO = new SketchVO();
                    skVO.name = swFeat.Name;
                    skVO.swSketch = (Sketch)swFeat.GetSpecificFeature2();
                    return skVO;
                   // sketches.Add(skVO);
                }
                swFeat = swFeat.GetNextFeature();
            }
            return null;
        }



        private void Button_Click(object sender, EventArgs e)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel =null;
            SelectionMgr swSelMgr = null;
            PartDoc swPart =null;
            SketchManager swSkMgr = null;         
            swModel = (ModelDoc2)swApp.ActiveDoc;
            file = new System.IO.StreamWriter(swModel.GetPathName() + ".relations");
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swSkMgr = swModel.SketchManager;

            swPart = (PartDoc)swModel;
            PartVO partVO = new PartVO();
       
            //TODO:Get Actual PartName
            partVO.name = "Test";
            try
            {
               partVO.sketches= getAllSketches(swPart);
                for(int i = 0; i < partVO.sketches.Count; i++)
                {
                    getRelationsFromSketch(partVO.sketches.ElementAt(i));
                }
            }catch(Exception e2)
            {
                file.Close();
            }
            System.IO.StreamWriter jsonFile = new System.IO.StreamWriter(swModel.GetPathName() + ".json");
            var json = new JavaScriptSerializer().Serialize(partVO);

            jsonFile.WriteLine(json);
            jsonFile.Close();
            file.Close();



        }

      private SketchVO getRelationsFromSketch(SketchVO skVO)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            SelectData swSelData = null;
            Sketch swSketch = skVO.swSketch;
            SketchRelationManager swSkRelMgr = null;
            SketchRelation swSkRel = null;
            DisplayDimension dispDim = null;
            object[] vSkRelArr = null;
            int[] vEntTypeArr = null;
            object[] vEntArr = null;
            object[] vDefEntArr = null;
            SketchSegment swSkSeg = null;
            SketchPoint swSkPt = null;
            int i = 0;
            int j = 0;
            bool bRet = false;
       
            swSkRelMgr = swSketch.RelationManager;

       

            vSkRelArr = (object[])swSkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
            if ((vSkRelArr == null))
            {
                file.WriteLine("No Relations found");
                
                return skVO;
            }

            foreach (SketchRelation vRel in vSkRelArr)
            {
                RelationVO skRel = new RelationVO();
                swSkRel = (SketchRelation)vRel;
               


                skRel.typeName = Enum.GetName(typeof(swConstraintType_e), swSkRel.GetRelationType());
      
                file.WriteLine("    Relation(" + i + ")");
                file.WriteLine("      Type         = " + swSkRel.GetRelationType());
                skRel.type = swSkRel.GetRelationType();

                //Dimensions need to check if required
                dispDim = (DisplayDimension)swSkRel.GetDisplayDimension();
                if (dispDim != null)
                {
                    skRel.dimensionValue = dispDim.GetDimension().GetSystemValue2(""); ;
                    file.WriteLine("      Display dimension         = " + Enum.GetName(typeof(swDimensionType_e), dispDim.GetType())+" "+dispDim.GetDimension().GetSystemValue2(""));
                  
                }

                vEntTypeArr = (int[])swSkRel.GetEntitiesType();
                vEntArr = (object[])swSkRel.GetEntities();

                vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if ((vDefEntArr == null))
                {
                }
                else
                {
                    file.WriteLine("    Number of definition entities in this relation: " + vDefEntArr.GetUpperBound(0));
                }

                if ((vEntTypeArr != null) & (vEntArr != null))
                {

                    if (vEntTypeArr.GetUpperBound(0) == vEntArr.GetUpperBound(0))
                    {
                        j = 0;

                        foreach (swSketchRelationEntityTypes_e vType in vEntTypeArr)
                        {
                            SketchRelationEntityVO entity = new SketchRelationEntityVO();
                            entity.typeName = ""+vType;
                            file.WriteLine("        EntType    = " + vType);
                           
                            entity.type = (int)vType;
                           
                            switch (vType)
                            {
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Unknown:
                                    file.WriteLine("          Not known");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_SubSketch:
                                    file.WriteLine("SubSketch");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point:
                                    swSkPt = (SketchPoint)vEntArr[j];
                                    
                                    //  Debug.Assert((swSkPt != null));
                                    
                                    file.WriteLine("          SkPoint ID = [" + ((int[])(swSkPt.GetID()))[0] + ", " + ((int[])(swSkPt.GetID()))[1] + "]");
                                  //  entity.id = "[" + ((int[])(swSkPt.GetID()))[0] + ", " + ((int[])(swSkPt.GetID()))[1] + "]";
                                    entity.name = swSkPt.X+","+swSkPt.Y+","+swSkPt.Z ;
                                    entity.sketchName = skVO.name;
                                    entity.id=swModel.Extension.GetPersistReference3(swSkPt);
                                    bRet = swSkPt.Select4(false, swSelData);
                             //       SketchPoint sk2 = (SketchPoint)swModel.Extension.GetObjectByPersistReference3(entity.id);
                            //        string t = sk2.GetID()[0]+"," +sk2.GetID()[1];
                                 //   P swModel.Extension.GetPersistReference3(swSelData);

                                    // swSkPt.name
                                    //
                                    break;

                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Line:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Arc:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Ellipse:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Parabola:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Spline:

                                    swSkSeg = (SketchSegment)vEntArr[j];
                                    Sketch sk = (Sketch)swSkSeg.GetSketch();
                                   // sk.
                                    file.WriteLine("          Name = " + swSkSeg.GetName() + " SkSeg   ID = [" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]");
                                    //entity.id = "[" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]";
                                    entity.id = swModel.Extension.GetPersistReference3(swSkSeg);
                                    entity.name = swSkSeg.GetName();
                                    entity.sketchName = skVO.name;
                                    bRet = swSkSeg.Select4(false, swSelData);

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Hatch:
                                    file.WriteLine("Hatch");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Text:
                                    file.WriteLine("Text");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Plane:
                                    file.WriteLine("Plane");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Cylinder:
                                    file.WriteLine("Cylinder");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Sphere:
                                    file.WriteLine("Sphere");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Surface:
                                    file.WriteLine("Surface");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Dimension:
                                    file.WriteLine("Dimension");

                                    break;
                                default:
                                    file.WriteLine("Something else");

                                    break;
                            }

                            j = j + 1;

                            skRel.entities.Add(entity);
                        }
                    }
                }

                i = i + 1;
                skVO.relations.Add(skRel);
            }
            return skVO;
           
        }

        private SketchVO getRelationsFromSketchWithLog(SketchVO skVO)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;

            SelectData swSelData = null;
            Sketch swSketch = skVO.swSketch;
            SketchRelationManager swSkRelMgr = null;
            SketchRelation swSkRel = null;
            DisplayDimension dispDim = null;
            object[] vSkRelArr = null;
            int[] vEntTypeArr = null;
            object[] vEntArr = null;
            object[] vDefEntArr = null;
            SketchSegment swSkSeg = null;
            SketchPoint swSkPt = null;
            int i = 0;
            int j = 0;
            bool bRet = false;

            swSkRelMgr = swSketch.RelationManager;

            System.IO.StreamWriter logFile = new System.IO.StreamWriter(swModel.GetPathName() + ".relationsLog");

            vSkRelArr = (object[])swSkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
            if ((vSkRelArr == null))
            {
                logFile.WriteLine("No Relations found");

                return skVO;
            }

            foreach (SketchRelation vRel in vSkRelArr)
            {
                RelationVO skRel = new RelationVO();
                swSkRel = (SketchRelation)vRel;



                skRel.typeName = Enum.GetName(typeof(swConstraintType_e), swSkRel.GetRelationType());

                logFile.WriteLine("    Relation(" + i + ")");
                logFile.WriteLine("      Type         = " + swSkRel.GetRelationType());
                skRel.type = swSkRel.GetRelationType();

                //Dimensions need to check if required
                dispDim = (DisplayDimension)swSkRel.GetDisplayDimension();
                if (dispDim != null)
                {
                    skRel.dimensionValue = dispDim.GetDimension().GetSystemValue2(""); ;
                    logFile.WriteLine("      Display dimension         = " + Enum.GetName(typeof(swDimensionType_e), dispDim.GetType()) + " " + dispDim.GetDimension().GetSystemValue2(""));

                }

                vEntTypeArr = (int[])swSkRel.GetEntitiesType();
                vEntArr = (object[])swSkRel.GetEntities();

                vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if ((vDefEntArr == null))
                {
                }
                else
                {
                    logFile.WriteLine("    Number of definition entities in this relation: " + vDefEntArr.GetUpperBound(0));
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
                            logFile.WriteLine("        EntType    = " + vType);

                            entity.type = (int)vType;

                            switch (vType)
                            {
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Unknown:
                                    logFile.WriteLine("          Not known");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_SubSketch:
                                    logFile.WriteLine("SubSketch");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point:
                                    swSkPt = (SketchPoint)vEntArr[j];

                                    //  Debug.Assert((swSkPt != null));

                                    logFile.WriteLine("          SkPoint ID = [" + ((int[])(swSkPt.GetID()))[0] + ", " + ((int[])(swSkPt.GetID()))[1] + "]");
                            
                                    entity.name = swSkPt.X + "," + swSkPt.Y + "," + swSkPt.Z;
                                    entity.sketchName = skVO.name;
                                    entity.id = swModel.Extension.GetPersistReference3(swSkPt);
                                    bRet = swSkPt.Select4(false, swSelData);
                           
                                    break;

                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Line:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Arc:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Ellipse:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Parabola:
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Spline:

                                    swSkSeg = (SketchSegment)vEntArr[j];
                                    Sketch sk = (Sketch)swSkSeg.GetSketch();
                                    // sk.
                                    logFile.WriteLine("          Name = " + swSkSeg.GetName() + " SkSeg   ID = [" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]");
                                    //entity.id = "[" + ((int[])(swSkSeg.GetID()))[0] + ", " + ((int[])(swSkSeg.GetID()))[1] + "]";
                                    entity.id = swModel.Extension.GetPersistReference3(swSkSeg);
                                    entity.name = swSkSeg.GetName();
                                    entity.sketchName = skVO.name;
                                    bRet = swSkSeg.Select4(false, swSelData);

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Hatch:
                                    logFile.WriteLine("Hatch");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Text:
                                    logFile.WriteLine("Text");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Plane:
                                    logFile.WriteLine("Plane");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Cylinder:
                                    logFile.WriteLine("Cylinder");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Sphere:
                                    logFile.WriteLine("Sphere");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Surface:
                                    logFile.WriteLine("Surface");

                                    break;
                                case swSketchRelationEntityTypes_e.swSketchRelationEntityType_Dimension:
                                    logFile.WriteLine("Dimension");

                                    break;
                                default:
                                    logFile.WriteLine("Something else");

                                    break;
                            }

                            j = j + 1;

                            skRel.entities.Add(entity);
                        }
                    }
                }

                i = i + 1;
                skVO.relations.Add(skRel);
            }
            return skVO;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void fullyDefineSketch(SketchVO sketch,ModelDoc2 swModel)
        {
            SketchManager swSketchManager = swModel.SketchManager;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            ModelDocExtension swModelExtension = swModel.Extension;
          //  swSketchManager.InsertSketch(true);
            swModelExtension.SelectByID2(sketch.name, "SKETCH", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
            swModel.EditSketch();
            swModel.DeleteAllRelations();
            swSketchManager.FullyDefineSketch(true, true, (int)swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Vertical |
                       (int)swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Horizontal |
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Coincident |
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Colinear | 
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Concentric |
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Equal | 
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Midpoint |
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Parallel | 
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Perpendicular | 
                       (int) swSketchFullyDefineRelationType_e.swSketchFullyDefineRelationType_Tangent 
                       , true, 1, null, 1, null, 1, 1);
             // getRelationsFromSketch(sketch);
            // swModel.EditSketch();
          //  swModel.ClearSelection2(true);
          //  swModel.Extension.RunCommand((int)swCommands_e.swCommands_Exit_Sketch, "");
         //   SendKeys.Send("^P");
          // swSketchManager.InsertSketch(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Fully Defines a sketch
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            PartDoc swPart = (PartDoc)swModel;
      
            file = new System.IO.StreamWriter(swModel.GetPathName() + ".fullyDefineRelations");
          

           
            PartVO partVO = new PartVO();

            //TODO:Get Actual PartName
            partVO.name = "Test";
            try
            {
                partVO.sketches = getAllSketches(swPart);
                swModel.Extension.StartRecordingUndoObject();
                for (int i = 0; i < partVO.sketches.Count; i++)
                {
                  
                    fullyDefineSketch(partVO.sketches.ElementAt(i),swModel);
                    swModel.Extension.RunCommand((int)swCommands_e.swCommands_Edit_Exit_No_Save, "");
                    // getRelationsFromSketch(partVO.sketches.ElementAt(i));
                    // swModel.EditUndo2(100);

                    //partVO.sketches.Count
                }
                swModel.Extension.FinishRecordingUndoObject2("TEST", true);

            }
            catch (Exception e2)
            {
                file.Close();
            }
            System.IO.StreamWriter jsonFile = new System.IO.StreamWriter(swModel.GetPathName() + ".fullyDefineRelations.json");
            var json = new JavaScriptSerializer().Serialize(partVO);

            jsonFile.WriteLine(json);
            jsonFile.Close();
            file.Close();


        }

        private void button4_Click(object sender, EventArgs e)
        {

            //On Each click keep on appending the relations. Can be made smarter by logging only differecnce in files
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            PartDoc swPart = (PartDoc)swModel;

            file = new System.IO.StreamWriter(swModel.GetPathName() + ".invalidRelations");



            PartVO partVO = new PartVO();

            //TODO:Get Actual PartName
            partVO.name = "Test";
            try
            {
                partVO.sketches = getAllSketches(swPart);
                for (int i = 0; i < partVO.sketches.Count; i++)
                {
                    fullyDefineSketch(partVO.sketches.ElementAt(i), swModel);
                    getRelationsFromSketch(partVO.sketches.ElementAt(i));
                    swModel.Extension.RunCommand((int)swCommands_e.swCommands_Edit_Exit_No_Save, "");
                }
            }
            catch (Exception e2)
            {
                file.Close();
            }
            System.IO.StreamWriter jsonFile = new System.IO.StreamWriter(swModel.GetPathName() + ".invalid.json");
            var json = new JavaScriptSerializer().Serialize(partVO);

            jsonFile.WriteLine(json);
            jsonFile.Close();
            file.Close();


        }

        private void button5_Click(object sender, EventArgs e)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            SelectionMgr swSelMgr = (SelectionMgr)swModel.SelectionManager;
            PartDoc swPart = (PartDoc)swModel;

            file = new System.IO.StreamWriter(swModel.GetPathName() + ".validRelations");



            PartVO partVO = new PartVO();

            //TODO:Get Actual PartName
            partVO.name = "Test";
            try
            {
                partVO.sketches = getAllSketches(swPart);
                for (int i = 0; i < partVO.sketches.Count; i++)
                {
                    fullyDefineSketch(partVO.sketches.ElementAt(i), swModel);
                    getRelationsFromSketch(partVO.sketches.ElementAt(i));
                    swModel.Extension.RunCommand((int)swCommands_e.swCommands_Edit_Exit_No_Save, "");

                    // getRelationsFromSketch(partVO.sketches.ElementAt(i));

                }
            }
            catch (Exception e2)
            {
                file.Close();
            }
            System.IO.StreamWriter jsonFile = new System.IO.StreamWriter(swModel.GetPathName() + ".valid.json");
            var json = new JavaScriptSerializer().Serialize(partVO);

            jsonFile.WriteLine(json);
            jsonFile.Close();
            file.Close();

        }

        private void button6_Click(object sender, EventArgs e)
        {

            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            SelectionMgr selectMgr = swModel.SelectionManager;
            PartDoc swPart = (PartDoc)swModel;
            int errorCode;
            System.IO.StreamWriter logFile = new System.IO.StreamWriter(swModel.GetPathName() + ".fixSketch.logs");

             System.IO.StreamReader file = new System.IO.StreamReader(swModel.GetPathName() + ".fixSketch.json");
          //  System.IO.StreamReader file = new System.IO.StreamReader("C:\\Users\\aj\\Documents\\3Cylinders\\" + ".fixSketch.json");
            string json = file.ReadToEnd();

            // deserialize json
            List<RelationVO> relations = new JavaScriptSerializer()
                .Deserialize<List<RelationVO>>(json);
            HashSet<string> sketchNames = new HashSet<string>();
            foreach (RelationVO relation in relations)
            {
                sketchNames.Add(relation.entities[0].sketchName);
            }

            ModelDocExtension swModelExtension = swModel.Extension;
            SketchManager swSketchManager = swModel.SketchManager;
            swModelExtension.SelectByID2(relations[0].entities[0].sketchName, "SKETCH", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);

            foreach (string sketchNam in sketchNames)
            {
                String sketchName = sketchNam;

                // sketchName = relations[0].entities[0].sketchName;

                SketchVO sketch = getSketchWithName(swPart, sketchName);
                SketchRelationManager swSkRelMgr = sketch.swSketch.RelationManager;

                swModel.DeleteAllRelations();
                fullyDefineSketch(sketch, swModel);
                Object[] vSkRelArr = (object[])swSkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
                if ((vSkRelArr == null))
                {
                    continue;
                }

                swModel.EditSketch();
                //      for (int i = 0; i < relations.Count(); i++)
                //      {
                //          RelationVO relation = relations.ElementAt(i);

               
                List<SketchRelation> deleteRelationList = new List<SketchRelation>();
                foreach (SketchRelation vRel in vSkRelArr)
                {

                    SketchRelation swSkRel = (SketchRelation)vRel;
                    try
                    {
                        RelationVO swRelation = new RelationVO(swSkRel, sketchName, swModel);
                        if(!relations.Contains(swRelation) && swRelation.entities[0].sketchName == sketchName)
                            swSkRelMgr.DeleteRelation(swSkRel);
                    }
                    catch (Exception e1)
                    {
                        logFile.WriteLine("Error creating relation on Sketch "+sketchName);
                        logFile.WriteLine("Failed with error "+e1);
                        logFile.WriteLine("");

                    }
                }

 


                //    }
                swSketchManager.InsertSketch(true);
            }

          
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ModelDoc2 swModel;
            ModelView swModelView;
            ModelDocExtension swModelDocExt;
            SketchManager swSketchManager;
            SketchSegment swSketchSegment;
            Feature swFeature;
            FeatureManager swFeatureManager;
            SelectionMgr swSelectionManager;

            bool status;
            int docErrors =0;
            SldWorks swApp = mSWApplication;
   
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swModel.Extension.RunCommand((int)swCommands_e.swCommands_Edit_Exit_No_Save, "");
           // swModelDocExt = (ModelDocExtension)swModel.Extension;

           // swModelView = (ModelView)swModel.ActiveView;
           // swModelView.FrameState = (int)swWindowState_e.swWindowMaximized;

           // swSketchManager = (SketchManager)swModel.SketchManager;

           // swSketchManager.InsertSketch(true);
           // status = swModelDocExt.SelectByID2("Front Plane", "PLANE", -0.0692248508634211, 0.0392379182115397, 0.00987134779060705, false, 0, null, 0);
           // swModel.ClearSelection2(true);

           // object vSkLines = null;
           // vSkLines = swSketchManager.CreateCornerRectangle(-0.0891172006155176, 0.0314069429482, 0, -0.0425302352423542, 0.00601966406507166, 0);
           // swModel.ClearSelection2(true);

           // swSketchManager.InsertSketch(true);
           // status = swModelDocExt.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
           // swModel.ClearSelection2(true);

           // swSketchSegment = (SketchSegment)swSketchManager.CreateCircle(0.009029, 0.03036, 0.0, 0.021854, 0.019629, 0.0);
           // swModel.ClearSelection2(true);

           // swSketchManager.InsertSketch(true);
           // status = swModelDocExt.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);

           // swModel.ClearSelection2(true);
           // swSketchSegment = (SketchSegment)swSketchManager.CreateEllipse(0.0306284568434307, 0.00619756829649987, 0, 0.0309763470298606, 0.00997419305453208, 0, 0.0286971648691861, 0.00637547252792807, 0);
           // swModel.ClearSelection2(true);

           // swSketchManager.InsertSketch(true);
           // status = swModelDocExt.SelectByID2("Front Plane", "PLANE", 0, 0, 0, false, 0, null, 0);
           // swModel.ClearSelection2(true);

           // swSketchSegment = (SketchSegment)swSketchManager.CreateEllipse(0.0240620641310443, 0.0131240684851264, 0, 0.0771974468433887, 0.0706711158113391, 0, 0.000886560440335415, 0.0345228945826079, 0);
           // swModel.ClearSelection2(true);
           // swSketchManager.InsertSketch(true);

           // status = swModelDocExt.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 0, null, 0);
           // swModel.ClearSelection2(true);
           // status = swModelDocExt.SelectByID2("Sketch1", "SKETCH", 0, 0, 0, false, 4, null, 0);

           // swFeatureManager = (FeatureManager)swModel.FeatureManager;
           // swFeature = (Feature)swFeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.00254, 0.00254, false, false, false,
           // false, 0.0174532925199433, 0.0174532925199433, false, false, false, false, true, true, true,
           // 0, 0, false);
           // swSelectionManager = (SelectionMgr)swModel.SelectionManager;
           // swSelectionManager.EnableContourSelection = false;

           // // Start recording the SOLIDWORKS Undo object
           // swModelDocExt.StartRecordingUndoObject();
           // status = swModelDocExt.SelectByID2("Sketch2", "SKETCH", 0, 0, 0, false, 4, null, 0);
           // swFeature = (Feature)swFeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.00254, 0.00254, false, false, false,
           // false, 0.0174532925199433, 0.0174532925199433, false, false, false, false, true, true, true,
           // 0, 0, false);
           //// swModelDocExt.StartRecordingUndoObject();
           // swSelectionManager.EnableContourSelection = false;
           // status = swModelDocExt.SelectByID2("Sketch4", "SKETCH", 0, 0, 0, false, 0, null, 0);
           // swFeature = (Feature)swFeatureManager.FeatureExtrusion2(true, false, false, 0, 0, 0.00254, 0.00254, false, false, false,
           // false, 0.0174532925199433, 0.0174532925199433, false, false, false, false, true, true, true,
           // 0, 0, false);
           // swSelectionManager.EnableContourSelection = false;

           // // End recording the SOLIDWORKS Undo object with name "API Undo" and hide it in the Undo list
           // swModelDocExt.FinishRecordingUndoObject2("API Undo", false);
           // swModel.ClearSelection2(true);
           // status = swModelDocExt.SelectByID2("Sketch3", "SKETCH", 0, 0, 0, false, 4, null, 0);
           // swFeature = (Feature)swFeatureManager.FeatureCut3(true, false, true, 0, 0, 0.00254, 0.00254, false, false, false,
           // false, 0.0174532925199433, 0.0174532925199433, false, false, false, false, false, true, true,
           // true, true, false, 0, 0, false);
           // swSelectionManager.EnableContourSelection = false;

        }

        private void button8_Click(object sender, EventArgs e)
        {
         //   double x = double.Parse(textBox1.Text);
         //   double y = double.Parse(textBox2.Text);
          //  double z = double.Parse(textBox3.Text);

          //  byte[] b = new byte[] { 40, 35, 0, 0, 6, 0, 0, 0, 255, 254, 255, 0, 0, 0, 0, 0, 44, 0, 0, 0, 255, 255, 1, 0, 13, 0, 115, 103, 80, 111, 105, 110, 116, 72, 97, 110, 100, 108, 101, 0, 0, 255, 255, 255, 255, 0, 0, 0, 0 };
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = (ModelDoc2)swApp.ActiveDoc;
            SelectionMgr selectMgr = swModel.SelectionManager;
            PartDoc swPart = (PartDoc)swModel;
            int errorCode;

        //   List<SketchVO> sketches =  getAllSketches(swPart);

            System.IO.StreamReader file = new System.IO.StreamReader(swModel.GetPathName() + ".fixSketch.json");
            string json = file.ReadToEnd();

            // deserialize json
            List<RelationVO> relations = new JavaScriptSerializer()
                .Deserialize<List<RelationVO>>(json);

            ModelDocExtension swModelExtension = swModel.Extension;
            SketchManager swSketchManager = swModel.SketchManager;
            swModelExtension.SelectByID2(relations[0].entities[0].sketchName, "SKETCH", 0, 0, 0, false, 0, null, (int)swSelectOption_e.swSelectOptionDefault);
            String sketchName = relations[0].entities[0].sketchName;

            SketchVO sketch = getSketchWithName(swPart, sketchName);
            SketchRelationManager swSkRelMgr = sketch.swSketch.RelationManager;


            fullyDefineSketch(sketch, swModel);
            Object[] vSkRelArr = (object[])swSkRelMgr.GetRelations((int)swSketchRelationFilterType_e.swAll);
            if ((vSkRelArr == null))
            {
                return;
            }

            swModel.EditSketch();
            for (int i = 0; i < relations.Count(); i++)
            {
                RelationVO relation = relations.ElementAt(i);
                
             
                
             foreach (SketchRelation vRel in vSkRelArr)
                {
                 SketchRelation   swSkRel = (SketchRelation)vRel;
              
                    RelationVO swRelation = new RelationVO(swSkRel, sketchName, swModel);
                    if(!swRelation.Equals(relation))
                        swSkRelMgr.DeleteRelation(swSkRel);
                }


             

            }
            swSketchManager.InsertSketch(true);
      

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = null;
            SelectionMgr swSelMgr = null;
            PartDoc swPart = null;
            SketchManager swSkMgr = null;
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swSkMgr = swModel.SketchManager;
            SolidWorks.Interop.sldworks.View view = null;
            swPart = (PartDoc)swModel;
            
            /*  object[] bodies = swPart.GetBodies2((int)SwConst.swBodyType_e.swSolidBody, true);

              List<SketchGeometry> skgListAll = new List<SketchGeometry>();


              foreach (Body2 body in bodies)
              {
                  skgListAll.Add(getBodyFromSketch(body,swModel));

              }



              writeToFile(swModel, ".BodyDetails.json", skgListAll);*/

            getAllSketchesWithBody(swPart);

     /*       List<SketchVO> sketches = getAllSketches(swPart);
            List<SketchGeometry> skgList = new List<SketchGeometry>();
            for (int i = 0; i < sketches.Count; i++)
            {
                skgList.Add(getGeometryFromSketch(sketches.ElementAt(i)));
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(swModel.GetPathName() + ".SketchDetails.json");
            var json = new JavaScriptSerializer().Serialize(skgList);

            file.WriteLine(json);
            file.Close();*/

            // 2. match lines
            /*        SketchGeometry skg = skgList[0];

                    foreach(PointVO point in skg.points) {
                        if (!skG.points.Contains(point))
                        {
                            PointVO pointNotMatched = point;
                        }
                        }
                    HashSet<LineVO> hsLine = new HashSet<LineVO>();
                    hsLine.Add(new LineVO(1.0, 1.0, 2.0, 0.0, 0.0, 0.0));
                    hsLine.Add(new LineVO(1.0,2.1,3.2,0,0,0));

                    hsLine.Add(new LineVO(1.0000000001, 1.0, 2.0, 0.0, 0.0, 0.0));

                    System.IO.StreamWriter debug = new System.IO.StreamWriter(swModel.GetPathName() + ".debug");

                    debug.WriteLine(hsLine.Count());
                    debug.Close();*/
         /*   for (int i = 0; i < sketches.Count; i++) {
                getRelationsFromSketch(sketches.ElementAt(i));
                    }*/


            // 3. infer rules
            // 4. apply relations
            // 5. move 2-4 functionality to RelnFixer
        }

        private void writeToFile(ModelDoc2 swModel, string fileName, List<SketchGeometry> skgListAll)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(swModel.GetPathName() + fileName);
            var json = new JavaScriptSerializer().Serialize(skgListAll);

            file.WriteLine(json);
            file.Close();
        }


        private SketchGeometry getBodyFromFace(Face face, ModelDoc2 swModel)
        {
            SketchGeometry sketchBody = new SketchGeometry();
            List<LineVO> lines = new List<LineVO>();
            List<PointVO> points = new List<PointVO>();
            //dynamic vertices = face.GetVertices();
            dynamic edges = face.GetEdges();
            
           /* foreach (Vertex vertex in vertices)
            {

                PointVO point = getVertexPoint(swModel, vertex.GetPoint());
                points.Add(point);
            }*/

           // sketchBody.points = points;

            foreach (Edge edge in edges)
            {
                if (edge.GetStartVertex() != null)
                {
                    Vertex startVertex = edge.GetStartVertex();
                    Vertex endVertex = edge.GetEndVertex();
                    LineVO line = new LineVO();
                    line.start = getVertexPoint(swModel, startVertex.GetPoint());
                    line.end = getVertexPoint(swModel, endVertex.GetPoint());
                    line.name = edge.GetID().ToString();
                    line.key = line.start.key + "##" + line.end.key + "##";
                    lines.Add(line);
                }
            }
            sketchBody.lines = lines;
            return sketchBody;
        }
       private SketchGeometry getBodyFromSketch(Body2 body, ModelDoc2 swModel)
        {
            SketchGeometry sketchBody = new SketchGeometry();
            List<LineVO> lines = new List<LineVO>();
            List<PointVO> points = new List<PointVO>();
            dynamic vertices = body.GetVertices();
            dynamic edges = body.GetEdges();
            
            foreach (Vertex vertex in vertices)
            {

                PointVO point = getVertexPoint(swModel, vertex.GetPoint());
                points.Add(point);
            }

            sketchBody.points = points;

            foreach (Edge edge in edges)
            {
                if (edge.GetStartVertex() != null)
                {
                    Vertex startVertex = edge.GetStartVertex();
                    Vertex endVertex = edge.GetEndVertex();
                    LineVO line = new LineVO();
                    line.start = getVertexPoint(swModel, startVertex.GetPoint());
                    line.end = getVertexPoint(swModel, endVertex.GetPoint());
                    line.name = edge.GetID().ToString();
                    line.key = line.start.key + "##" + line.end.key + "##";
                    lines.Add(line);
                }
            }
            sketchBody.lines = lines;
            return sketchBody;
        }

        private SketchGeometry getGeometryFromSketch(SketchBodyVO sketchBody)
        {
            SketchVO skVO = new SketchVO();

                skVO.name = sketchBody.name;
            skVO.relations = sketchBody.relations;
            skVO.swSketch = sketchBody.swSketch;
          return  getGeometryFromSketch(skVO);
        }

        private SketchGeometry getGeometryFromSketch(SketchVO sketch)
        {
            SldWorks swApp = mSWApplication;
            ModelDoc2 swModel = null;
            SelectionMgr swSelMgr = null;
            PartDoc swPart = null;
            SketchManager swSketchMgr = null;
           
             SketchGeometry skg = new SketchGeometry();
            swModel = (ModelDoc2)swApp.ActiveDoc;
            swSelMgr = (SelectionMgr)swModel.SelectionManager;
            swSketchMgr = swModel.SketchManager;

         


            String sketchName = sketch.name;
            bool selected = swModel.Extension.SelectByID2(sketchName, "SKETCH", 0, 0, 0, false, 0, null, 0);
            skg.sketchName = sketchName;

            if (selected)
            {
                Sketch swSketch = sketch.swSketch;
                
                /*      swSketch.GetSketchSegments();
                      swSketch.GetArcs();
                      swSketch.GetLines2(true);
                      swSketch.GetSketchPoints2()*/
                List<LineVO> lines = new List<LineVO>();
                List<PointVO> points = new List<PointVO>();
                List<CircleVO> circles = new List<CircleVO>();
                List<ArcVO> arcs = new List<ArcVO>();

                dynamic sketchSegments = swSketch.GetSketchSegments();
           //     swSketch    
                if (sketchSegments != null)
                {

                    foreach (dynamic skSeg in sketchSegments)
                    {

                        swSketchSegments_e type = (swSketchSegments_e)skSeg.GetType();
                        switch (type)
                        {
                            case swSketchSegments_e.swSketchLINE:
                                LineVO line = getLine(swModel, skSeg);
                                lines.Add(line);
                                break;
                            case swSketchSegments_e.swSketchELLIPSE:
                                //Not Implemented
                                break;
                            case swSketchSegments_e.swSketchARC:
                                // SketchArc skArc = (SketchArc)skSeg;
                                ArcVO arc = getArc(swModel, skSeg);
                                arcs.Add(arc);
                                break;
                            case swSketchSegments_e.swSketchPARABOLA:
                                //Not Implemented
                                break;
                            case swSketchSegments_e.swSketchSPLINE:
                                //Not Implemented
                                break;

                        }
                    }
                }
                dynamic sketchPoints = swSketch.GetSketchPoints2();
                if (sketchPoints != null)
                {
                    foreach (dynamic sketchPoint in sketchPoints)
                    {
                        SketchPoint skPoint = (SketchPoint)sketchPoint;
                        PointVO point = getPoint(swModel, skPoint);
                        point.id = swModel.Extension.GetPersistReference3(skPoint);
                        points.Add(point);
                    }
                }
                skg.points = points;
                skg.lines = lines;
                skg.arcs = arcs;
            }

            
            return skg;
        }

        static ArcVO getArc(ModelDoc2 swModel, SketchSegment skSegArc)
        {
            ArcVO arc = new ArcVO();
           // arc.id = swModel.Extension.GetPersistReference3(skSegArc);
            SketchArc skArc = (SketchArc)skSegArc;
            //  line.id = swModel.Extension.GetPersistReference3(skLine);
            arc.centre = getPoint(swModel, skArc.GetCenterPoint2());
            arc.normalVector = skArc.GetNormalVector();
            arc.start = skArc.GetStartPoint2();
            arc.end = skArc.GetEndPoint2();
            arc.radius = skArc.GetRadius();
            return arc;
        }

        static LineVO getLine(ModelDoc2 swModel, SketchSegment skSegLine)
        {
            LineVO line = new LineVO();
        //    line.id = swModel.Extension.GetPersistReference3(skSegLine);
            SketchLine skLine = (SketchLine)skSegLine;
          //  line.id = swModel.Extension.GetPersistReference3(skLine);
            line.start = getPoint(swModel, skLine.GetStartPoint2());
            line.end = getPoint(swModel, skLine.GetEndPoint2());
            line.name = skSegLine.GetName();
            line.key = line.start.key+"##" + line.end.key+"##";
            return line;
        }

        static PointVO getPoint(ModelDoc2 swModel, SketchPoint skPoint)
        {
            PointVO pointVO = new PointVO();
            pointVO.x = skPoint.X;
            pointVO.y = skPoint.Y;
            pointVO.z = skPoint.Z;
            pointVO.key = "x:" + pointVO.x + "y:" + pointVO.y + "z:" + pointVO.z;
           // pointVO.id = swModel.Extension.GetPersistReference3(skPoint);
            return pointVO;
        }

        static PointVO getVertexPoint(ModelDoc2 swModel, double[] skPoint)
        {
            PointVO pointVO = new PointVO();
            pointVO.x = skPoint[0];
            pointVO.y = skPoint[1];
            pointVO.z = skPoint[2];
            pointVO.key = "x:" + pointVO.x + "y:" + pointVO.y + "z:" + pointVO.z;
            return pointVO;
        }

        /* private PointVO getPoint2(ModelDoc2 swModel, double[] skPoint)
         {
             PointVO pointVO = new PointVO();
             pointVO.x = skPoint[0];
             pointVO.y = skPoint[1];
             pointVO.z = skPoint[2];

             return pointVO;
         }*/

    }
}
