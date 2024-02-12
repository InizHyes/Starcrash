using UnityEngine;

[System.Serializable]
public class GridSection
{
    public GameObject[] prefabs; // Array of prefabs for this section
}

public class GridManager : MonoBehaviour
{
    public GridSection[] gridSections; // Array of sections for the grid
    public Transform[,] grid; // 2D array to hold references to instantiated prefabs
    public Vector2 gridSize = new Vector2(3, 3); // Size of the grid
    public Vector2 cellSize = new Vector2(1, 1); // Size of each grid cell

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Transform[(int)gridSize.x, (int)gridSize.y]; // Initialize the grid

        // Loop through each position in the grid
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // Select a prefab from the appropriate section options
                GameObject prefab = GetRandomPrefab(x, y);

                // Calculate position for current grid cell
                Vector2 position = new Vector2(x * cellSize.x, y * cellSize.y);

                // Instantiate the prefab at the current grid position
                GameObject newPrefab = Instantiate(prefab, position, Quaternion.identity);

                // Store a reference to the instantiated prefab in the grid array
                grid[x, y] = newPrefab.transform;
            }
        }
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
}
