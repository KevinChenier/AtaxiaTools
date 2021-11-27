using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class EyeTrackingMultipleConfig : EyeTrackingConfig
    {
        public double timer;
        public int fieldOfView = 30;
    }
}
