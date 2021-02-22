#region Headers

	using UnityEngine;
	using UnityEngine.UI;

#endregion


///<summary>
/// 
///		
///
///</summary>
[AddComponentMenu("Scripts/MindwaveUnity/Mindwave UI")]
public class MindwaveUI : MonoBehaviour
{

	#region Attributes

		// Constants & Statics

		private const int LABEL_WIDTH = 100;
		private const int VALUE_MIN_WIDTH = 70;

		// References

		[Header("References")]

		[SerializeField]
		private MindwaveController m_Controller = null;

		[SerializeField]
		private MindwaveCalibrator m_Calibrator = null;

		// Flow

		private MindwaveDataModel m_MindwaveData;
		private int m_EEGValue = 0;
		private int m_BlinkStrength = 0;

		float a,c;
		int b;
		int count;
		string raw1;
		string raw2;

	#endregion

	
	#region Engine Methods

		private void Awake()
		{
			if(m_Controller == null)
			{
				m_Controller = GetComponent<MindwaveController>();
			}

			if(m_Calibrator == null)
			{
				m_Calibrator = GetComponent<MindwaveCalibrator>();
			}

			BindMindwaveControllerEvents();
		}

		private void OnGUI()
		{
			GUILayout.BeginHorizontal();
			{
				DrawControllerGUI();
				DrawCalibratorGUI();
			}
			GUILayout.EndHorizontal();
		}

	#endregion

	
	#region Public Methods
		
		public void OnUpdateMindwaveData(MindwaveDataModel _Data)
		{
			m_MindwaveData = _Data;
		}

		public void OnUpdateRawEEG(int _EEGValue)
		{
			m_EEGValue = _EEGValue;
			if(a >= 5.0f && a <= 6.0f){
				raw1 += _EEGValue.ToString()+",";
					Debug.Log(raw1);
				}
		}

		public void OnUpdateBlink(int _BlinkStrength)
		{
			m_BlinkStrength = _BlinkStrength;
		}

	#endregion

	
	#region Private Methods

		private void BindMindwaveControllerEvents()
		{
			m_Controller.OnUpdateMindwaveData += OnUpdateMindwaveData;
			m_Controller.OnUpdateRawEEG += OnUpdateRawEEG;
			m_Controller.OnUpdateBlink += OnUpdateBlink;
		}

		private void DrawControllerGUI()
		{
			GUILayout.BeginVertical(GUI.skin.box);
			{
				GUILayout.Box("CONTROLLER");
				DrawSpace();

				if (m_Controller.IsConnecting)
				{
					GUILayout.BeginVertical(GUI.skin.box);
					{
						GUILayout.Label("Trying to connect to Mindwave...");
						GUILayout.Label("Timeouts in " + Mathf.CeilToInt(m_Controller.ConnectionTimeoutDelay - m_Controller.TimeoutTimer) + "s");
					}
					GUILayout.EndVertical();
				}

				else if (m_Controller.IsConnected)
				{
					DrawConnectedGUI();
				}

				else
				{
					DrawDisconnectedGUI();
				}
			}
			GUILayout.EndVertical();
		}

