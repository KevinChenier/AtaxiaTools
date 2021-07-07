using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class ToolConfigs
    {
        public PegboardConfig PegboardConfig;

        public FingerFollowConfig FingerFollowConfig;

        public EyeTrackingFollowConfig EyeTrackingFollowConfig;

        public EyeTrackingMultipleConfig EyeTrackingMultipleConfig;

        public EyeTrackingFixationConfig EyeTrackingFixationConfig;

        public SimpleToolConfig SampleConfig;

        public SimpleToolConfig MenuConfig;

        public IEnumerable<IToolConfig> All()
        {
            return new List<IToolConfig> 
            {
                PegboardConfig,
                FingerFollowConfig,
                EyeTrackingFollowConfig,
                EyeTrackingMultipleConfig,
                EyeTrackingFixationConfig,
                SampleConfig,
                MenuConfig
            };
        }
    }
}