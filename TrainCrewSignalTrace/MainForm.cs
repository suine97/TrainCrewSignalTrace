using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using TrainCrew;

namespace TrainCrewSignalTrace
{
    public partial class MainForm : Form
    {
        private readonly Timer timer;
        private readonly Timer interval500Timer;
        private readonly StringBuilder sb = new StringBuilder();
        private readonly StringBuilder sb2 = new StringBuilder();
        private readonly CalcATS calcATS = new CalcATS();
        private string OldSignalName = "None";
        private bool IsInterval = false;
        private bool IsUIUpdate = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            FormClosing += MainForm_FormClosing;
            SignalComboBox.SelectedIndex = 0;

            TrainCrewInput.Init();

            //Timer設定
            timer = InitializeTimer(50, Timer_Tick);
            interval500Timer = InitializeTimer(500, Interval500Timer_Tick);
            this.TopMost = true;
        }

        /// <summary>
        /// Timer初期化メソッド
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="tickEvent"></param>
        /// <returns></returns>
        private Timer InitializeTimer(int interval, EventHandler tickEvent)
        {
            var timer = new Timer
            {
                Interval = interval
            };
            timer.Tick += tickEvent;
            timer.Start();
            return timer;
        }

        /// <summary>
        /// Timer_Tickイベント (Interval:50ms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            var state = TrainCrewInput.GetTrainState();
            TrainCrewInput.RequestStaData();
            TrainCrewInput.RequestData(DataRequest.Signal);
            if (state == null || state.CarStates.Count == 0 || state.stationList.Count == 0) { return; }
            try { var dataCheck = state.stationList[state.nowStaIndex].Name; }
            catch { return; }

