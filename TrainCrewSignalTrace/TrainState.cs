using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainCrewSignalTrace
{
    internal static class TrainState
    {
        /// <summary>
        /// 自車速度
        /// </summary>
        static public float? TrainSpeed;
        /// <summary>
        /// 自車手動P段
        /// </summary>
        static public int TrainPnotch;
        /// <summary>
        /// 自車手動B段
        /// </summary>
        static public int TrainBnotch;
        /// <summary>
        /// ATS非常ブレーキ
        /// </summary>
        static public bool ATSEmergencyBrake;
        /// <summary>
        /// ATS常用ブレーキ
        /// </summary>
        static public bool ATSServiceBrake;
        /// <summary>
        /// ATS制限速度
        /// </summary>
        static public float ATSLimitSpeed;
        /// <summary>
        /// 信号機地上子情報
        /// </summary>
        static public List<SignalBeaconsInfo>? SignalBeaconsList;
        /// <summary>
        /// 結合した信号機地上子Index
        /// </summary>
        static public int CoupledSignalBeaconIndex;

        /// <summary>
        /// 完全初期化
        /// </summary>
        static public void init()
        {
            TrainSpeed = 0f;
            ATSEmergencyBrake = false;
            ATSServiceBrake = false;
            ATSLimitSpeed = 0f;
            SignalBeaconsList = null;
            CoupledSignalBeaconIndex = -1;
        }
    }
}
