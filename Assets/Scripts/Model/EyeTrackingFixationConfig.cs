using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class EyeTrackingFixationConfig : EyeTrackingConfig
    {
        public double timeFixation = 7.0;
        public double distance = 3;
    }
}
