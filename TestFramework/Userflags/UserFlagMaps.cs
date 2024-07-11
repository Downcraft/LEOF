using Spea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spea.TestFramework;
using Spea.TestEnvironment;


namespace Program
{
    public class UserFlagMaps
    {
        public UserFlagMaps()
        {
            UserFlag.DefaultGroup = OutputUserFlags.RLYNOCPU;
        }


        private UserFlagMap<UserFlagPurpose> _userFlagMap = new UserFlagMap<UserFlagPurpose>
        {
            { UserFlagPurpose.CAN, 1 },
            { UserFlagPurpose.Power_Wheel_Speed, 2 },
            { UserFlagPurpose.Wheel_Speed_Signal, 3 },
            { UserFlagPurpose.Phase_W_Bottom_ISO, 4 },
            { UserFlagPurpose.Phase_W_Top_ISO, 5 },
            { UserFlagPurpose.Phase_V_Bottom_ISO, 6 },
            { UserFlagPurpose.Phase_V_Top_ISO, 7 },
            { UserFlagPurpose.Phase_U_Bottom_ISO, 8 },
            { UserFlagPurpose.Phase_U_Top_ISO, 9 },
            { UserFlagPurpose.HV, 10 },
            { UserFlagPurpose.CAP_Cb, 11 },
            { UserFlagPurpose.HV_GND_GND, 12 },
            { UserFlagPurpose.Reslv, 13 },
            { UserFlagPurpose.EM_Temp_Prim, 14 },
            { UserFlagPurpose.EM_Temp_Sec, 15 },
            { UserFlagPurpose.Power_Mod_U, 16 },
            { UserFlagPurpose.Power_Mod_V, 17 },
            { UserFlagPurpose.Power_Mod_W, 18 },
            { UserFlagPurpose.Solenoid, 19 },
            { UserFlagPurpose.CAN_Termination, 20 },
            { UserFlagPurpose.KL30C_KL30T, 32 },
            { UserFlagPurpose.OpenDrain, 31 },
        };


        public UserFlagMap<UserFlagPurpose> getUserFlagMap()
        {
            return _userFlagMap;
        }


    }
}
