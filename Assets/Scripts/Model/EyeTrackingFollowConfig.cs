using System;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class EyeTrackingFollowConfig : EyeTrackingConfig
    {
        public int repetitions = 5;
        public double speed = 1.0;
    }
}
