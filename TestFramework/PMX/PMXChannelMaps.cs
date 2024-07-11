using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spea;
using Spea.TestFramework;
using Spea.TestEnvironment;

namespace Program
{
    public class PMXChannelMaps
    {
        public PMXChannelMaps() 
        {
            PmxChannel.DefaultInstrument = PmxInstrId.PMX1;
            PmxChannel.DefaultSection = PmxSection.HVREL;
        }        

            private PmxChannelMap<PmxPurpose> _pmxChannelMap = new PmxChannelMap<PmxPurpose>
            {
                //{ PmxPurpose.PurposeOfFirstChannel, 1 }, // Inplicit cast from integer to Pmx Channel, assuming PmxChannel.DefaultInstrument and PmxChannel.DefaultSection.
                //{ PmxPurpose.PurposeOfSecondChannel, new PmxChannel(2, PmxSection.HVREL, PmxInstrId.PMX1) },
            };


        public PmxChannelMap<PmxPurpose> getPmxChannelMap()
        {
            return _pmxChannelMap;
        }
    }   
}
