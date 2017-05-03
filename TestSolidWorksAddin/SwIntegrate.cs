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
                return true;

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
                rk.SetValue("Title", "My SwAddin - AJ"); // Title
                rk.SetValue("Description", "All your pixels are belong to us"); // Description
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
