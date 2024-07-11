using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Program;
using System.Runtime.InteropServices;
using Spea;

namespace Program
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Extras.AddLeonardoToPath();
            }
            catch
            {
            }

            AtosF.modAtosF.LV_Begin();
            FixFPU();
            AtosF.modAtosF.LV_TestplanBegin();
            TestPlan.TestPlanStart();
            AtosF.modAtosF.LV_TestplanEnd();
            AtosF.modAtosF.LV_End();
            AtosF.modAtosF.LV_Exit();
        }

        #region FixFpu

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _controlfp(int IN_New, int IN_Mask); // this imports the call

        private const int _MCW_EM = 0x0008001f;
        private const int _EM_INVALID = 0x00000010;

        static public void FixFPU()
        {
            _controlfp(_MCW_EM, _EM_INVALID);  // this is the call
        }

        #endregion

    }
}
