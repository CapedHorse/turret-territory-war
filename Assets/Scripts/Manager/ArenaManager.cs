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
    public MeshRenderer GridArenaMesh;
    public List<TeamMachine> BulletProducers;
    [FormerlySerializedAs("GridPrefab")] public ArenaGrid arenaGridPrefab;
    // public List<Turret> Turrets;
    public List<ArenaGrid> Grids;

  
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
        
        float gridScale = GridParent.localScale.x / GameManager.instance.gameSettings.defaultGridsCount;
        float textureTilingScale = 1 / GameManager.instance.gameSettings.defaultGridsCount;
        Vector2 gridRowColumn = Vector2.one * Mathf.Sqrt(GameManager.instance.gameSettings.defaultGridsCount);

        var bounds = GridArenaMesh.bounds;
        float initXPos = bounds.min.x;
        float initZPos = bounds.min.z;

        for (int i = 0; i < gridRowColumn.x; i++)
        {
            for (int j = 0; j < gridRowColumn.y; j++)
            {
                ArenaGrid newArenaGrid = LeanPool.Spawn(arenaGridPrefab, GridParent);

                //Set the position to match the grid based on grid bound
                float xPos = initXPos + j + (gridScale / 2);
                float zPos = initZPos + i + (gridScale / 2);

                newArenaGrid.transform.localPosition = new Vector3(xPos, 0, zPos);
            
                //scale the new grid based on total count
                newArenaGrid.transform.localScale = new Vector3(gridScale, 0, gridScale);
            
                //set the grid photo texture tiling to counted one
                var material = newArenaGrid.GridPhotoPlane.material;
                material.mainTextureScale =  new Vector2(textureTilingScale, textureTilingScale);
            
                //Set the grid photo texture offset based on position
                material.mainTextureOffset =  new Vector2(textureTilingScale, textureTilingScale);
                
                newArenaGrid.GridPhotoPlane.material = material;

                newArenaGrid.Initiate();
                
                Grids.Add(newArenaGrid);
            }
        }
    }

    public void ProcessData(SimulationSettings.DummyPlayerInfo playerInfo)
    {
        
    }

}
