using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCampaign", menuName = "Gamedev/Campaign Data")]
public class CampaignData : ScriptableObject
{
    public List<WaveData> Waves;
}