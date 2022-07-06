using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class Config
    {
        public string Name;

        public ToolConfigs ToolConfigs;

        public ScenarioConfig? ScenarioConfig;

        public bool ScenarioActive;

        public bool ActivateTutorial;

        public bool AllowNavigation;

        public bool UseMongo;

        public string Locale;
    }
}
