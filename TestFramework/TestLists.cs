using Spea.TestEnvironment;
using Spea.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class TestLists
    {
        /// <summary>
        /// The list of multi site tests that will be executed for this TPGM.
        /// </summary>
        private MultiSiteTestList<TestParameters, UserFlagPurpose, PmxPurpose> _multiSiteTestList = new MultiSiteTestList<TestParameters, UserFlagPurpose, PmxPurpose>
        {
        };

        /// <summary>
        /// The list of tests that will run for this TPGM. <br/>
        /// The tests will run in the order they are inserted in this list. <br/>
        /// The example test can be find in ./Tests. Further tests can be added to the same folder.
        /// </summary>
        private TestList<TestParameters, UserFlagPurpose, PmxPurpose> _testList = new TestList<TestParameters, UserFlagPurpose, PmxPurpose>
        {
            (site, siteManager, variant) => new Fct8311_SupplyVoltagesAndCurrents(site, siteManager, variant), 
            (site, siteManager, variant) => new Fct8312_15VMonitoring(site, siteManager, variant), 
            (site, siteManager, variant) => new Fct8313_Terminal30_Monitoring(site, siteManager, variant), 
            (site, siteManager, variant) => new Fct8314_HvFlybackConverter(site, siteManager, variant), 
            (site, siteManager, variant) => new Fct8321_5VMonitoring(site, siteManager, variant), 
            (site, siteManager, variant) => new Fct8322_SystemBasisChip(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8331_SafeStateLogic(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8341_AksSsoSwitching(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8342_HvDcMeasurementAndOvDetection(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8351_HvInterlock(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8361_CrashDetection(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8371_ActiveDischarge(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8381_CanTransceiver(site, siteManager, variant),
            (site, siteManager, variant) => new Fct8391_Decoupling(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83101_WheelSpeed(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83111_ResolverExcitationOutput(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83112_ResolverInterface(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83121_AcCurrentMeasurement(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83131_EmTemperatureMeasurements(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83141_GateDriverMagneto(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83142_InternalSuppliesHvSide(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83151_4_096V_Ref_Voltage(site, siteManager, variant),
            (site, siteManager, variant) => new Fct83161_InverterSyncOut(site, siteManager, variant),
        };


        public MultiSiteTestList<TestParameters, UserFlagPurpose, PmxPurpose> getMultiSiteTestList()
        {
            
            return _multiSiteTestList;
        }

        public TestList<TestParameters, UserFlagPurpose, PmxPurpose> getSingleSiteTestList()
        {
            return _testList;
        }

    }
}
