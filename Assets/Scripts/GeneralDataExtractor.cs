using Assets.Scripts;
using Assets.Scripts.Model;
using System.Diagnostics;
using UnityEngine;
using ViveSR.anipal.Eye;
using UnityEngine.SceneManagement;

public class GeneralDataExtractor : MonoBehaviour
{
    private static GeneralDataExtractor _instance;

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

        sw = new Stopwatch();
        sw.Start();
    }

    private void Start()
    {
        bus = EventBus.Instance;

        // 12 HZ (83 ms) pour quantifier le tremblement
        InvokeRepeating("SaveOculusControllersData", 0.0f, 0.083f);

        // Capped to 120 HZ because of HTC VIVE Pro Eye refresh rate
        InvokeRepeating("SaveEyesData", 0.0f, 0.0083f);
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
                    Time = time,
                    Type = Assets.Scripts.Model.Types.EventType.LeftControllerPosition.ToString(),

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
                    Time = time,
                    Type = Assets.Scripts.Model.Types.EventType.RightControllerPosition.ToString(),

                    x = pos.x,
                    y = pos.y,
                    z = pos.z
                });
            }
        }
    }

    private void SaveEyesData()
    {
        var time = sw.ElapsedMilliseconds;

        VerboseData data;
        bool valid = SRanipal_Eye.GetVerboseData(out data);

        bus.Push(Assets.Scripts.Model.Types.EventType.EyeData, new
        {
            Time = time,
            Type = Assets.Scripts.Model.Types.EventType.EyeData.ToString(),
            
            LeftEyeOpenness = data.left.eye_openness,
            LeftEyePupilDiameter = data.left.pupil_diameter_mm,
            LeftEyePupilPositionInSensorArea = data.left.pupil_position_in_sensor_area,
            LeftEyeGazeDirectionNormalized = data.left.gaze_direction_normalized,
            LeftEyeGazeOrigin = data.left.gaze_origin_mm,

            RightEyeOpenness = data.right.eye_openness,
            RightEyePupilDiameter = data.right.pupil_diameter_mm,
            RightEyePupilPositionInSensorArea = data.right.pupil_position_in_sensor_area,
            RightEyeGazeDirectionNormalized = data.right.gaze_direction_normalized,
            RightEyeGazeOrigin = data.right.gaze_origin_mm,

            CombinedEyesOpenness = data.combined.eye_data.eye_openness,
            CombinedEyesPupilDiameter = data.combined.eye_data.pupil_diameter_mm,
            CombinedEyesPupilPositionInSensorArea = data.combined.eye_data.pupil_position_in_sensor_area,
            CombinedEyesGazeDirectionNormalized = data.combined.eye_data.gaze_direction_normalized,
            CombinedEyesGazeOrigin = data.combined.eye_data.gaze_origin_mm,

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
            Timestamp = time,
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
}
