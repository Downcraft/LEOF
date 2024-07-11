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
            (site, siteManager) => new Fct8311_SupplyVoltagesAndCurrents(site, siteManager), //funktioniert
            (site, siteManager) => new Fct8322_SystemBasisChip(site, siteManager), //funktioniert, aber instabil nSCCP3V3 ist manchmal 0V
            (site, siteManager) => new Fct8312_15VMonitoring(site, siteManager), //funktioniert
            (site, siteManager) => new Fct8313_Terminal30_Monitoring(site, siteManager), //Bridge Spannung fehlt (muss über Software getriggert werden?)
            (site, siteManager) => new Fct8314_HvFlybackConverter(site, siteManager), //funktioniert
            (site, siteManager) => new Fct8321_5VMonitoring(site, siteManager), //funktioniert
            (site, siteManager) => new Fct8331_SafeStateLogic(site, siteManager), //funktioniert
            (site, siteManager) => new Fct8341_AksSsoSwitching(site, siteManager),//XCP Variable IoEcu_rAsoAsc.AsoAscPinLvl bleibt auf 0 obwohl HW Wert sich ändert
            (site, siteManager) => new Fct8342_HvDcMeasurementAndOvDetection(site, siteManager),//funktioniert
            (site, siteManager) => new Fct8351_HvInterlock(site, siteManager), //Funktioniert HW Signal seitig HwTest_rHvIlkCurrAdcvolt und HwTest_rHvIlkVoltAdcvolt bleiben ca. auf 0
            (site, siteManager) => new Fct8361_CrashDetection(site, siteManager), //funktioniert aber ist nur ein Zustand als Messung genug?
            (site, siteManager) => new Fct8371_ActiveDischarge(site, siteManager), //Eine Spannung zu niedrig ist GND LV richtiger Bezugspunkt? ActvDchaCurrEst 0.3V statt 0.45V.
            (site, siteManager) => new Fct8381_CanTransceiver(site, siteManager), //WakeUpSbc_Dig Spannung etwas zu hoch 12.2V satt 12.0V
            (site, siteManager) => new Fct8391_Decoupling(site, siteManager), //Stromwert über XCP hat polariät geändert über XCP jetzt -0.7A statt +0.7A
            (site, siteManager) => new Fct83101_WheelSpeed(site, siteManager), //Spannungen alle etwas anders als bei C1: SPD_SIG = 13.8 statt 13.5V , HwTest_VADC_Result_G3_Ch5 = 1.9V statt 1.8V
            (site, siteManager) => new Fct83111_ResolverExcitationOutput(site, siteManager),//RMS Werte etwas höher als bei C1: 7.7V statt 7.4
            (site, siteManager) => new Fct83112_ResolverInterface(site, siteManager), //funkioniert
            (site, siteManager) => new Fct83121_AcCurrentMeasurement(site, siteManager), //funktioniert aber instabil 
            (site, siteManager) => new Fct83131_EmTemperatureMeasurements(site, siteManager),//funktioniert garnicht mehr. Die Range umschaltung hatte bei C1 mit HWTEST FW funktioniert.
            (site, siteManager) => new Fct83141_GateDriverMagneto(site, siteManager), //funktioniert
            (site, siteManager) => new Fct83142_InternalSuppliesHvSide(site, siteManager), //funktioniert nicht Baustein reagiert nicht auf Änderungen an LV Seite PWM Signal an MCU zu sehen aber nicht bei den Gates.
            //(site, siteManager) => new Fct83143_Desaturation(site, siteManager), // Zerstört Mosfets und Baugruppe.
            (site, siteManager) => new Fct83151_4_096V_Ref_Voltage(site, siteManager),//funktioniert
            (site, siteManager) => new Fct83161_InverterSyncOut(site, siteManager),//funktioniert
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
