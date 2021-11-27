using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class ToolConfigs
    {
        public PegboardConfig PegboardConfig;

        public FingerFollowConfig FingerFollowConfig;

        public EverydayTaskConfig EverydayTaskConfig;

        public RhythmConfig RhythmConfig; 

        public FingerNoseConfig FingerNoseConfig;

        public EyeTrackingFollowConfig EyeTrackingFollowConfig;

        public EyeTrackingMultipleConfig EyeTrackingMultipleConfig;

        public EyeTrackingFixationConfig EyeTrackingFixationConfig;

        public SimpleToolConfig MenuConfig;

        public IEnumerable<IToolConfig> All()
        {
            return new List<IToolConfig> 
            {
                PegboardConfig,
                FingerFollowConfig,
                EverydayTaskConfig,
                RhythmConfig,
                FingerNoseConfig,
                EyeTrackingFollowConfig,
                EyeTrackingMultipleConfig,
                EyeTrackingFixationConfig,
                MenuConfig
            };
        }
    }
}