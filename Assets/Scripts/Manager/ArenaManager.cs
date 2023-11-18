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
    public BoxCollider GridParentBoxCol;
    public List<TeamMachine> BulletProducers;
    public Dictionary<int, TeamMachine> BulletProducerMaps;
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
        BulletProducerMaps = new Dictionary<int, TeamMachine>();
        await InitiateArena();
        
        GameManager.instance.StartGame();
    }

    //Initiating Area Task, after done, start the game
    async Task InitiateArena()
    {
        //Initiate Bullet Machine and Turrets
        for (int i = 0; i < GameManager.instance.gameSettings.Teams.Count; i++)
        {
            GameSettings.Team currentTeam = GameManager.instance.gameSettings.GetTeam(i+1);
            BulletProducers[i].InitiateTeam(i+1, currentTeam);
            BulletProducerMaps.Add(i+1, BulletProducers[i]);
        }

        //Initiate Grids

        float gridScale = GridParent.lossyScale.x / Mathf.Sqrt(GameManager.instance.gameSettings.defaultGridsCount) / GridParent.lossyScale.x;
        float textureTilingScale = gridScale;
        // float maxTextureOffsetValue = 0.9f;
        Vector2 textureInitOffset = new Vector2();
        initPos = new Vector3(GridParentBoxCol.bounds.min.x, 1, GridParentBoxCol.bounds.min.z);
        Vector2 gridRowColumn = Vector2.one * Mathf.Floor(Mathf.Sqrt(GameManager.instance.gameSettings.defaultGridsCount));

        for (int i = 0; i < gridRowColumn.x; i++)
        {
            for (int j = 0; j < gridRowColumn.y; j++)
            {
                ArenaGrid newArenaGrid = LeanPool.Spawn(arenaGridPrefab, GridParent);
            
                //scale the new grid based on total count
                newArenaGrid.transform.localScale = new Vector3(gridScale, 1, gridScale);

                //Set the position to match the grid based on grid bound
                float xPos = initPos.x / 10 + newArenaGrid.transform.localScale.x/2 + gridScale  * j;
                float zPos = initPos.z / 10 + newArenaGrid.transform.localScale.z/2 + gridScale * i;

                newArenaGrid.transform.localPosition = new Vector3(xPos, initPos.y, zPos);
            
                //set the grid photo texture tiling to counted one
                var material = newArenaGrid.GridPhotoPlane.material;
                material.mainTextureScale =  new Vector2(textureTilingScale, textureTilingScale);
            
                //Set the grid photo texture offset based on position
                float textureTilingOffsetX = textureTilingScale * j;// (maxTextureOffsetValue - gridRowColumn.y * j )/ 100; 
                float textureTilingOffsetY = textureTilingScale * i;// (maxTextureOffsetValue - gridRowColumn.x * i )/ 100; 
                material.mainTextureOffset =  new Vector2(textureTilingOffsetX, textureTilingOffsetY);
                
                newArenaGrid.GridPhotoPlane.material = material;

                newArenaGrid.Initiate();

                Grids.Add(newArenaGrid);
            }
        }

        foreach (var producer in BulletProducers)
        {
            producer.SetCanShoot(true);
        }
    }

    public void ProcessData(SimulationSettings.DummyPlayerInfo playerInfo)
    {
        BulletProducerMaps[playerInfo.teamId].ProduceBullet(playerInfo.bullet);
    }

}
