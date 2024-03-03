using System; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Instructions:
// EndRoomSEquence should be triggered once "isRoomComplete" bool is true

// Info:
// The floor within the object has the "Child Collider" script attached to it,
// Which sends player/enemy collision messages to the public collision trigger functions inside this script.

// To do:
// Make enemy tiles do damage to player


public class ColourChange : MonoBehaviour
{
    Tilemap tilemap;
    Tile defaultTile;
    Tile playerTile;
    Tile enemyTile;

    Vector3Int tileLocation;
    public List<Vector2Int> playerTileList = new List<Vector2Int>();
    public List<Vector2Int> enemyTileList = new List<Vector2Int>();

    int halfMapWidth;
    int halfMapHeight;
    int tileCount;
    float playerTileDamage = 0.1f;
    float enemyTileDamage = 0.1f;
    public bool isRoomComplete = false;
    public bool toggleEnemyTileDmg = false;

    // Audio
    AudioSource audioSource;

    private void Start()
    {
        //Initialise variables
        //Loads ColourFloor tiles located in resources folder
        defaultTile = Resources.Load<Tile>("ColourFloor/Coloured Floors_7") as Tile;
        playerTile = Resources.Load<Tile>("ColourFloor/Coloured Floors_15") as Tile;
        enemyTile = Resources.Load<Tile>("ColourFloor/Coloured Floors_5") as Tile;

        tilemap = FindChildWithTag(this.transform, "Floor", "Floor"); 

        halfMapWidth = tilemap.size.x / 2;
        halfMapHeight = tilemap.size.y / 2;
        tileCount = tilemap.size.x * (tilemap.size.y - 2);

        audioSource = this.GetComponent<AudioSource>();
        audioSource.volume = 0.05f;
    }

    // Finds the child with a "floor" tag in order to get it's Tilemap component
    Tilemap FindChildWithTag(Transform parent, string tag, string childName)
    {
        Tilemap child = null;

        foreach (Transform transform in parent)
        {
            if (transform.CompareTag(tag) && transform.name == childName)
            {
           
                child = transform.GetComponent<Tilemap>(); 
                break;
            }
        }

        return child;
    }

    // Do something when all tiles are same colour
    public void AreAllTilesSameColour()
    {
        // Check if all the tilemaps tiles are the same colour
        if(playerTileList.Count >= tileCount)
        {
            // Can start the end room sequence!
            isRoomComplete = true;
        }
    }

    public void ReceiveOnTriggerStay(Collider2D collision)
    {
        if (isRoomComplete) return;

        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                //Convert player pos to world pos
                tileLocation = tilemap.WorldToCell(collision.gameObject.transform.position);
                Vector2Int pos = new Vector2Int(tileLocation.x, tileLocation.y);

                //Skip
                if (playerTileList.Contains(pos))
                    return;

                //Prevents painting tiles outside the room seems to work no matter where tilemap is positioned I.e it's relative
                if (pos.x < -halfMapWidth || pos.x > (halfMapWidth - 1) ||
                    pos.y < -(halfMapHeight - 1) || pos.y > (halfMapHeight - 2))
                    return;

                //Player stepped on enemy tile
                if (enemyTileList.Contains(pos))
                {
                    enemyTileList.Remove(pos);
                    //Damage player code here
                    if (toggleEnemyTileDmg)
                    {
                        var plrStats = collision.GetComponent<PlayerStats>();
                        plrStats.TakeDamage(enemyTileDamage);
                    }
                }

                //Set & track tile node
                tilemap.SetTile(tileLocation, playerTile);
                playerTileList.Add(pos);

                //Play ding sound 
                if (!audioSource.isPlaying)
                    audioSource.PlayOneShot(audioSource.clip);
            }

            else if (collision.gameObject.CompareTag("Enemy"))
            {
                //Convert enemy pos to world pos
                tileLocation = tilemap.WorldToCell(collision.gameObject.transform.position);
                Vector2Int pos = new Vector2Int(tileLocation.x, tileLocation.y);

                //Skip
                if (enemyTileList.Contains(pos))
                    return;

                //Prevents painting tiles outside the room
                if (pos.x < -halfMapWidth || pos.x > (halfMapWidth - 1) ||
                    pos.y < -(halfMapHeight - 1) || pos.y > (halfMapHeight - 2))
                    return;

                //Enemy stepped on player tile
                if (playerTileList.Contains(pos))
                {
                    playerTileList.Remove(pos);
                    //Damage enemy code here
                }

                //Set & track tile node
                tilemap.SetTile(tileLocation, enemyTile);
                enemyTileList.Add(pos);
            }
        }

        AreAllTilesSameColour();
    }

    public void ReceiveOnTriggerEnter(Collider2D collision)
    {
    }

    public void ReceiveOnTriggerExit(Collider2D collision)
    { 
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                ResetPuzzle();
            }
        }
    }



    //Resets the floor
    private void ResetPuzzle()
    {
        foreach (Vector2Int vec in playerTileList)
        {
            Vector3Int pos = new Vector3Int(vec.x, vec.y, 0);
            tilemap.SetTile(pos, defaultTile);
        }

        foreach (Vector2Int vec in enemyTileList)
        {
            Vector3Int pos = new Vector3Int(vec.x, vec.y, 0);
            tilemap.SetTile(pos, defaultTile);
        }

        playerTileList.Clear();
    }
}
