using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;
  
    public GameObject clearText;
    //Initialize
    int[,] map;
    //updated position
    GameObject[,] field;
    int playerIndex;
    // GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        clearText.SetActive(false);

        Screen.SetResolution(1280, 720, false);


        map = new int[,] //1 = player, 2 = block, 3= goal
        {

        { 0, 0, 0, 0, 0, 3 },
        { 0, 0, 1, 2, 0, 0 },
        { 0, 0, 0, 2, 0, 0 },
        { 0, 3, 0, 0, 0, 0 },

        };
        field = new GameObject //create a gameobject
        [
         map.GetLength(0), //that is the same as our map
         map.GetLength(1) // ^^
        ];



        // PrintArray();

        // string debugText = "";

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1) //found player
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                    
                }

                if (map[y, x] == 2) //found block
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                    break;
                }

                if (map[y, x] == 3) //found block
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                    break;
                }

                //debugText += map[y, x].ToString() + ",";
            }
            //debugText += "\n";
        }
        //Debug.Log(debugText);
    }


    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.RightArrow)) //GetKey Every frame//GetKeyDown only when you clicked it// GetKeyUp is when you stop pressing the button
        {

            var playerIndex = GetPlayerIndex(); //same as auto, it finds the appropriate type

            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
            //PrintArray();
            if (IsCleared())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));

            //PrintArray();
            if (IsCleared())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));

            //PrintArray();
            if (IsCleared())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));

            //PrintArray();
            if (IsCleared())
            {
                clearText.SetActive(true);
            }
        }

       
    }

    //    void PrintArray()
    //    {
    //        string debugText = "";
    //        for (int i = 0; i < map.Length; i++)
    //        {
    //            debugText += map[i].ToString() + ",";
    //        }
    //        Debug.Log(debugText);
    //    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                GameObject obj = field[y, x];
                if (field[y, x] == null)
                {
                    continue;
                }
                if (obj?.tag == "Player") //? is a null check. Which means if obj.tag is not null then d
                {
                    return new Vector2Int(x, y);
                }
            }

        }
        return new Vector2Int(-1, -1);

    }

    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {

        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) //if we are outside of the map
        {
            return false; //dont return anything
        }

        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) //if we are outside of the map
        {
            return false; //dont return anything
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            {
                Vector2Int velocity = moveTo - moveFrom;
                bool success = MoveNumber(moveTo, moveTo + velocity);
                if (!success)
                {
                    return false;
                }
            }
        }

        // if (map[moveTo] == 2) //if there is a 2 where we are trying to go
        // {
        //     int velocity = moveTo - moveFrom;
        //     bool success = MoveNumber(2, moveTo, moveTo + velocity);
        //     if (!success)
        //     {
        //         return false;
        //     }
        // }
        //
        // map[moveTo] = number; //the place we moved to becomes 1
        // map[moveFrom] = 0; //the place we came from becomes 0
        //
        // return true;
        for (int i = 0; i < 5; i++)
        {
            Instantiate(particlePrefab, field[moveFrom.y, moveFrom.x].transform.position, Quaternion.identity);
        }
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        //field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        Vector3 moveToPosition = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        field[moveFrom.y, moveFrom.x] = null;
       
        return true;


    }

    bool IsCleared()
    {
        //same as Vector in C++
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3) //found a goal
                {
                    goals.Add(new Vector2Int(x, y)); //add the pos to goals
                }

            }
        }

        //foreach(var g in goals)
        //{
        //    var go = field[g.y, g.y];
        //    if(go == null  || go.tag != "Box")
        //    {
        //        return false;
        //    }
        //}

        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box") //if were not on a mapchip point where there is a "3", or if whats on top of it isnt a Box
            {
                return false; //this is false
            }
        }

        return true;

    }

}


    



