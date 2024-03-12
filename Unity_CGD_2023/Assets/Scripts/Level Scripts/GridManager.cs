using UnityEngine;

[System.Serializable]
public class GridSection
{
    public GameObject[] prefabs; // Array of prefabs for this section
}

public class GridManager : MonoBehaviour
{
    public GridSection[] gridSections; // Array of sections for the grid
    public Vector2 gridSize = new Vector2(3, 3); // Size of the grid
    public Vector2 cellSize = new Vector2(1, 1); // Size of each grid cell

    private int currentRoomX = 0; // Track the current room index (X-axis)
    private int currentRoomY = 0; // Track the current room index (Y-axis)
    private int roomEnterCounter = 1; // Counter for entering a room

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        // Loop through each position in the grid
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Select a prefab from the appropriate section options
                GameObject prefab = GetRandomPrefab(x, y);

                // Calculate position for the current grid cell
                Vector2 position = new Vector2(x * cellSize.x, y * cellSize.y);

                // Instantiate the prefab at the current grid position
                GameObject newPrefab = Instantiate(prefab, position, Quaternion.identity);

                // Get the size of the prefab
                Bounds bounds = GetPrefabBounds(newPrefab);

                // Add a trigger collider to the prefab based on its size
                BoxCollider2D collider = newPrefab.AddComponent<BoxCollider2D>();
                collider.size = bounds.size;
                collider.isTrigger = true;

                // Add a script to the prefab to handle entering the trigger zone
                GridPrefabScript gridPrefabScript = newPrefab.AddComponent<GridPrefabScript>();
                gridPrefabScript.SetGridManager(this);
                gridPrefabScript.SetRoomIndex(x, y);
                gridPrefabScript.SetDisableColliderOnEnter(y == 0 && x == 0); // Disable collider for the first room
            }
        }

        // Disable trigger colliders for other rooms initially
        DisableOtherRoomTriggers();
    }

    // Get a random prefab from the section options based on the grid position
    GameObject GetRandomPrefab(int x, int y)
    {
        // Determine the section index based on the grid position
        int sectionIndex = y * (int)gridSize.x + x;

        // Ensure the section index is within bounds of the grid sections array
        if (sectionIndex >= 0 && sectionIndex < gridSections.Length)
        {
            // Get the section for the current grid position
            GridSection section = gridSections[sectionIndex];

            // Select a random prefab from the section options
            return section.prefabs[Random.Range(0, section.prefabs.Length)];
        }
        else
        {
            Debug.LogError("Invalid section index!");
            return null;
        }
    }

    // Get the bounds of a prefab, considering all child objects
    Bounds GetPrefabBounds(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
        Bounds bounds = renderers[0].bounds;

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    public void PlayerEnteredNewPrefab(int roomX, int roomY)
    {
        if (roomX != currentRoomX || roomY != currentRoomY)
        {
            roomEnterCounter = 1;
            currentRoomX = roomX;
            currentRoomY = roomY;
            DisableOtherRoomTriggers();
        }
        else
        {
            roomEnterCounter = 0;
            EnableAllRoomTriggers();
        }

        Debug.Log("Player entered a new prefab in room (" + roomX + ", " + roomY + ") with counter " + roomEnterCounter);
    }

    private void DisableOtherRoomTriggers()
    {
        foreach (var script in FindObjectsOfType<GridPrefabScript>())
        {
            if (script.RoomX != currentRoomX || script.RoomY != currentRoomY)
            {
                script.SetTriggerEnabled(false);
            }
        }
    }

    private void EnableAllRoomTriggers()
    {
        foreach (var script in FindObjectsOfType<GridPrefabScript>())
        {
            script.SetTriggerEnabled(true);
        }
    }
}

public class GridPrefabScript : MonoBehaviour
{
    private GridManager gridManager;
    private int roomX;
    private int roomY;
    private BoxCollider2D collider;
    private bool disableColliderOnEnter;

    public int RoomX => roomX;
    public int RoomY => roomY;

    public void SetGridManager(GridManager manager)
    {
        gridManager = manager;
    }

    public void SetRoomIndex(int x, int y)
    {
        roomX = x;
        roomY = y;
        collider = GetComponent<BoxCollider2D>();
    }

    public void SetTriggerEnabled(bool enabled)
    {
        if (collider != null)
        {
            collider.enabled = enabled;
        }
    }

    public void SetDisableColliderOnEnter(bool disable)
    {
        disableColliderOnEnter = disable;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the GridManager that the player entered a new prefab
            gridManager.PlayerEnteredNewPrefab(roomX, roomY);

            // Disable the collider on enter if specified
            if (disableColliderOnEnter)
            {
                collider.enabled = false;
            }
        }
    }
}
