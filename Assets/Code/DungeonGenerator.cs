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
    public GameObject wall;
    public GameObject floor;
    public GameObject redWall;
    public GameObject blueWall;
    public GameObject greenWall;
    public GameObject redShrine;
    public GameObject blueShrine;
    public GameObject greenShrine;
    public GameObject player;
    public GameObject lockedDoor;
    public Map map;
    [Range(0.0f, 0.3f)]
    public float colourDoorChance;
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

    public bool IsGraphFullyConnectedColours(List<RoomSeeds> rooms)
    {
        // Step 1: Create a dictionary to track visited rooms and the colors used to visit them
        Dictionary<RoomSeeds, HashSet<PlayerColour>> visitedRooms = new Dictionary<RoomSeeds, HashSet<PlayerColour>>();

        // Step 2: Perform DFS starting from the first room with each initial color
        DFSColour(rooms[0], visitedRooms, PlayerColour.White);

        // Step 3: Check if every room has been visited with at least one color
        return visitedRooms.Keys.Count == rooms.Count;
    }

    // Helper method to perform Depth-First Search (DFS)
    private void DFSColour(RoomSeeds currentRoom, Dictionary<RoomSeeds, HashSet<PlayerColour>> visitedRooms, PlayerColour currentColour)
    {
        // If the room has already been visited with the current color, return
        if (visitedRooms.ContainsKey(currentRoom) && visitedRooms[currentRoom].Contains(currentColour))
            return;

        // Mark the room as visited with the current color
        if (!visitedRooms.ContainsKey(currentRoom))
            visitedRooms[currentRoom] = new HashSet<PlayerColour>();

        visitedRooms[currentRoom].Add(currentColour);

        // Get the connections based on the current color
        HashSet<RoomSeeds> connections = new HashSet<RoomSeeds>();
        switch (currentColour)
        {
            case PlayerColour.Red:
                connections = currentRoom.redConnections;
                break;
            case PlayerColour.Blue:
                connections = currentRoom.blueConnections;
                break;
            case PlayerColour.Green:
                connections = currentRoom.greenConnections;
                break;
        }

        foreach (var neighboringRoom in connections)
        {
            DFSColour(neighboringRoom, visitedRooms, currentColour);
        }

        // Handle shrines that allow color changes and continue traversal
        if (currentRoom.blueShrinePresent)
        {
            foreach (var neighboringRoom in currentRoom.blueConnections)
            {
                DFSColour(neighboringRoom, visitedRooms, PlayerColour.Blue);
            }
        }
        if (currentRoom.redShrinePresent)
        {
            foreach (var neighboringRoom in currentRoom.redConnections)
            {
                DFSColour(neighboringRoom, visitedRooms, PlayerColour.Red);
            }
        }
        if (currentRoom.greenShrinePresent)
        {
            foreach (var neighboringRoom in currentRoom.greenConnections)
            {
                DFSColour(neighboringRoom, visitedRooms, PlayerColour.Green);
            }
        }
    }



    public static int GetDoorValue(int x)
    {
        // If x is less than 36, return 1 door
        if (x < 36)
        {
            return 1;
        }

        // For values >= 36, calculate how many additional doors are needed
        // Start at 36 and every 10 units above it will add 1 door.
        return 1 + (x - 36) / 40;
    }

    public static List<List<KeyValuePair<RoomSeeds, List<Vector2Int>>>> GetCombinationsFromNToMax(
    Dictionary<RoomSeeds, List<Vector2Int>> dict,
    Dictionary<RoomSeeds, List<Vector2Int>> secondDict,
    int n)
    {
        List<KeyValuePair<RoomSeeds, List<Vector2Int>>> dictList = new List<KeyValuePair<RoomSeeds, List<Vector2Int>>>(dict);

        List<List<KeyValuePair<RoomSeeds, List<Vector2Int>>>> combinations = new List<List<KeyValuePair<RoomSeeds, List<Vector2Int>>>>();

        int count = (int)Mathf.Pow(2, dict.Count);

        for (int i = 0; i < count; i++)
        {
            string binaryString = System.Convert.ToString(i, 2).PadLeft(dict.Count, '0');

            // Create a list to store the current combination
            List<KeyValuePair<RoomSeeds, List<Vector2Int>>> currentCombination = new List<KeyValuePair<RoomSeeds, List<Vector2Int>>>();

            // Add elements from dictList to the current combination based on the binary string
            for (int j = 0; j < binaryString.Length; j++)
            {
                if (binaryString[j] == '1')
                {
                    currentCombination.Add(dictList[j]);
                }
            }

            // Check if all values from secondDict are included in the current combination
            bool isValidCombination = true;
            if (secondDict.Count >= 1)
                foreach (var kvp in secondDict)
                {
                    bool found = false;
                    foreach (var combinationKvp in currentCombination)
                    {
                        if (combinationKvp.Key.Equals(kvp.Key) && combinationKvp.Value.SequenceEqual(kvp.Value))
                        {
                            found = true;
                            break;
                        }
                    }

                    // If any value from secondDict is missing, the combination is invalid
                    if (!found)
                    {
                        isValidCombination = false;
                        break;
                    }
            }

            // If the combination is valid, add it to the list of combinations
            if (isValidCombination && currentCombination.Count >= n)
            {
                combinations.Add(currentCombination);
            }
        }

        return combinations;
    }



    bool reduceRoomConnections(List<RoomSeeds> rooms, int index = 0)
    {
        if (index >= rooms.Count)
        {
            return true; // All rooms have been processed, successfully reduced connections
        }

        RoomSeeds currentRoom = rooms[index];
        int minDoors = Mathf.Min(currentRoom.neighbouringRooms.Count, GetDoorValue(currentRoom.roomSize) - currentRoom.chosenDoors.Count);
        Debug.Log("Min doors is" + minDoors);

        // Backup the current neighboring rooms state before modifying anything
        Dictionary<RoomSeeds, List<Vector2Int>> originalNeighbours = new Dictionary<RoomSeeds, List<Vector2Int>>(currentRoom.neighbouringRooms);
        Dictionary<RoomSeeds, List<Vector2Int>> originalChosenDoors = new Dictionary<RoomSeeds, List<Vector2Int>>(currentRoom.chosenDoors);

        var combinations = GetCombinationsFromNToMax(originalNeighbours, currentRoom.chosenDoors, minDoors);
        print("no of combinations is" + combinations.Count);
        foreach (var combination in combinations)
        {
            foreach(var room in combination)
            {
                if (room.Key.chosenDoors.Count >= GetDoorValue(room.Key.roomSize) && !room.Key.chosenDoors.ContainsKey(currentRoom))
                    continue;
            }
            Debug.Log(index);
            // Apply the new combination to the current room
            currentRoom.neighbouringRooms = new Dictionary<RoomSeeds, List<Vector2Int>>(combination);
            currentRoom.chosenDoors = new Dictionary<RoomSeeds, List<Vector2Int>>(combination);


            // Update the chosenDoors in both directions
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> room in combination)
            {
                if (!room.Key.chosenDoors.ContainsKey(currentRoom))
                {
                    room.Key.chosenDoors.Add(currentRoom, room.Value);
                }
            }

            // Ensure we only update removed neighboring rooms from originalNeighbours
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> room in originalNeighbours)
            {
                if (!currentRoom.neighbouringRooms.ContainsKey(room.Key))
                {
                    room.Key.neighbouringRooms.Remove(currentRoom);
                }
            }

            // Check if the graph is still fully connected
            if (IsGraphFullyConnected(rooms))
            {
                // Recursively attempt to reduce connections in the next room
                if (reduceRoomConnections(rooms, index + 1))
                {
                    return true; // If it works, we are done
                }
            }

            // If not successful, revert to the original state
            currentRoom.neighbouringRooms = new Dictionary<RoomSeeds, List<Vector2Int>>(originalNeighbours);
            currentRoom.chosenDoors = new Dictionary<RoomSeeds, List<Vector2Int>>(originalChosenDoors);

            // Remove the connections from the combination, restore previous state
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> room in combination)
            {
                if(!currentRoom.chosenDoors.ContainsKey(room.Key))
                    room.Key.chosenDoors.Remove(currentRoom);
            }

            // Restore original neighboring rooms
            foreach (KeyValuePair<RoomSeeds, List<Vector2Int>> room in originalNeighbours)
            {
                if (!room.Key.neighbouringRooms.ContainsKey(currentRoom))
                {
                    room.Key.neighbouringRooms.Add(currentRoom, originalNeighbours[room.Key]);
                }
            }
        }

        return false; // If no successful reduction was found
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
            if (map.getValueAt(current) == -1)

                foreach (Vector2Int direction in new Vector2Int[]
    {
        new Vector2Int(1, 0), // Right
        new Vector2Int(0, 1), // Up
    })
                {
                    Vector2Int adjacent = current + direction;
                    Vector2Int opposite = current - direction;

                    if (adjacent.x < 0 || adjacent.x >= map.xSize || adjacent.y < 0 || adjacent.y >= map.ySize)
                        continue;

                    if (opposite.x < 0 || opposite.x >= map.xSize || opposite.y < 0 || opposite.y >= map.ySize)
                        continue;

                    int oneSide = map.getValueAt(adjacent);
                    int twoSide = map.getValueAt(opposite);

                    if (oneSide != twoSide && oneSide >= 0 && twoSide >= 0)
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
            if(map.getValueAt(current) == -2)
            {
                map.setCell(current, -1);
            }
        }

        rooms = rooms.Where(room => room.neighbouringRooms.Any()).ToList();

        Debug.Log("The graph is connected:" + IsGraphFullyConnected(rooms));
        var sortedRooms = rooms.OrderBy(room => room.roomSize).ToList();
        reduceRoomConnections(sortedRooms, 0);

        HashSet<(int, int)> processedRoomPairs = new HashSet<(int, int)>();

        // Iterate through rooms and place doors between neighboring rooms
        foreach (var room in sortedRooms)
        {
            int colour = -3;
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
                if (room.neighbouringRooms.Count == 1)
                {
                    float randomNumber = Random.Range(0f, 1f);
                    //red
                    if (randomNumber < colourDoorChance/2)
                    {
                        colour = -4;
                        room.redConnections.Add(entry.Key);
                        entry.Key.redConnections.Add(room);
                    }
                    //blue
                    else if (randomNumber < (colourDoorChance * 2)/2)
                    {
                        colour = -5;
                        room.blueConnections.Add(entry.Key);
                        entry.Key.blueConnections.Add(room);
                    }
                    //green
                    else if (randomNumber < (colourDoorChance * 3)/2)
                    {
                        colour = -6;
                        room.greenConnections.Add(entry.Key);
                        entry.Key.greenConnections.Add(room);
                    } else
                    {
                        colour = -7;
                        entry.Key.greenConnections.Add(room);
                        entry.Key.blueConnections.Add(room);
                        entry.Key.redConnections.Add(room);
                        room.redConnections.Add(entry.Key);
                        room.blueConnections.Add(entry.Key);
                        room.greenConnections.Add(entry.Key);
                    }
                }
            
                else
                {

                    
                    float randomNumber = Random.Range(0f, 1f);
                    //red
                    if (randomNumber < colourDoorChance)
                    {
                        colour = -4;
                        room.redConnections.Add(entry.Key);
                        entry.Key.redConnections.Add(room);
                    }
                    //blue
                    else if (randomNumber < colourDoorChance * 2)
                    {
                        colour = -5;
                        room.blueConnections.Add(entry.Key);
                        entry.Key.blueConnections.Add(room);
                    }
                    //green
                    else if (randomNumber < colourDoorChance * 3)
                    {
                        colour = -6;
                        room.greenConnections.Add(entry.Key);
                        entry.Key.greenConnections.Add(room);
                    } else
                    {
                        entry.Key.greenConnections.Add(room);
                        entry.Key.blueConnections.Add(room);
                        entry.Key.redConnections.Add(room);
                        room.redConnections.Add(entry.Key);
                        room.blueConnections.Add(entry.Key);
                        room.greenConnections.Add(entry.Key);
                    }
                }

                Vector2Int door = doorPositions[Random.Range(0, doorPositions.Count)];
                
                map.setCell(door, colour);
            }
        }
        Debug.Log(IsGraphFullyConnectedColours(rooms));
        while (!IsGraphFullyConnectedColours(rooms))
        {
            int shrineNumber = 0;
            RoomSeeds randomRoom = rooms[Random.Range(0, rooms.Count)];
            int choice = Random.Range(0, 3);
            
            if (choice == 0)
                randomRoom.blueShrinePresent = true;
            if (choice == 1)
                randomRoom.redShrinePresent = true;
            if (choice == 2)
                randomRoom.greenShrinePresent = true;
            }

        for (int i = 0; i < map.grid.Count; i++)
        {
            if (map.grid[i] == -1)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                Instantiate(wall, position, Quaternion.identity);
            }
            else if (map.grid[i] >= 0)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(floor, position, Quaternion.identity);
                // Randomly assign a color to the room
                int roomID = map.grid[i];
                Random.InitState(roomID); // Initialize the random generator with the roomID as the seed

                // Generate a random color based on the seed
                Color roomColor = Random.ColorHSV(); 

                // Apply the room color to the object's renderer
                Renderer renderer = roomObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    //renderer.material.color = roomColor; // Apply the generated color
                }
            } else if (map.grid[i] == -3)
            {
                Vector3 position = new(i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(floor, position, Quaternion.identity);
            } else if(map.grid[i] == -4)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(redWall, position, Quaternion.identity);
                Instantiate(floor, position, Quaternion.identity);
            }
            else if (map.grid[i] == -5)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(blueWall, position, Quaternion.identity);
                Instantiate(floor, position, Quaternion.identity);
            }
            else if (map.grid[i] == -6)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                GameObject roomObj = Instantiate(greenWall, position, Quaternion.identity);
                Instantiate(floor, position, Quaternion.identity);
            }
            else if (map.grid[i] == -7)
            {
                Vector3 position = new Vector3((int)i % map.xSize, (int)i / map.xSize, 0);
                Instantiate(floor, position, Quaternion.identity);
            }
        }
        foreach (var room in rooms)
        {
            List<Vector2Int> possibleTiles = new List<Vector2Int>(room.roomTiles);
            if (room.blueShrinePresent)
            {
                var possibleTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
                Vector3 position = new Vector3(possibleTile.x, possibleTile.y, 0);
                GameObject roomObj = Instantiate(blueShrine, position, Quaternion.identity);
            }
            if (room.redShrinePresent)
            {
                var possibleTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
                Vector3 position = new Vector3(possibleTile.x, possibleTile.y, 0);
                GameObject roomObj = Instantiate(redShrine, position, Quaternion.identity);
            }
            if (room.greenShrinePresent)
            {
                var possibleTile = possibleTiles[Random.Range(0, possibleTiles.Count)];
                Vector3 position = new Vector3(possibleTile.x, possibleTile.y, 0);
                GameObject roomObj = Instantiate(greenShrine, position, Quaternion.identity);
            }
        }
        Vector3 newPosition = new Vector3(rooms[0].centre.x, rooms[0].centre.y, 0);
        player.transform.position = newPosition;

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
