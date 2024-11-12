using UnityEngine;
using System.Collections.Generic;

public class RoomSeeds
{
    public int roomNumber;
    public int minRoomRadius;
    public Vector2Int centre;
    public Vector2Int roomDimensions;
    public Map map;
    HashSet<Vector2Int> walls = new HashSet<Vector2Int>();
    HashSet<Vector2Int> nodesToBeChecked = new HashSet<Vector2Int>();
    Queue<Vector2Int> maxQueue = new Queue<Vector2Int>();
    List<RoomSeeds> rooms;
    public Dictionary<RoomSeeds, List<Vector2Int>> neighbouringRooms = new Dictionary<RoomSeeds, List<Vector2Int>>();
    public HashSet<RoomSeeds> redConnections = new HashSet<RoomSeeds>();
    public HashSet<RoomSeeds> blueConnections = new HashSet<RoomSeeds>();
    public HashSet<RoomSeeds> greenConnections = new HashSet<RoomSeeds>();
    public Dictionary<RoomSeeds, List<Vector2Int>> chosenDoors = new Dictionary<RoomSeeds, List<Vector2Int>>();
    public HashSet<Vector2Int> roomTiles = new HashSet<Vector2Int>();
    public bool redShrinePresent = false;
    public bool blueShrinePresent = false;
    public bool greenShrinePresent = false;
    public int roomSize = 0;
    public int doors;


    public RoomSeeds(int roomNumber, Vector2Int centre, Vector2Int roomDimensions, int minRoomRadius, Map map, List<RoomSeeds> rooms)
    {
        this.roomNumber = roomNumber;
        this.minRoomRadius = minRoomRadius;
        this.centre = centre;
        this.map = map;
        this.roomDimensions = roomDimensions;
        this.rooms = rooms;


        nodesToBeChecked = getMinRoomTiles();
        growRoom(false, true);
    }

    private HashSet<Vector2Int> getPossibleTiles(Vector2Int roomDimensions)
    {
        HashSet<Vector2Int> nodesToBeChecked = new HashSet<Vector2Int>();
        // Calculate how many squares fit along each dimension

        // Determine the starting point for the grid to be centered on the 'center'
        int startX = centre.x - (roomDimensions.x) / 2;
        int startY = centre.y - (roomDimensions.y) / 2;

        // Generate the grid of squares
        for (int i = 0; i < roomDimensions.x; i++)
        {
            for (int j = 0; j < roomDimensions.y; j++)
            {
                Vector2Int tile = new Vector2Int();
                tile.x = startX + i;
                tile.y = startY + j;
                if (tile.x >= 0 && tile.x < map.xSize && tile.y >= 0 && tile.y < map.ySize)
                {
                    nodesToBeChecked.Add(tile);
                    if ((i == 0 || i == roomDimensions.x - 1 || j == 0 || j == roomDimensions.y - 1))
                    {
                        walls.Add(tile);
                    }
                }
            }
        }
        return nodesToBeChecked;
    }


    private HashSet<Vector2Int> getMinRoomTiles()
    {
        int minX = Mathf.Min(minRoomRadius, roomDimensions.x);
        int minY = Mathf.Min(minRoomRadius, roomDimensions.y);

        return getPossibleTiles(new Vector2Int(minX, minY));
    }
    private void getMaxRoomTiles()
    {
        walls = new HashSet<Vector2Int>();
        nodesToBeChecked = getPossibleTiles(roomDimensions);
        //nodesToBeChecked.ExceptWith(getMinRoomTiles());
    }

    private void growRoom(bool includeWall, bool initialGrow)
    {
        Queue<Vector2Int> q;
        if (initialGrow)
        {
            q = new Queue<Vector2Int>();
            q.Enqueue(centre);
        }
        else
        {
            q = maxQueue;
        }

        while (q.Count > 0)
        {
            int hits = 0;
            Vector2Int current = q.Dequeue();
            if (map.getValueAt(current) != -2)
            {
                nodesToBeChecked.Remove(current);
                continue;
            }

            
            foreach (Vector2Int direction in new Vector2Int[]
            {
                new Vector2Int(1, 1),
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            })
            {
                Vector2Int neighbor = current + direction;
                if (neighbor.x >= 0 && neighbor.x < map.xSize && neighbor.y >= 0 && neighbor.y < map.ySize)
                    if (map.getValueAt(neighbor) != roomNumber && map.getValueAt(neighbor) != -1 && map.getValueAt(neighbor) != -2)
                        hits += 1;

            }

            if (walls.Contains(current) || hits >= 1)
            {
                if (includeWall && map.getValueAt(current) == -2)
                {
                    map.setCell(current, -1);
                    continue;
                }
            }
            else
            {
                if (nodesToBeChecked.Contains(current))
                {
                    map.setCell(current, roomNumber);
                    roomTiles.Add(current);
                    roomSize += 1;
                }
            }
            nodesToBeChecked.Remove(current);
            foreach (Vector2Int direction in new Vector2Int[]
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            })
            {
                Vector2Int neighbor = current + direction;
                if (nodesToBeChecked.Contains(neighbor))
                {
                    if (map.getValueAt(neighbor) == -2)
                    {
                        if (!walls.Contains(neighbor) || !initialGrow)
                        {
                            q.Enqueue(neighbor);

                        }
                        else
                        {
                            maxQueue.Enqueue(neighbor);
                            nodesToBeChecked.Remove(neighbor);
                        }

                    }
                }
            }
        }
        q.Clear();
    }

    public void expandRoom()
    {
        getMaxRoomTiles();
        growRoom(true, false);
    }

}

