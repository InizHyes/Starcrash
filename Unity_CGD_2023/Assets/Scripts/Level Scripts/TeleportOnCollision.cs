using UnityEngine;

public class TeleportOnCollision : MonoBehaviour
{
    // Define the destination positions for each teleporter
    public Vector3 Room1 = new Vector3(0f, 0f, 0f);
    public Vector3 Room2 = new Vector3(36f, 0f, 0f);
    public Vector3 Room3 = new Vector3(72f, 0f, 0f);
    public Vector3 Room4 = new Vector3(108f, 0f, 0f);
    public Vector3 Room5 = new Vector3(144f, 0f, 0f);

    public Vector3 Room6 = new Vector3(0f, 20f, 0f);
    public Vector3 Room7 = new Vector3(36f, 20f, 0f);
    public Vector3 Room8 = new Vector3(72f, 20f, 0f);
    public Vector3 Room9 = new Vector3(108f, 20f, 0f);
    public Vector3 Room10 = new Vector3(144f, 20f, 0f);

    public Vector3 Room11 = new Vector3(0f, 40f, 0f);
    public Vector3 Room12 = new Vector3(36f, 40f, 0f);
    public Vector3 Room13 = new Vector3(72f, 40f, 0f);
    public Vector3 Room14 = new Vector3(108f, 40f, 0f);
    public Vector3 Room15 = new Vector3(144f, 40f, 0f);

    public Vector3 Room16 = new Vector3(0f, 60f, 0f);
    public Vector3 Room17 = new Vector3(36f, 60f, 0f);
    public Vector3 Room18 = new Vector3(72f, 60f, 0f);
    public Vector3 Room19 = new Vector3(108f, 60f, 0f);
    public Vector3 Room20 = new Vector3(144f, 60f, 0f);

    public Vector3 Room21 = new Vector3(0f, 80f, 0f);
    public Vector3 Room22 = new Vector3(36f, 80f, 0f);
    public Vector3 Room23 = new Vector3(72f, 80f, 0f);
    public Vector3 Room24 = new Vector3(108f, 80f, 0f);
    public Vector3 Room25 = new Vector3(144f, 80f, 0f);

    // Called when the GameObject this script is attached to collides with another Collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 currentPlayerPosition = collision.gameObject.transform.position;
            int roomIndex = GetRoomIndexFromScriptName();
            Vector3 roomDestination = GetRoomPositionByIndex(roomIndex);

            Debug.Log("Script name: " + gameObject.name);
            Debug.Log("Extracted room index: " + roomIndex);

            collision.gameObject.transform.position = roomDestination;
            Debug.Log("Player Teleported from: " + currentPlayerPosition + " to: " + roomDestination);
        }
    }

    private int GetRoomIndexFromScriptName()
    {
        string scriptName = gameObject.name;
        int underscoreIndex = scriptName.LastIndexOf('_');

        if (underscoreIndex != -1 && underscoreIndex < scriptName.Length - 1)
        {
            string roomIndexString = scriptName.Substring(underscoreIndex + 1);

            // Remove the "Room" prefix
            if (roomIndexString.StartsWith("Room"))
            {
                roomIndexString = roomIndexString.Substring("Room".Length);
            }

            if (int.TryParse(roomIndexString, out int roomIndex))
            {
                return roomIndex;
            }
            else
            {
                Debug.LogError("Failed to parse room index from script name. RoomIndexString: " + roomIndexString);
            }
        }
        else
        {
            Debug.LogError("Invalid script name format. UnderscoreIndex: " + underscoreIndex + ", ScriptName: " + scriptName);
        }

        Debug.LogWarning("Script name does not follow the expected format. Defaulting to Room1.");
        return 13; // Default to Room1 if room index extraction fails
    }





    // Function to get room position based on the index
    private Vector3 GetRoomPositionByIndex(int index)
    {
        switch (index)
        {
            case 1:
                return Room1;
            case 2:
                return Room2;
            case 3:
                return Room3;
            case 4:
                return Room4;
            case 5:
                return Room5;
            case 6:
                return Room6;
            case 7:
                return Room7;
            case 8:
                return Room8;
            case 9:
                return Room9;
            case 10:
                return Room10;
            case 11:
                return Room11;
            case 12:
                return Room12;
            case 13:
                return Room13;
            case 14:
                return Room14;
            case 15:
                return Room15;
            case 16:
                return Room16;
            case 17:
                return Room17;
            case 18:
                return Room18;
            case 19:
                return Room19;
            case 20:
                return Room20;
            case 21:
                return Room21;
            case 22:
                return Room22;
            case 23:
                return Room23;
            case 24:
                return Room24;
            case 25:
                return Room25;
            default:
                return Room13;
        }
    }
}
