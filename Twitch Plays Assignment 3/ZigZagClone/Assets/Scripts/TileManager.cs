using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour 
{

    /// <summary>
    /// A list of the tiles, that we can spawn
    /// </summary>
    public GameObject[] tilePrefabs;

    /// <summary>
    /// The last tile that we spawned
    /// This is used as a reference for the next tile, that we are spawning
    /// </summary>
    public GameObject currentTile;

    /// <summary>
    /// A singleton instance for this Manager
    /// </summary>
    private static TileManager instance;

    /// <summary>
    /// A stack, that contains all the left tiles, this is used for recycling
    /// </summary>
    private Stack<GameObject> leftTiles = new Stack<GameObject>();

    /// <summary>
    /// A stack, that contains all the top tiles, this is used for recycling
    /// </summary>
    private Stack<GameObject> topTiles = new Stack<GameObject>();

    /// <summary>
    /// Property for accessing the left tiles
    /// </summary>
    public Stack<GameObject> LeftTiles
    {
        get { return leftTiles; }
        set { leftTiles = value; }
    }

    /// <summary>
    /// Property for accessing the top tiles
    /// </summary>
    public Stack<GameObject> TopTiles
    {
        get { return topTiles; }
        set { topTiles = value; }
    }

    /// <summary>
    /// A property for accessing the singleton instance 
    /// </summary>
    public static TileManager Instance
    {
        get 
        {
            if (instance == null) //Finds the instance if it doesn't exist
            {
                instance = GameObject.FindObjectOfType<TileManager>();
            }

            return instance; 
        
        }

    }

    

	// Use this for initialization
	void Start () 
    {
        //Creates 100 tiles
        CreateTiles(1);

        //Spawns 50 tiles when the game starts
        for (int i = 0; i < 10; i++)
        {
            SpawnTile();
        }
        
	}
	
    /// <summary>
    /// Creates an amount of tiles to be used in the game
    /// </summary>
    /// <param name="amount">amount of tiles</param>
    public void CreateTiles(int amount)
    {   

        for (int i = 0; i < amount; i++)
        {   
            //Creates a tile and adds it to the left stack
            leftTiles.Push(Instantiate(tilePrefabs[0]));

            //Creates a tile and adds it to the top stack
            topTiles.Push(Instantiate(tilePrefabs[1]));

            //Sets the name of the visibility of the tiles
            topTiles.Peek().name = "TopTile";
            topTiles.Peek().SetActive(false);
            leftTiles.Peek().name = "LeftTile";
            leftTiles.Peek().SetActive(false);
            
        }

    }

    /// <summary>
    /// Spawns a tile in the gameworld
    /// </summary>
    public void SpawnTile()
    {
        //If we are out of tiles, then we need to create more
        if (leftTiles.Count == 0 || topTiles.Count == 0)
        {
            CreateTiles(10);
        }

        //Generating a random number between 0 and 1
        int randomIndex = Random.Range(0, 2);


        if (randomIndex == 0) //If the random number is one then spawn a left tile
        {
            GameObject tmp = leftTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }
        else if(randomIndex == 1) //If the random number is one then spawn a top tile
        {
            GameObject tmp = topTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }

        int spawnPickup = Random.Range(0, 10); //rolls between  0 and 9

        if (spawnPickup == 0) //if we roll 0 then show the powerup
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Resets the game
    /// </summary>
    public void ResetGame()
    {   
        //Reloads the level
        Application.LoadLevel(Application.loadedLevel);
    }
}
