using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class ToolConfigs
    {
        public PegboardConfig PegboardConfig;

        public FingerFollowConfig FingerFollowConfig;

        public EverydayTaskConfig EverydayTaskConfig;

        public FingerNoseConfig FingerNoseConfig;

        public SimpleToolConfig SampleConfig;

        public SimpleToolConfig MenuConfig;

        public IEnumerable<IToolConfig> All()
        {
            return new List<IToolConfig> 
            {
                PegboardConfig,
                FingerFollowConfig,
                EverydayTaskConfig,
                FingerNoseConfig,
                SampleConfig,
                MenuConfig
            };
        }
    }
}