using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
    // 4.1 inspector fields
    [Header("Prefab")]
    [SerializeField] private GameObject roomPrefab;

    [Header("Grid Size")]
    [SerializeField] private int numX = 10;
    [SerializeField] private int numY = 10;

    // 4.2 storage for grid and room size
    private Room[,] rooms;
    private float roomWidth;
    private float roomHeight;

    // 4.3 getting room size
    private void GetRoomSize()
    {
        SpriteRenderer[] renderers = roomPrefab.GetComponentsInChildren<SpriteRenderer>();

        Vector3 minBounds = new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0f);
        Vector3 maxBounds = new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0f);

        for (int i = 0; i < renderers.Length; i++)
        {
            Bounds b = renderers[i].bounds;
            minBounds = Vector3.Min(minBounds, b.min);
            maxBounds = Vector3.Max(maxBounds, b.max);
        }

        roomWidth = maxBounds.x - minBounds.x;
        roomHeight = maxBounds.y - minBounds.y;
    }

    // 4.4 (+ video + chatgpt for extra assistance) create grid
    private void CreateGrid()
    {
        rooms = new Room[numX, numY];

        for (int i = 0; i < numX; ++i)
        {
            for (int j = 0; j < numY; ++j)
            {
                GameObject room = Instantiate(roomPrefab,
                    new Vector3(i * roomWidth, j * roomHeight, 0.0f),
                    Quaternion.identity);

                room.SetActive(true);
                room.name = $"Room ({i},{j})";
                rooms[i, j] = room.GetComponent<Room>();
                rooms[i, j].Index = new Vector2Int(i, j);
            }
        }
    }

    // 4.5 setup the camera
    private void SetCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // The first part of the function calculates the position of the camera (at the 
        // center of the grid in the XZ plane, looking down from z = -10f.)

        float totalWidth = numX * roomWidth;
        float totalHeight = numY * roomHeight;

        Vector3 centrePos = new Vector3(
                (totalWidth - roomWidth) / 2f, (totalHeight - roomHeight) / 2f, -10f);
        cam.transform.position = centrePos;


        // The second part of the function calculates the orthographic size of the camera. 
        // This determines how much of the scene will be visible  to the camera.


        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = totalWidth / totalHeight;

        if (targetRatio >= screenRatio)
        {
            cam.orthographicSize = (totalWidth / 2f) / screenRatio;
        }
        else
        {
            cam.orthographicSize = totalHeight / 2f;
        }

        cam.orthographicSize += 1f;
    }

    // 5.1 add stack and generating flags
    private Stack<Room> roomStack = new Stack<Room>();
    private bool generating = false;

    // 5.2 reset the maze method
    private void ResetMaze()
    {
        for (int x = 0; x < numX; x++)
        {
            for (int y = 0; y < numY; y++)
            {
                rooms[x, y].ResetRoom();
            }
        }
        roomStack.Clear();
    }

    // 6.1 remove a specific room wall
    private void RemoveRoomWall(int x, int y, Room.Directions direction)
    {
        if (direction == Room.Directions.NONE) return;

        rooms[x, y].SetDirectionFlag(direction, false);

        int nx = x;
        int ny = y;
        Room.Directions opposite = Room.Directions.NONE;

        switch (direction)
        {
            case Room.Directions.TOP:
                ny = y + 1;
                opposite = Room.Directions.BOTTOM;
                break;

            case Room.Directions.RIGHT:
                nx = x + 1;
                opposite = Room.Directions.LEFT;
                break;

            case Room.Directions.BOTTOM:
                ny = y - 1;
                opposite = Room.Directions.TOP;
                break;

            case Room.Directions.LEFT:

                nx = x - 1;
                opposite = Room.Directions.RIGHT;
                break;
        }

        if (nx < 0 || nx >= numX || ny < 0 || ny >= numY) return;

        rooms[nx, ny].SetDirectionFlag(opposite, false);
    }

    // 7.1 Finding unvisited neighbours
    private List<(Room.Directions, Room)> GetUnvisitedNeighbours(int cx, int cy)
    {
        List<(Room.Directions, Room)> neighbours = new List<(Room.Directions, Room)>();

        foreach (Room.Directions d in System.Enum.GetValues(typeof(Room.Directions)))
        {
            if (d == Room.Directions.NONE)
            {
                continue;
            }

            int nx = cx;
            int ny = cy;

            switch (d)
            {
                case Room.Directions.TOP: ny = cy + 1; break;
                case Room.Directions.RIGHT: nx = cx + 1; break;
                case Room.Directions.BOTTOM: ny = cy - 1; break;
                case Room.Directions.LEFT: nx = cx - 1; break;
            }

            if (nx < 0 || nx >= numX || ny < 0 || ny >= numY) continue;

            Room neighbourRoom = rooms[nx, ny];
            if (!neighbourRoom.Visited)
            {
                neighbours.Add((d, neighbourRoom));
            }
        }
        return neighbours;
    }

    // 8.1 backtracking steps
    private bool GenerateStep()
    {
        if (roomStack.Count == 0)
        {
            return true;
        }

        Room current = roomStack.Peek();
        int cx = current.Index.x;
        int cy = current.Index.y;

        List<(Room.Directions, Room)> neighbours = GetUnvisitedNeighbours(cx, cy);

        if (neighbours.Count > 0)
        {
            int r = UnityEngine.Random.Range(0, neighbours.Count);
            Room.Directions dir = neighbours[r].Item1;
            Room nextRoom = neighbours[r].Item2;

            nextRoom.Visited = true;
            RemoveRoomWall(cx, cy, dir);
            roomStack.Push(nextRoom);
        }
        else
        {
            roomStack.Pop();
        }

        return false;
    }

    // 9.1 coroutine to generate the maze
    private IEnumerator GenerateRoutine()
    {
        generating = true;

        bool complete = false;
        while (!complete)
        {
            complete = GenerateStep();
            yield return new WaitForSeconds(0.05f);
        }

        generating = false;
    }

    // 10.1 Create the maze
    private void CreateMaze()
    {
        if (generating) return;

        ResetMaze();

        RemoveRoomWall(0, 0, Room.Directions.BOTTOM);
        RemoveRoomWall(numX - 1, numY - 1, Room.Directions.RIGHT);

        Room startRoom = rooms[0, 0];
        startRoom.Visited = true;
        roomStack.Push(startRoom);

        StartCoroutine(GenerateRoutine());
    }


    // 10.2 Spacebar input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !generating)
        {
            CreateMaze();
        }
    }

    private void Start()
    {
        GetRoomSize();
        CreateGrid();
        SetCamera();
    }
}