using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState : MonoBehaviour
{
    #region [Variables]
    //Variables for floor painting
    public bool floorPaintingLevel = false;
    [Range (0.0f, 1.0f)]
    [Tooltip ("Levels have more tiles under walls so dont put above 0.8")]
    public float requiredPercentage;

    //Doors
    
    public GameObject topDoor;
    public Sprite topDoorSprite;
    public GameObject bottomDoor;
    public Sprite bottomDoorSprite;

    //Bools for level states
    public bool levelCompleted = false;
    private bool levelStarted = false;
    private bool enemiesDefeated = false;
    private bool tubesDestroyed = false;
    private float timer;

    //miscellaneous to  floor painiting
    private Transform colorChangingFloor;
    private Collider2D tilemapCollider;
    private float totalTilesCount;
    private float playerTilesCount;
    private float enemyTilesCount;
    #endregion


    void Start()
    {
        #region [Floor Painting Data Init]
        //Gets all information about painted floor
        if (floorPaintingLevel)
        {
            int childCount = transform.parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (transform.parent.GetChild(i).GetComponent<ColourChange>() != null)
                {
                    colorChangingFloor = transform.parent.GetChild(i);
                    tilemapCollider = transform.parent.GetChild(i).GetComponent<Collider2D>();
                    totalTilesCount = tilemapCollider.shapeCount;
                }
            }
        }
        #endregion
    }


    void Update()
    {
        #region [Floor Painting Mechanics]
        //Checks how many tiles have been pained by player and Enemy
        if (floorPaintingLevel)
        {
            playerTilesCount = colorChangingFloor.GetComponent<ColourChange>().playerTileList.Count;
            enemyTilesCount = colorChangingFloor.GetComponent<ColourChange>().enemyTileList.Count;

            //Opens door if certain % of room was painted
            if (playerTilesCount / totalTilesCount >= requiredPercentage)
            {
                levelCompleted = true;
            }
        }
        #endregion

        #region [Enemy and Tube Mechanics]        
        //If all tubes and enemies are destroyed complete level
        if (levelStarted & enemiesDefeated & tubesDestroyed & timer >= 1)
        {
            levelCompleted = true;
        }

        //Safety timer to prevent bugs. If removed destroying last tube when enemies dont exist will cause level completed to be true
        if (enemiesDefeated & tubesDestroyed)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
        #endregion

        #region [Opens doors]
        //Opens the door
        if (levelCompleted)
        {
            topDoor.GetComponent<SpriteRenderer>().sprite = topDoorSprite;
            topDoor.GetComponent<BoxCollider2D>().enabled = false;
            bottomDoor.GetComponent<SpriteRenderer>().sprite = bottomDoorSprite;
            bottomDoor.GetComponent<BoxCollider2D>().enabled = false;
        }
        #endregion

    }

    #region [Collisions]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When player enters level level begins
        if (collision.tag == "Player")
        {
            levelStarted = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Checks if there are any unbroken tubes in level
        if (collision.tag == "Tube")
        {
            tubesDestroyed = false;
        }

        //Checks if there are any living enemies
        if (collision.tag == "Enemy")
        {
            enemiesDefeated = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Checks when tubes are destroyed
        if (collision.tag == "Tube")
        {
            tubesDestroyed = true;
        }

        //Checks when enemies are dead
        if (collision.tag == "Enemy")
        {
            enemiesDefeated = true;
        }
    }
    #endregion

}
