using System; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Instructions:
//Place the script on the floor which is under level 3 and assign the correct tiles.

public class ColourChange : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile defaultTile;
    public Tile playerTile;
    public Tile enemyTile;
  
    private Vector3Int tileLocation;
    public List<Vector2Int> playerTileList = new List<Vector2Int>();
    public List<Vector2Int> enemyTileList = new List<Vector2Int>();

    bool startPuzzle = false;

    //Audio
    public AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = 0.05f;
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (!startPuzzle)
            return;

        if (collision.GetType() == typeof(CircleCollider2D)) 
        {
            if (collision.gameObject.tag == "Player")
            {
                //Convert player pos to world pos
                tileLocation = tilemap.WorldToCell(collision.gameObject.transform.position);
                Vector2Int pos = new Vector2Int(tileLocation.x, tileLocation.y);

                //Skip
                if (playerTileList.Contains(pos)) 
                    return;

                //Prevents painting tiles outside the room
                if (pos.x < -11)
                    return;

                //Player stepped on enemy tile
                if (enemyTileList.Contains(pos))
                {
                    enemyTileList.Remove(pos);
                    //Damage player code here
                }

                //Set & track tile node
                tilemap.SetTile(tileLocation, playerTile);
                playerTileList.Add(pos);

                //Play ding sound 
                if (!audioSource.isPlaying)
                audioSource.PlayOneShot(audioSource.clip);
            }
           
            else if (collision.gameObject.tag == "Enemy")
            {
                //Convert enemy pos to world pos
                tileLocation = tilemap.WorldToCell(collision.gameObject.transform.position);
                Vector2Int pos = new Vector2Int(tileLocation.x, tileLocation.y);

                //Skip
                if (enemyTileList.Contains(pos))
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                StartPuzzle();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CircleCollider2D))
        {
            if (collision.gameObject.tag == "Player")
            {
                ResetPuzzle();
            }
        }
    }

    private void StartPuzzle()
    {
        if (!startPuzzle)
            startPuzzle = true;
        Debug.Log("start puzzle " + startPuzzle);
    }

    //Resets the floor
    private void ResetPuzzle()
    {
        foreach(Vector2Int vec in playerTileList)
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
        startPuzzle = false;
        Debug.Log("reset puzzle " + startPuzzle);
    }
}
