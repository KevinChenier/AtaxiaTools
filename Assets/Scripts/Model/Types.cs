namespace Assets.Scripts.Model.Types
{
    public enum Mode
    {
        Normal,
        IncrementalSpeed,
        Target
    }

    public enum EventType
    {
        All,
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
        Other
    }
}
