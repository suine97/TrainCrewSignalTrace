using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainCrew;

namespace TrainCrewSignalTrace
{
    internal class CalcATS
    {
        /// <summary> ATS誤出発防止起動時素 </summary>
        private const int FALSE_DEPARTURE_PROTECTION_TIME_DELAY = 60;
        /// <summary> ATS常用ブレーキ動作判定 </summary>
        private bool IsATSEmergencyBraking;
        /// <summary> ATS非常ブレーキ動作判定 </summary>
        private bool IsATSServiceBraking;
        /// <summary> ATS制限速度 </summary>
        private float ATSLimitSpeed;

        public CalcATS()
        {
            ResetATS();
            Task.Run(() => StartUpdateLoop());
        }

        /// <summary>
        /// 非同期更新ループ
        /// </summary>
        private async void StartUpdateLoop()
        {
            while (true)
            {
                var timer = Task.Delay(50);
                try
                {
                    Elapse();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                await timer;
            }
        }

        /// <summary>
        /// 非同期処理
        /// </summary>
        internal void Elapse()
        {
            SignalBeaconsInfo beaconsInfo = null;
            float speed = (TrainState.TrainSpeed != null) ? (float)TrainState.TrainSpeed : 0.0f;

            //地上子情報更新
            if ((TrainState.SignalBeaconsList != null) && (TrainState.SignalBeaconsList.Count > 0))
            {
                if (TrainState.CoupledSignalBeaconIndex >= 0)
                {
                    beaconsInfo = TrainState.SignalBeaconsList[TrainState.CoupledSignalBeaconIndex];
                }
                else
                {
                    beaconsInfo = TrainState.SignalBeaconsList[0];
                }
            }

            //ATSブレーキ出力処理
            EnableATSBrake(beaconsInfo, speed);

            TrainState.ATSLimitSpeed = ATSLimitSpeed;
            TrainState.ATSEmergencyBrake = IsATSEmergencyBraking;
            TrainState.ATSServiceBrake = IsATSServiceBraking;
        }

        /// <summary>
        /// ATS初期化
        /// </summary>
        public void ResetATS()
        {
            IsATSEmergencyBraking = false;
            IsATSServiceBraking = false;
            ATSLimitSpeed = 30.0f;
        }

        /// <summary>
        /// ATSブレーキ緩解処理
        /// </summary>
        public void ReleaseATSBrake()
        {
            IsATSEmergencyBraking = false;
            IsATSServiceBraking = false;
        }

        /// <summary>
        /// ATSブレーキ出力
        /// </summary>
        private void EnableATSBrake(SignalBeaconsInfo beaconsInfo, float NowSpeed)
        {
            try
            {
                var signalLimitSpeed = 30.0f;

                //地上子情報があれば処理
                if (beaconsInfo != null)
                {
                    //信号現示別制限速度
                    switch (beaconsInfo.SignalPhase)
                    {
                        case "R":
                            signalLimitSpeed = 0.0f;
                            break;
                        case "YY":
                            signalLimitSpeed = 30.0f;
                            break;
                        case "Y":
                            signalLimitSpeed = 60.0f;
                            break;
                        case "YG":
                            signalLimitSpeed = 85.0f;
                            break;
                        case "G":
                            signalLimitSpeed = 300.0f;
                            break;
                        default:
                            signalLimitSpeed = 0.0f;
                            break;
                    }

                    //地上子結合時処理
                    if (beaconsInfo.IsCoupling)
                    {
                        switch (beaconsInfo.BeaconType)
                        {
                            case "Signal":
                                {
                                    ATSLimitSpeed = signalLimitSpeed;
                                }
                                break;
                            case "SigIfStop":
                            case "PattrenSwitch":
                                //R現示なら地上子制限速度に更新
                                if (beaconsInfo.SignalPhase == "R")
                                {
                                    ATSLimitSpeed = beaconsInfo.BeaconLimitspeed;
                                }
                                //R現示以外なら現示アップ時のみ更新
                                else if (ATSLimitSpeed < signalLimitSpeed)
                                {
                                    ATSLimitSpeed = signalLimitSpeed;
                                }
                                break;
                            case "誤出発防止":
                                if ((beaconsInfo.SignalPhase == "R") && (beaconsInfo.ElapsedTime?.TotalSeconds > FALSE_DEPARTURE_PROTECTION_TIME_DELAY))
                                {
                                    ATSLimitSpeed = beaconsInfo.BeaconLimitspeed;
                                }
                                else if (ATSLimitSpeed < signalLimitSpeed)
                                {
                                    ATSLimitSpeed = signalLimitSpeed;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                //0信号受信後の現示アップ
                if (ATSLimitSpeed.IsZero() && signalLimitSpeed > 0.0f)
                {
                    ATSLimitSpeed = signalLimitSpeed;
                }

                //速度照査
                if (ATSLimitSpeed < NowSpeed)
                {
                    //非常ブレーキ判定
                    if (ATSLimitSpeed < 20.0f)
                    {
                        IsATSEmergencyBraking = true;
                    }
                    //常用ブレーキ判定
                    else
                    {
                        IsATSServiceBraking = true;
                    }
                }
                //常用ブレーキ緩解判定
                else if (IsATSServiceBraking && (NowSpeed <= (ATSLimitSpeed - 2.0f)))
                {
                    IsATSServiceBraking = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
