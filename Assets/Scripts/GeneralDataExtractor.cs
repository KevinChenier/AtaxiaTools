using Assets.Scripts;
using Assets.Scripts.Model;
using System.Diagnostics;
using UnityEngine;
using ViveSR.anipal.Eye;
using UnityEngine.SceneManagement;

public class GeneralDataExtractor : MonoBehaviour
{
    private static GeneralDataExtractor _instance;
    private string toolType;

    private EventBus bus;
    private Stopwatch sw;

    public static GeneralDataExtractor Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        bus = EventBus.Instance;

        OnApplicationStart();
    }

    public void StartSaveOculusControllersData(string toolType)
    {
        this.toolType = toolType;
        // 12 HZ (83 ms) pour quantifier le tremblement
        InvokeRepeating("SaveOculusControllersData", 0.0f, 0.083f);
        sw = new Stopwatch();
        sw.Start();
        
    }

    public void CancelSaveOculusControllersData()
    {
        CancelInvoke("SaveOculusControllersData");
        sw.Stop();
    }

    public void StartSaveEyesData(string toolType)
    {
        this.toolType = toolType;
        // Capped to 120 HZ because of HTC VIVE Pro Eye refresh rate
        InvokeRepeating("SaveEyesData", 0.0f, 0.0083f);
        sw = new Stopwatch();
        sw.Start();
    }

    public void CancelSaveEyesData()
    {
        CancelInvoke("SaveEyesData");
        sw.Stop();
    }

    private void SaveOculusControllersData()
    {
        var controllers = OVRInput.GetConnectedControllers().GetFlags();

        if (controllers is null || bus is null) return;

        var time = sw.ElapsedMilliseconds;

        foreach (var controller in controllers)
        {
            if (controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.LHand)
            {
                var pos = OVRInput.GetLocalControllerPosition(controller);

                bus.Push(Assets.Scripts.Model.Types.EventType.LeftControllerPosition, new
                {
                    Time = System.DateTime.Now.ToString(),
                    ElapsedTime = time,
                    Type = Assets.Scripts.Model.Types.EventType.LeftControllerPosition.ToString(),
                    ToolData = toolType,

                    x = pos.x,
                    y = pos.y,
                    z = pos.z
                });
            }
            else if (controller == OVRInput.Controller.RTouch || controller == OVRInput.Controller.RHand)
            {
                var pos = OVRInput.GetLocalControllerPosition(controller);

                bus.Push(Assets.Scripts.Model.Types.EventType.RightControllerPosition, new
                {
                    Time = System.DateTime.Now.ToString(),
                    ElapsedTime = time,
                    Type = Assets.Scripts.Model.Types.EventType.RightControllerPosition.ToString(),
                    ToolData = toolType,

                    x = pos.x,
                    y = pos.y,
                    z = pos.z
                });
            }
            UnityEngine.Debug.Log("Saved Oculus controller data");
        }
    }

    private void SaveEyesData()
    {
        var time = sw.ElapsedMilliseconds;

        VerboseData data;
        bool valid = SRanipal_Eye.GetVerboseData(out data);

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeData, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeData.ToString(),
            ToolData = toolType,

            LeftEyeOpenness = data.left.eye_openness,
            LeftEyePupilDiameter = data.left.pupil_diameter_mm,
            LeftEyePupilPositionInSensorArea_x = data.left.pupil_position_in_sensor_area.x,
            LeftEyePupilPositionInSensorArea_y = data.left.pupil_position_in_sensor_area.y,
            LeftEyeGazeDirectionNormalized_x = data.left.gaze_direction_normalized.x,
            LeftEyeGazeDirectionNormalized_y = data.left.gaze_direction_normalized.y,
            LeftEyeGazeDirectionNormalized_z = data.left.gaze_direction_normalized.z,
            LeftEyeGazeOrigin_x = data.left.gaze_origin_mm.x,
            LeftEyeGazeOrigin_y = data.left.gaze_origin_mm.y,
            LeftEyeGazeOrigin_z = data.left.gaze_origin_mm.z,

            RightEyeOpenness = data.right.eye_openness,
            RightEyePupilDiameter = data.right.pupil_diameter_mm,
            RightEyePupilPositionInSensorArea_x = data.right.pupil_position_in_sensor_area.x,
            RightEyePupilPositionInSensorArea_y = data.right.pupil_position_in_sensor_area.y,
            RightEyeGazeDirectionNormalized_x = data.right.gaze_direction_normalized.x,
            RightEyeGazeDirectionNormalized_y = data.right.gaze_direction_normalized.y,
            RightEyeGazeDirectionNormalized_z = data.right.gaze_direction_normalized.z,
            RightEyeGazeOrigin_x = data.right.gaze_origin_mm.x,
            RightEyeGazeOrigin_y = data.right.gaze_origin_mm.y,
            RightEyeGazeOrigin_z = data.right.gaze_origin_mm.z,

            CombinedEyesOpenness = data.combined.eye_data.eye_openness,
            CombinedEyesPupilDiameter = data.combined.eye_data.pupil_diameter_mm,
            CombinedEyesPupilPositionInSensorArea_x = data.combined.eye_data.pupil_position_in_sensor_area.x,
            CombinedEyesPupilPositionInSensorArea_y = data.combined.eye_data.pupil_position_in_sensor_area.y,
            CombinedEyesGazeDirectionNormalized_x = data.combined.eye_data.gaze_direction_normalized.x,
            CombinedEyesGazeDirectionNormalized_y = data.combined.eye_data.gaze_direction_normalized.y,
            CombinedEyesGazeDirectionNormalized_z = data.combined.eye_data.gaze_direction_normalized.z,
            CombinedEyesGazeOrigin_x = data.combined.eye_data.gaze_origin_mm.x,
            CombinedEyesGazeOrigin_y = data.combined.eye_data.gaze_origin_mm.y,
            CombinedEyesGazeOrigin_z = data.combined.eye_data.gaze_origin_mm.z,

            CombinedEyesConvergenceDistance = data.combined.convergence_distance_mm,
            CombinedEyesConvergenceDistanceValidity = data.combined.convergence_distance_validity
        });
    }

    private void SaveEyesDataForPyTrack()
    {
        var time = sw.ElapsedTicks;

        VerboseData data;
        bool valid = SRanipal_Eye.GetVerboseData(out data);

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeData, new
        {
            Time = System.DateTime.Now.ToString(),
            ElapsedTime = time,
            StimulusName = SceneManager.GetActiveScene().name,
            EventSource = "ET",
            GazeLeftx = data.left.gaze_direction_normalized.x + 1.0f,
            GazeRightx = data.right.gaze_direction_normalized.x + 1.0f,
            GazeLefty = data.left.gaze_direction_normalized.y + 1.0f,
            GazeRighty = data.right.gaze_direction_normalized.y + 1.0f,
            PupilLeft = data.left.pupil_diameter_mm,
            PupilRight = data.right.pupil_diameter_mm,
            FixationSeq = -1.0,
            SaccadeSeq = -1.0,
            Blink = -1.0,
            GazeAOI = -1.0
        });
    }

    private void OnApplicationStart()
    {
        bus.Push(Assets.Scripts.Model.Types.EventType.ApplicationStart, new
        {
            Time = System.DateTime.Now.ToString(),

            Type = Assets.Scripts.Model.Types.EventType.ApplicationStart.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,
            Scenario = ConfigManager.Instance.ScenarioManager.toolsOrder
        });
    }

    private void OnApplicationQuit()
    {
        bus.Push(Assets.Scripts.Model.Types.EventType.ApplicationQuit, new
        {
            Time = System.DateTime.Now.ToString(),

            Type = Assets.Scripts.Model.Types.EventType.ApplicationQuit.ToString(),
            PatientID = PatientData.PatientID,
            TrialID = PatientData.TrialID,
            Scenario = ConfigManager.Instance.ScenarioManager.toolsOrder
        });
    }

}
