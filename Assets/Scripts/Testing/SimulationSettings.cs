using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DummyPlayerDatabase", menuName = "Scriptable Objects/DummyPlayerDatabase")]
public class SimulationSettings : ScriptableObject
{
    
    public int randomizerSeedRange = 100;
    public bool useFixed = true;
    public int simulatedDataReceivengOnStart = 10;
    public List<DummyPlayerInfo> dummyPlayerDatabase;

    public DummyPlayerInfo randomDummyPlayerPlaceHolder;
    
    [System.Serializable]
    public class DummyPlayerInfo
    {
        public string nickname = "player Name";
        public int bullet = 1;
        public string url;
        public int teamId = 1;

        public DummyPlayerInfo()
        {
            
        }
        public DummyPlayerInfo(DummyPlayerInfo dummyPlayerInfo)
        {
            nickname = dummyPlayerInfo.nickname;
            bullet = dummyPlayerInfo.bullet;
            teamId = dummyPlayerInfo.teamId;
            url = dummyPlayerInfo.url;
        }
    }
}
