using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using System.Linq;

public class Map
{
    public List<int> grid;
    public int xSize;
    public int ySize;

    public Map(int x, int y)
    {
        xSize = x;
        ySize = y;
        grid = new List<int>(new int[x * y]);
        for (int i = 0; i < grid.Capacity; i++)
        {

            int xPos = i % xSize;
            int yPos = i / xSize;
            if (xPos == 0 || xPos == xSize - 1 || yPos == 0 || yPos == ySize - 1)
            {
                grid[i] = -1;
            }
            else
            {
                grid[i] = -2;
            }
        }
    }

    public int getValueAt(int x, int y)
    {
        return grid[y * ySize + x];
    }
    public int getValueAt(Vector2Int position)
    {
        return grid[position.y * ySize + position.x];
    }
    public void setCell(int x, int y, int val)
    {
        grid[y * ySize + x] = val;
    }
    public void setCell(Vector2Int position, int val)
    {
        grid[position.y * ySize + position.x] = val;
    }

}

public class DungeonGenerator : MonoBehaviour
{
    public Vector2 bottomLeft = new Vector2(0, 0);
    public Vector2 topRight = new Vector2(100, 100);
    public float minimumDistance = 1.0f;
    public int iterationPerPoint = 30;
    public GameObject pointPrefab;
    public GameObject pointPrefab2;
    public Map map;
    public List<RoomSeeds> rooms = new List<RoomSeeds>();

    private List<Vector2Int> samplePoints;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static List<float> GenerateBoundedNormal(float mean, float standardDeviation, float min, float max, int n)
    {
        List<float> values = new List<float>();

        while (values.Count < n)
        {
            float value = GenerateNormal(mean, standardDeviation);

            // Ensure the generated value is within the specified bounds
            if (value >= min && value <= max)
            {
                values.Add(value);
            }
        }

        return values;
    }

    private static float GenerateNormal(float mean = 0f, float standardDeviation = 1f)
    {
        float u1 = Random.Range(0f, 1f);
        float u2 = Random.Range(0f, 1f);

        float z0 = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return z0 * standardDeviation + mean;
    }

    public bool IsGraphFullyConnected(List<RoomSeeds> rooms)
    {
        // Step 1: Create a set of all rooms to track visited rooms
        var allRooms = new HashSet<RoomSeeds>(rooms);
        HashSet<RoomSeeds> visitedRooms = new HashSet<RoomSeeds>();
        Debug.Log(rooms.Count);

        // Step 2: Perform DFS starting from the first room (or any room)
        DFS(rooms[0], visitedRooms);

        // Step 3: Check if all rooms were visited
        Debug.Log(visitedRooms.Count);
        allRooms.ExceptWith(visitedRooms);
        foreach (var room in allRooms)
            Debug.Log(room.centre);
        return visitedRooms.Count == rooms.Count;
    }

    // Helper method to perform Depth-First Search (DFS)
    private void DFS(RoomSeeds currentRoom, HashSet<RoomSeeds> visitedRooms)
    {
        // If the room is already visited, return
        if (visitedRooms.Contains(currentRoom))
            return;

        // Mark the current room as visited
        visitedRooms.Add(currentRoom);

        // Step 4: Recursively visit all neighboring rooms
        foreach (var neighboringRoom in currentRoom.neighbouringRooms.Keys)
        {
            // Visit each neighboring room recursively
            DFS(neighboringRoom, visitedRooms);
        }
    }

