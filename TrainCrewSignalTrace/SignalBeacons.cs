using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TrainCrew;

namespace TrainCrewSignalTrace
{
    public static class SignalBeacons
    {
        /// <summary> 最小結合範囲 </summary>
        private const float MINIMUM_COUPLING_RANGE = 0.35f;
        /// <summary> 最大結合範囲 </summary>
        private const float MAXIMUM_COUPLING_RANGE = 5.00f;
        /// <summary> 直前の信号機名称と進入時刻を管理する辞書 </summary>
        private static Dictionary<string, DateTime> signalTimestamps = new Dictionary<string, DateTime>();
        /// <summary> 地上子結合範囲表示用 </summary>
        public static float couplingRange = 0.0f;

        /// <summary>
        /// 地上子結合中判定
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="beaconDistance"></param>
        /// <returns></returns>
        internal static bool IsCouplingBeacon(float speed, float beaconDistance)
        {
            try
            {
                //地上子結合範囲算出
                couplingRange = CustomMath.Lerp(0.0f, MINIMUM_COUPLING_RANGE, 120.0f, MAXIMUM_COUPLING_RANGE, Math.Abs(speed));
                //地上子結合判定
                if ((0.0f <= Math.Abs(beaconDistance)) && (Math.Abs(beaconDistance) <= couplingRange))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return false;
        }

        /// <summary>
        /// SignalInfoクラスをSignalBeaconsInfoクラスに変換
        /// </summary>
        /// <param name="signalInfoList"></param>
        /// <returns></returns>
        internal static List<SignalBeaconsInfo>? ConvertSignalBeacons(List<SignalInfo> signalInfoList, string signalPhase, float nowSpeed)
        {
            try
            {
                if (signalInfoList?.Count > 0)
                {
                    List<SignalBeaconsInfo> SignalBeaconsInfoList = new List<SignalBeaconsInfo>();

                    foreach (var signal in signalInfoList)
                    {
                        foreach (var beacon in signal.beacons)
                        {
                            SignalBeaconsInfo newBeacon = new SignalBeaconsInfo(
                                signal.name,
                                signalPhase,
                                signal.distance,
                                beacon.type,
                                beacon.speed,
                                beacon.distance,
                                IsCouplingBeacon((float)nowSpeed, beacon.distance)
                            );

                            //直前の信号機名称と比較して辞書を参照
                            if (signalTimestamps.TryGetValue(signal.name, out DateTime value))
                            {
                                //経過時間を計算
                                newBeacon.SetInitialTimestamp(value);
                                newBeacon.UpdateElapsedTime();
                            }
                            else if (signalTimestamps.Count == 0)
                            {
                                //最初の信号機名称の場合は呼び出し時刻を60秒前に更新
                                signalTimestamps[signal.name] = DateTime.Now.AddSeconds(-60);
                            }
                            else
                            {
                                //新しい信号機名称の場合は呼び出し時刻を更新
                                signalTimestamps[signal.name] = DateTime.Now;
                            }
                            SignalBeaconsInfoList.Add(newBeacon);
                        }
                    }

                    // BeaconDistanceを基準に昇順へ並べ替え
                    SignalBeaconsInfoList.Sort((s1, s2) => s1.BeaconDistance.CompareTo(s2.BeaconDistance));

                    return SignalBeaconsInfoList;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return null;
        }

        /// <summary>
        /// 結合した地上子情報のインデックスを取得
        /// </summary>
        internal static int GetCoupledSignalBeaconIndex(List<SignalBeaconsInfo> signals)
        {
            try
            {
                for (int i = 0; i < signals.Count; i++)
                {
                    if (signals[i].IsCoupling)
                    {
                        return i;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return -1;
        }

        /// <summary>
        /// 保持データ初期化
        /// </summary>
        internal static void ResetSignalData()
        {
            signalTimestamps.Clear();
        }
    }
}