		private void DrawCalibratorGUI()
		{
			GUILayout.BeginVertical(GUI.skin.box);
			{
				GUILayout.Box("CALIBRATOR");
				DrawSpace();

				if (m_Controller != null && m_Controller.IsConnected)
				{
					GUILayout.Box("Data");
					DrawData("Nb. data collected", m_Calibrator.DataCount);
					DrawSpace();

					GUILayout.Box("Ratios");
					DrawData("Delta", m_Calibrator.EvaluateRatio(Brainwave.Delta, m_MindwaveData.eegPower.delta));
					DrawData("Theta", m_Calibrator.EvaluateRatio(Brainwave.Theta, m_MindwaveData.eegPower.theta));
					DrawData("Low Alpha", m_Calibrator.EvaluateRatio(Brainwave.LowAlpha, m_MindwaveData.eegPower.lowAlpha));
					DrawData("High Alpha", m_Calibrator.EvaluateRatio(Brainwave.HighAlpha, m_MindwaveData.eegPower.highAlpha));
					DrawData("Low Beta", m_Calibrator.EvaluateRatio(Brainwave.LowBeta, m_MindwaveData.eegPower.lowBeta));
					DrawData("High Beta", m_Calibrator.EvaluateRatio(Brainwave.HighBeta, m_MindwaveData.eegPower.highBeta));
					DrawData("Low Gamma", m_Calibrator.EvaluateRatio(Brainwave.LowGamma, m_MindwaveData.eegPower.lowGamma));
					DrawData("High Gamma", m_Calibrator.EvaluateRatio(Brainwave.HighGamma, m_MindwaveData.eegPower.highGamma));
				}

				else
				{
					GUILayout.Box("Not connected");
				}
			}
			GUILayout.EndVertical();
		}
		
		public void DrawConnectedGUI()
		{
			GUILayout.BeginVertical(GUI.skin.box);
			{
				/*GUILayout.Box("Signal");
				DrawData("Status", m_MindwaveData.status);
				DrawData("Poor signal level", m_MindwaveData.poorSignalLevel);
				DrawSpace();*/
			
				GUILayout.Box("Senses");
				DrawData("集中度", m_MindwaveData.eSense.attention);
				DrawData("リラックス度", m_MindwaveData.eSense.meditation);
				DrawSpace();

				GUILayout.Box("Brain Waves");
				DrawData("Delta", m_MindwaveData.eegPower.delta);
				DrawData("Theta", m_MindwaveData.eegPower.theta);
				DrawData("Low Alpha", m_MindwaveData.eegPower.lowAlpha);
				DrawData("High Alpha", m_MindwaveData.eegPower.highAlpha);
				DrawData("Low Beta", m_MindwaveData.eegPower.lowBeta);
				DrawData("High Beta", m_MindwaveData.eegPower.highBeta);
				DrawData("Low Gamma", m_MindwaveData.eegPower.lowGamma);
				DrawData("High Gamma", m_MindwaveData.eegPower.highGamma);
				DrawSpace();

				GUILayout.Box("Others");
				DrawData("Blink strength", m_BlinkStrength);
				DrawData("Raw EEG", m_EEGValue);
				
				
				//Debug.Log(a);
				//if(a <= 1.0f){
				//Debug.Log(m_EEGValue); // デバッグログは0.5秒が限界
				//raw1 += m_EEGValue.ToString()+",";
				
				//}
				/*count += 1;
				if (count <= 5){
					raw1 += m_EEGValue.ToString()+",";
				}
				if (count <= 5){
					raw2 += m_EEGValue.ToString()+",";
				}*/
			}
			GUILayout.EndVertical();

			if(GUILayout.Button("Disconnect"))
			{
				m_Controller.Disconnect();
			}
		}
		void FixedUpdate(){
			if(m_Controller.IsConnected){
				
			}
		}
		void Update(){
			if(m_Controller.IsConnected){
				a += Time.deltaTime;
				
			}
		}

		private void DrawData(string _Label, string _Value)
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label(_Label, GUILayout.Width(LABEL_WIDTH));
				GUILayout.Label(_Value, GUILayout.MinWidth(VALUE_MIN_WIDTH));
			}
			GUILayout.EndHorizontal();
		}

		private void DrawData(string _Label, int _Value)
		{
			DrawData(_Label, _Value.ToString());
		}

		private void DrawData(string _Label, float _Value)
		{
			DrawData(_Label, _Value.ToString());
		}

		private void DrawSpace()
		{
			GUILayout.Label("");
		}

		private void DrawDisconnectedGUI()
		{
			GUILayout.Box("Not connected");

			if(m_Controller != null)
			{
				if (GUILayout.Button("Connect"))
				{
					m_Controller.Connect();
				}
			}

			else
			{
				GUILayout.Box("No MindwaveController");
			}
		}

	#endregion

}