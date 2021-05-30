using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tool : MonoBehaviour, IScore
{
    public abstract int score();
}
