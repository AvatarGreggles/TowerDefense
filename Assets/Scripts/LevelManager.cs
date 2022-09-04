using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] tilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private Transform map;

    private Point mapSize;

    private Point blueSpawn;
    private Point redSpawn;

    public Point BlueSpawn
    {
        get
        {
            return blueSpawn;

        }
    }

    public Point RedSpawn
    {
        get
        {
            return redSpawn;

        }
    }

    [SerializeField]
    private GameObject bluePortalPrefab;
    [SerializeField]
    private GameObject redPortalPrefab;

    public Portal BluePortal { get; set; }
    public Portal RedPortal { get; set; }

    public Dictionary<Point, TileScript> Tiles { get; set; }

    private Stack<Node> path;

    public Stack<Node> Path
    {
        get
        {
            if (path == null)
            {
                GeneratePath();
            }

            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public float TileSize
    {
        get
        {
            return tilePrefabs[0].GetComponent<Renderer>().bounds.size.x;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;


        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for (int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);

    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');

    }

    private void SpawnPortals()
    {
        blueSpawn = new Point(11, 21);

        GameObject tmp = Instantiate(bluePortalPrefab, Tiles[blueSpawn].WorldPosition, Quaternion.identity);
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        redSpawn = new Point(9, 0);

        GameObject tmp2 = Instantiate(redPortalPrefab, Tiles[redSpawn].WorldPosition, Quaternion.identity);
        RedPortal = tmp2.GetComponent<Portal>();
        RedPortal.name = "RedPortal";

    }

    public bool InBounds(Point position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < mapSize.x && position.y < mapSize.y;
    }

    public void GeneratePath()
    {
        path = AStar.GetPath(redSpawn, blueSpawn);
    }

}
