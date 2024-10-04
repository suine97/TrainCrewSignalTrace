using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainCrewSignalTrace
{
    /// <summary>
    /// 地上子情報リスト
    /// </summary>
    internal class SignalBeaconsInfo(string signalName, string signalPhase, float signalDistance, string beaconType, float beaconLimitspeed, float beaconDistance, bool isBeaconCoupling)
    {
        /// <summary> 信号機名称 </summary>
        public string SignalName { get; private set; } = signalName;
        /// <summary> 信号機現示 </summary>
        public string SignalPhase { get; private set; } = signalPhase;
        /// <summary>信号機までの距離 </summary>
        public float SignalDistance { get; private set; } = signalDistance;
        /// <summary> 地上子種類 </summary>
        public string BeaconType { get; private set; } = beaconType;
        /// <summary> 地上子制限速度 </summary>
        public float BeaconLimitspeed { get; private set; } = beaconLimitspeed;
        /// <summary> 地上子までの距離 </summary>
        public float BeaconDistance { get; private set; } = beaconDistance;
        /// <summary> 地上子結合判定 </summary>
        public bool IsCoupling { get; private set; } = isBeaconCoupling;
        /// <summary> 初回呼び出し時のタイムスタンプ </summary>
        public DateTime InitialTimestamp { get; private set; } = DateTime.Now;
        /// <summary> 経過時間 </summary>
        public TimeSpan? ElapsedTime { get; private set; } = null;

        /// <summary>
        /// 初回呼び出し時のタイムスタンプを設定
        /// </summary>
        public void SetInitialTimestamp(DateTime timestamp)
        {
            InitialTimestamp = timestamp;
        }

        /// <summary>
        /// 経過時間を更新
        /// </summary>
        public void UpdateElapsedTime()
        {
            ElapsedTime = DateTime.Now - InitialTimestamp;
        }
    }
}
