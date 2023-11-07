using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Range(1, 10)] public float turretRotateSpeed = 5;
    public float turretRotateDegree = 45;
    public float gameDurationSeconds = 540;
    public float playerEmptyDelayBeforeStart = 1.5f;
    public int countDownTime = 2;
    
    public List<Projectile> projectilesPrefab;


    [System.Serializable]
    public class Team
    {
        
    }
}