            //運転画面遷移なら処理
            if (TrainCrewInput.gameState.gameScreen == GameScreen.MainGame
                || TrainCrewInput.gameState.gameScreen == GameScreen.MainGame_Pause
                || TrainCrewInput.gameState.gameScreen == GameScreen.MainGame_Loading)
            {
                SuspendLayout();

                //現在速度
                TrainState.TrainSpeed = state.Speed;
                //手動力行ノッチ
                TrainState.TrainPnotch = state.Pnotch;
                //手動制動ノッチ
                TrainState.TrainBnotch = state.Bnotch;

                //信号機情報取得
                var strSignal = TrainState.SignalBeaconsList;
                //次閉塞区間の信号機地上子情報を取得
                string signalPhase = SignalComboBox.SelectedItem.ToString();
                TrainState.SignalBeaconsList
                    = SignalBeacons.ConvertSignalBeacons(TrainCrewInput.signals, signalPhase, (float)TrainState.TrainSpeed);

                //地上子情報を取得
                if ((TrainState.SignalBeaconsList != null) && (TrainState.SignalBeaconsList.Count > 0))
                {
                    string strName = OldSignalName;
                    OldSignalName = TrainState.SignalBeaconsList[0].SignalName;

                    //閉塞区間が変わったら地上子Indexをリセット
                    if (strName != OldSignalName)
                    {
                        TrainState.CoupledSignalBeaconIndex = -1;
                    }
                    else
                    {
                        //地上子と結合したら情報を更新
                        var strIndex = SignalBeacons.GetCoupledSignalBeaconIndex(TrainState.SignalBeaconsList);
                        if (strIndex >= 0)
                        {
                            TrainState.CoupledSignalBeaconIndex = strIndex;
                        }
                    }
                }

                //ブレーキ出力
                switch (state.CarStates[0].CarModel)
                {
                    case "50000":
                    case "5320":
                    case "5300":
                    case "4321":
                    case "4300":
                    case "4000R":
                    case "4000":
                    case "3300V":
                    case "3300":
                        if (TrainState.ATSEmergencyBrake)
                        {
                            TrainCrewInput.SetATO_Notch(-8);
                        }
                        else if (TrainState.ATSServiceBrake)
                        {
                            TrainCrewInput.SetATO_Notch(-7);
                        }
                        else
                        {
                            TrainCrewInput.SetATO_Notch(0);
                        }
                        break;
                    case "3020":
                    case "3000":
                        if (TrainState.ATSEmergencyBrake)
                        {
                            TrainCrewInput.SetATO_Notch(-9);
                        }
                        else if (TrainState.ATSServiceBrake)
                        {
                            TrainCrewInput.SetATO_Notch(-8);
                        }
                        else
                        {
                            TrainCrewInput.SetATO_Notch(0);
                        }
                        break;
                    default:
                        break;
                }

                string atsClass = state.ATS_Class;
                string atsSpeed = (TrainState.ATSLimitSpeed < float.Parse(state.ATS_Speed)) ? TrainState.ATSLimitSpeed.ToString("F0") : state.ATS_Speed;
                string atsState = TrainState.ATSEmergencyBrake ? "EB" : TrainState.ATSServiceBrake ? "B動作" : state.ATS_State;

                sb.Clear();
                if (strSignal!= null && strSignal.Count > 0)
                {
                    for (int i = 0; i < strSignal.Count; i++)
                    {
                        sb.AppendLine($"SignalName       :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].SignalName : ""));
                        sb.AppendLine($"SignalPhase      :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].SignalPhase: ""));
                        sb.AppendLine($"SignalDistance   :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].SignalDistance.ToString("F2") : ""));
                        sb.AppendLine($"BeaconType       :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].BeaconType : ""));
                        sb.AppendLine($"BeaconSpeed      :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].BeaconLimitspeed.ToString("F2") : ""));
                        sb.AppendLine($"BeaconDistance   :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].BeaconDistance.ToString("F2") : ""));
                        sb.AppendLine($"BeaconCoupled    :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].IsCoupling : ""));
                        sb.AppendLine($"InitialTimestamp :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].InitialTimestamp.ToString("HH:mm:ss.FF") : ""));
                        sb.AppendLine($"ElapsedTime      :[{i}]:" + (strSignal.Count > 0 ? strSignal[i].ElapsedTime?.TotalSeconds.ToString("F2") : ""));
                        sb.AppendLine("");
                    }
                }
                else
                {
                    sb.AppendLine("SignalName       :[0]:");
                    sb.AppendLine("SignalPhase      :[0]:");
                    sb.AppendLine("SignalDistance   :[0]:");
                    sb.AppendLine("BeaconType       :[0]:");
                    sb.AppendLine("BeaconSpeed      :[0]:");
                    sb.AppendLine("BeaconDistance   :[0]:");
                    sb.AppendLine("BeaconCoupled    :[0]:");
                    sb.AppendLine("InitialTimestamp :[0]:");
                    sb.AppendLine("ElapsedTime      :[0]:");
                    sb.AppendLine("");
                }
                if (IsUIUpdate)
                {
                    textBox1.Text = sb.ToString();
                }
                
                sb2.Clear();
                sb2.AppendLine($"ATS_Class   :{atsClass}");
                sb2.AppendLine($"ATS_Speed   :{atsSpeed}");
                sb2.AppendLine($"ATS_State   :{atsState}");
                sb2.AppendLine($"OldTrackName:{OldSignalName}");
                sb2.AppendLine($"BeaconIndex :{TrainState.CoupledSignalBeaconIndex}");
                if (IsUIUpdate)
                {
                    label2.Text = sb2.ToString();
                }

                ResumeLayout();
            }
            else
            {
                calcATS.ResetATS();
                SignalBeacons.ResetSignalData();
            }
        }

        /// <summary>
        /// Interval500Timer_Tickイベント (Interval:500ms)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Interval500Timer_Tick(object sender, EventArgs e)
        {
            IsInterval = !IsInterval;
        }

        /// <summary>
        /// MainForm_KeyDownイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                        Application.Exit();
                    break;
            }
        }

        /// <summary>
        /// MainForm_FormClosingイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TrainCrewInput.Dispose();
        }

        /// <summary>
        /// CheckBox_UIUpdate_CheckedChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_UIUpdate_CheckedChanged(object sender, EventArgs e)
        {
            IsUIUpdate = CheckBox_UIUpdate.Checked;
        }

        /// <summary>
        /// ATSResetButton_Clickイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ATSResetButton_Click(object sender, EventArgs e)
        {
            //非常ブレーキ復帰
            if (TrainState.ATSEmergencyBrake
                && (TrainState.ATSLimitSpeed > 0.0)
                && (TrainState.TrainSpeed < 1.0)
                && (TrainState.TrainBnotch >= 8))
            {
                calcATS.ReleaseATSBrake();
            }
        }
    }
}
