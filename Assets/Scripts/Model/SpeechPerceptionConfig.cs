using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model
{
    [System.Serializable]
    public class SpeechPerceptionConfig : IToolConfig
    {
        public int repetitionsPerVolume;
        public int nbVolumeIncreases;
    }
}
