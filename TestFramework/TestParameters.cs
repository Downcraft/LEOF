﻿namespace Program
{
    using Spea;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using PeakCanXcp;
    using Spea.TestFramework;
    using Newtonsoft.Json;
    using System.IO;
    using System.Diagnostics;
    using Spea.TestEnvironment;

    /// <summary>
    /// Use this class to pass custom parameters to your tests.
    /// Please ensure creante only on instance of this class.
    /// </summary>
    public class TestParameters
    {
        public static bool Debug = true;
        public static bool DontBreakOnFail = true;

        public TestParameters()
        {


            A2l = new A2lParser(IniFile.GetValue("SOFTWARE_CHECK", $"A2L"));
            Xcp = new Xcp(
                Xcp.NominalBitrate._500k,
                Xcp.DataBitrate._2M,
                A2l.CanParameters.CanIdMaster,
                A2l.CanParameters.CanIdSlave,
                A2l.CanParameters.CanIdBroadcast,
                1);
            Xcp.Uninitialize();

            XcpRestBus = new Xcp(
               Xcp.NominalBitrate._500k,
               Xcp.DataBitrate._2M,
               A2l.CanParameters.CanIdMaster,
               A2l.CanParameters.CanIdSlave,
               A2l.CanParameters.CanIdBroadcast,
               2);
            XcpRestBus.Uninitialize();
        }

        public static A2lParser A2l { get; set; }
        public Xcp Xcp { get; set; }
        public static Xcp XcpRestBus { get; set; }

       

    }
}