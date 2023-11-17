using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Lean.Pool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager Instance;

    public Transform GridParent;
    public List<TeamMachine> BulletProducers;
    [FormerlySerializedAs("GridPrefab")] public ArenaGrid arenaGridPrefab;
    // public List<Turret> Turrets;
    public List<ArenaGrid> Grids;

    public Vector3 initPos;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async void Start()
    {
        await InitiateArena();
        
        GameManager.instance.StartGame();
    }

    //Initiating Area Task, after done, start the game
    async Task InitiateArena()
    {
        //Initiate Bullet Machine and Turrets
        for (int i = 0; i < GameManager.instance.gameSettings.Teams.Count; i++)
        {
            GameSettings.Team currentTeam = GameManager.instance.gameSettings.Teams[i];
            BulletProducers[i].InitiateTeam(i, currentTeam);
        }

        //Initiate Grids

        float gridScale = Mathf.Sqrt(GameManager.instance.gameSettings.defaultGridsCount) / GridParent.lossyScale.x;
        float textureTilingScale = gridScale/10f;
        Vector2 gridRowColumn = Vector2.one * Mathf.Floor(Mathf.Sqrt(GameManager.instance.gameSettings.defaultGridsCount));

        for (int i = 0; i < gridRowColumn.x; i++)
        {
            for (int j = 0; j < gridRowColumn.y; j++)
            {
                ArenaGrid newArenaGrid = LeanPool.Spawn(arenaGridPrefab, GridParent);
            
                //scale the new grid based on total count
                newArenaGrid.transform.localScale = new Vector3(gridScale, 1, gridScale);

                //Set the position to match the grid based on grid bound
                float xPos = (10 * (initPos.x * gridScale)) + ( j *  (gridScale / 2)) * 2;
                float zPos =  ( 10 * (initPos.z * gridScale)) + ( i * (gridScale / 2)) * 2;

                newArenaGrid.transform.localPosition = new Vector3(xPos, initPos.y, zPos);
            
                //set the grid photo texture tiling to counted one
                var material = newArenaGrid.GridPhotoPlane.material;
                material.mainTextureScale =  new Vector2(textureTilingScale, textureTilingScale);
            
                //Set the grid photo texture offset based on position
                float textureTilingOffsetX = textureTilingScale * j;
                float textureTilingOffsetY = textureTilingScale * i;
                material.mainTextureOffset =  new Vector2(textureTilingOffsetX, textureTilingOffsetY);
                
                newArenaGrid.GridPhotoPlane.material = material;

                newArenaGrid.Initiate();

                newArenaGrid.TestTexture();
                
                Grids.Add(newArenaGrid);
            }
        }
    }

    public void ProcessData(SimulationSettings.DummyPlayerInfo playerInfo)
    {
        BulletProducers[playerInfo.teamId-1].ProduceBullet(playerInfo.bullet);
    }

}
