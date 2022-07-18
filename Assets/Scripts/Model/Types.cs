namespace Assets.Scripts.Model.Types
{
    public enum FingerFollowMode
    {
        Normal,
        IncrementalSpeed,
        Target
    }

    public enum RhythmMode
    {
        Normal,
        Clinical
    }

    public enum RhythmNote
    {
        hit,
        missed,
        spam
    }

    public enum EventType
    {
        All,
        ApplicationStart,
        ApplicationQuit,
        RightControllerPosition,
        LeftControllerPosition,
        RhythmData,
        RhythmConfig,
        EverydayTaskData,
        EverydayTaskConfig,
        FingerNoseData,
        FingerNoseConfig,
        FingerFollowData,
        FingerFollowConfig,
        PegboardData,
        PegboardConfig,
        EyeTrackingFixConfig,
        EyeTrackingFollowConfig,
        EyeTrackingMultipleConfig,
        EyeData,
        VibrationData,
        VibrationConfig,
        EyeContrastData,
        EyeContrastConfig,
        SpeechPerceptionData,
        SpeechPerceptionConfig,
        Other
    }
}
