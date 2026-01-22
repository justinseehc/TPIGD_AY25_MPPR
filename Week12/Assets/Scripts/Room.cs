using System.Collections.Generic;
using UnityEngine;

// 2.1 create the class
public class Room : MonoBehaviour
{
    // 2.2 directions enums
    public enum Directions
    {
        TOP,
        RIGHT,
        BOTTOM,
        LEFT,
        NONE,
    }

    // 2.3 SerializeField makes the field visible in the inspector but not for the other scripts
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private GameObject leftWall;

    // 2.4 state variables
    public Vector2Int Index
    {
        get;
        set;
    }
    public bool Visited { get; set; } = false;

    // 2.5 wall control
    private Dictionary<Directions, GameObject> walls;
    private Dictionary<Directions, bool> directionFlags;

    // 2.6 build the dictionaries
    private void Awake()
    {
        walls = new Dictionary<Directions, GameObject>
    {
        {Directions.TOP, topWall},
        {Directions.RIGHT, rightWall},
        {Directions.BOTTOM, bottomWall},
        {Directions.LEFT, leftWall},
    };

        directionFlags = new Dictionary<Directions, bool>
    {
        {Directions.TOP, true},
        {Directions.RIGHT, true},
        {Directions.BOTTOM, true},
        {Directions.LEFT, true},
        {Directions.NONE, true}
    };
    }


    // 2.7 using helper method to do the work of turning off or on the walls
    private void SetActive(Directions direction, bool active)
    {
        if (!walls.ContainsKey(direction))
        {
            return;
        }

        walls[direction].SetActive(active);
    }


    // 2.8 this method will be used by the generator script
    // purpose: stored flag and update the visible wall gameobject
    public void SetDirectionFlag(Directions direction, bool isActive)
    {
        if (!directionFlags.ContainsKey(direction))
        {
            directionFlags.Add(direction, isActive);
        }
        else
        {
            directionFlags[direction] = isActive;
        }

        SetActive(direction, isActive);
    }


    // 2.9 after game is over or to reset the room
    public void ResetRoom()
    {
        Visited = false;
        SetDirectionFlag(Directions.TOP, true);
        SetDirectionFlag(Directions.RIGHT, true);
        SetDirectionFlag(Directions.BOTTOM, true);
        SetDirectionFlag(Directions.LEFT, true);
    }

}
