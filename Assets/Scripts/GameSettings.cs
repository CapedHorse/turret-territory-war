using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public List<Team> Teams;
    public int defaultGridsCount = 100;
    [Range(1, 10)] public float turretRotateSpeed = 5;
    public float turretRotateDegree = 45;
    public float gameDurationSeconds = 540;
    public float playerEmptyDelayBeforeStart = 1.5f;
    public int countDownTime = 2;
    public float shootInterval = 0.05f;
    

    [System.Serializable]
    public class Team
    {
        public string TeamName;
        [Tooltip("Resolusi foto harus sama, misal 512x512")] public Texture TeamPhotoTexture;
        public Color TeamColor;
    }
}