    public static int GetDoorValue(int x)
    {
        for (int n = 6; n < 100; n += 3) 
        {
            if (x < n * n) 
            {
                return (n / 3) - 1; 
            }
        }
        return 100; 
    }

   
    void Start()
    {
        map = new Map((int)(topRight.x - bottomLeft.x), (int)(topRight.y - bottomLeft.y));

        samplePoints = FastPoissonDiskSampling.Sampling(new(bottomLeft.x + minimumDistance, bottomLeft.y + minimumDistance), new(topRight.x - minimumDistance, topRight.y - minimumDistance), minimumDistance, iterationPerPoint);
        List<float> guassain = GenerateBoundedNormal(15, 5, 3, 60, samplePoints.Count);
        int seedNo = 0;
        foreach (var point in samplePoints)
        {
            //int increaseFactor = 0;
            int x = Mathf.Max(5, (int)guassain[seedNo] + (int)Random.Range(-(int)guassain[seedNo] / 3, (int)guassain[seedNo] / 3));
            int y = Mathf.Max(5, (int)guassain[seedNo] + (int)Random.Range(-(int)guassain[seedNo] / 3, (int)guassain[seedNo] / 3));
            rooms.Add(new RoomSeeds(seedNo, point, new Vector2Int(x, y), (int)minimumDistance, map, rooms));
            seedNo += 1;
        }
        foreach (var room in rooms)
        {
            room.expandRoom();
        }
        for (int i = 0; i < map.grid.Count; i++)
        {
            Vector2Int current = new Vector2Int(i % map.xSize, i / map.xSize);
            if(map.getValueAt(current) == -1)

                foreach (Vector2Int direction in new Vector2Int[]
    {
        new Vector2Int(1, 0), // Right
        new Vector2Int(0, 1), // Up
    })
                {
                    Vector2Int adjacent = current + direction;
                    Vector2Int opposite = current - direction;

                    if (adjacent.x < 0 || adjacent.x >= map.xSize || adjacent.y < 0 || adjacent.y >= map.ySize )
                        continue;

                    if (opposite.x < 0 || opposite.x >= map.xSize || opposite.y < 0 || opposite.y >= map.ySize)
                        continue;

                    int oneSide = map.getValueAt(adjacent);
                    int twoSide = map.getValueAt(opposite);

                    if (oneSide != twoSide && oneSide != -1 && twoSide != -1 && oneSide != -2 && twoSide != -2)
                    {

                        if (!rooms[oneSide].neighbouringRooms.ContainsKey(rooms[twoSide]))
                        {
                            rooms[oneSide].neighbouringRooms[rooms[twoSide]] = new List<Vector2Int>();
                        }

                        rooms[oneSide].neighbouringRooms[rooms[twoSide]].Add(current);

                        if (!rooms[twoSide].neighbouringRooms.ContainsKey(rooms[oneSide]))
                        {
                            rooms[twoSide].neighbouringRooms[rooms[oneSide]] = new List<Vector2Int>();
                        }
                        rooms[twoSide].neighbouringRooms[rooms[oneSide]].Add(current);

                    }
                }
        }

        rooms = rooms.Where(room => room.neighbouringRooms.Any()).ToList();
       
        Debug.Log("The graph is connected:" + IsGraphFullyConnected(rooms));
        var sortedRooms = rooms.OrderBy(room => room.roomSize).ToList();

        HashSet<(int, int)> processedRoomPairs = new HashSet<(int, int)>();

        // Iterate through rooms and place doors between neighboring rooms
        foreach (var room in rooms)
        {
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> entry in room.neighbouringRooms)
            {
                RoomSeeds neighboringRoom = entry.Key;
                List<Vector2Int> doorPositions = entry.Value;

                int roomID1 = room.roomNumber;
                int roomID2 = neighboringRoom.roomNumber;

                int minRoomID = Mathf.Min(roomID1, roomID2);
                int maxRoomID = Mathf.Max(roomID1, roomID2);

                if (processedRoomPairs.Contains((minRoomID, maxRoomID)))
                {
                    continue;
                }

                processedRoomPairs.Add((minRoomID, maxRoomID));


                Vector2Int door = doorPositions[Random.Range(0, doorPositions.Count)];
                map.setCell(door, -3); 
            }
        }

        foreach (var room in rooms)
        {
            string setContents = "";
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> entry in room.neighbouringRooms)
            {
                RoomSeeds otherRoom = entry.Key;  // Access the RoomSeeds object (key)
                setContents += otherRoom.roomNumber + " ";  // Add the room number to the string
            }

            Debug.Log(room.centre + " " + setContents);
        }
        for (int i = 0; i < map.grid.Count; i++)
        {
            if (map.grid[i] == -1)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                Instantiate(pointPrefab, position, Quaternion.identity);
            }
            else if (map.grid[i] > 0)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(pointPrefab2, position, Quaternion.identity);
                // Randomly assign a color to the room
                int roomID = map.grid[i];
                Random.InitState(roomID); // Initialize the random generator with the roomID as the seed

                // Generate a random color based on the seed
                Color roomColor = Random.ColorHSV(); // This will generate a consistent color for this room

                // Apply the room color to the object's renderer
                Renderer renderer = roomObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = roomColor; // Apply the generated color
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(bottomLeft.x, bottomLeft.y, 0), new Vector3(topRight.x, bottomLeft.y, 0));
        Gizmos.DrawLine(new Vector3(bottomLeft.x, bottomLeft.y, 0), new Vector3(bottomLeft.x, topRight.y, 0));
        Gizmos.DrawLine(new Vector3(topRight.x, bottomLeft.y, 0), new Vector3(topRight.x, topRight.y, 0));
        Gizmos.DrawLine(new Vector3(bottomLeft.x, topRight.y, 0), new Vector3(topRight.x, topRight.y, 0));
    }
}
