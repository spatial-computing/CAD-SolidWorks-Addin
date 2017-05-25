using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;
using System.Runtime.InteropServices;



namespace TestSolidWorksAddin
{
    public class SwIntegrate : ISwAddin
    {
        public SldWorks mSWApplication;
        private int mSWCookie;
        private TaskpaneView mTaskpaneView;
        private SWTaskpaneHost mTaskpaneHost;

        public bool ConnectToSW(object ThisSW, int Cookie)
        {
                mSWApplication = (SldWorks)ThisSW;
               
                mSWCookie = Cookie;
                // Set-up add-in call back info
                bool result = mSWApplication.SetAddinCallbackInfo(0, this, Cookie);
                this.UISetup();
            AttachEventHandlers();
            return true;

            }
        public bool AttachEventHandlers()
        {
            AttachSwEvents();
            
            return true;
        }
        private bool AttachSwEvents()
        {
            try
            {
       //         mSWApplication.ActiveDocChangeNotify += new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(ActiveDocChangeNotify);
                mSWApplication.DocumentLoadNotify2 += new DSldWorksEvents_DocumentLoadNotify2EventHandler(DocumentLoadNotify2);
       //         mSWApplication.FileNewNotify2 += new DSldWorksEvents_FileNewNotify2EventHandler(FileNewNotify2);
        //        mSWApplication.ActiveModelDocChangeNotify += new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(ActiveModelDocChangeNotify);
                mSWApplication.FileOpenPostNotify += new DSldWorksEvents_FileOpenPostNotifyEventHandler(FileOpenPostNotify);
         //       mSWApplication.FileOpenPreNotify += new DSldWorksEvents_FileOpenPreNotifyEventHandler(FileOpenPreNotify);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public int DocumentLoadNotify2(string docTitle, string docPath)
        {
            return 0;
        }

        int FileOpenPostNotify(string FileName)
        {
            
            return 0;
        }

        public bool DisconnectFromSW()     
            {
                this.UITeardown();
                return true;
            }

        private void UISetup()
        {
            mTaskpaneView = mSWApplication.CreateTaskpaneView2(string.Empty, "Woo! My first SwAddin");
            mTaskpaneHost = (SWTaskpaneHost)mTaskpaneView.AddControl(SWTaskpaneHost.SWTASKPANE_PROGID, "");
            mTaskpaneHost.mSWApplication = mSWApplication;
        }
        private void UITeardown()
        {
            mTaskpaneHost = null;
            mTaskpaneView.DeleteView();
            Marshal.ReleaseComObject(mTaskpaneView);
            mTaskpaneView = null;
        }



        [ComRegisterFunction()]
        private static void ComRegister(Type t)
        {
            string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
            using (Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyPath))
            {
                rk.SetValue(null, 1); // Load at startup
                rk.SetValue("Title", "Solidworks-Addin - Rel-II"); // Title
                rk.SetValue("Description", "Release - II"); // Description
            }
        }
        [ComUnregisterFunction()]
        private static void ComUnregister(Type t)
        {
            string keyPath = String.Format(@"SOFTWARE\SolidWorks\AddIns\{0:b}", t.GUID);
            Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(keyPath);
        }
    }
}
