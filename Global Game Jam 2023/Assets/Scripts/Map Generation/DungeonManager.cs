using Unity.VisualScripting;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    /// <summary>
    /// The current floor the player is.
    /// </summary>
    public static int CurrentFloor { get { return currentFloor; } }

    private static bool gameComplete = false;
    public static bool GameComplete { get { return gameComplete; } set { gameComplete = value; } }

    private LevelGenerator levelGenerator;
    private float floorTimer = 0;

    private static int currentFloor = 0;

    private int totalFloors;
    private bool floorGenerationEnded = false;
    [Tooltip("Minimum amount of floors the game will have.")]
    [SerializeField] private int minimumFloors = 12;
    [Tooltip("Maximum amount of floors the game will have.")]
    [SerializeField] private int maximumFloors = 20;

    private void Awake()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();
        GenerateFloors();
    }

    private void OnEnable() => LevelExit.LevelFinished += NextFloor;

    private void Start()
    {
        currentFloor = 0;
        NextFloor();
    }

    private void OnDisable() => LevelExit.LevelFinished -= NextFloor;

    private void Update()
    {
        if(floorGenerationEnded == false)
        {
            floorTimer += Time.deltaTime;

            UI_PlayerDungeon.Instance.ChangeLevelTime(floorTimer);
        }
        
    }

    /// <summary>
    /// Picks a random floor number between the minimum and maximum floors.
    /// </summary>
    private void GenerateFloors() => totalFloors = Random.Range(minimumFloors, maximumFloors + 1);

    /// <summary>
    /// Travels to the next floor, loading a new level.
    /// </summary>
    public void NextFloor()
    {
        currentFloor++;

        UI_PlayerDungeon.Instance.ChangeLevelText(CurrentFloor);

        if (currentFloor >= totalFloors)
        {
            // We are on the last floor so we need to create rules for this floor for being the last one (endgame)
            levelGenerator.CreateLastLevel();
            floorTimer = 0;
        }
        else if(currentFloor < totalFloors)
        {
            levelGenerator.CreateNewLevel();
            floorTimer = 0;
            
        }else
        {
            Debug.Log("Stopped Generating Levels");
        }
        
    }

    public bool FloorGenerationStatus()
    {
        return floorGenerationEnded;
    }
    
}
