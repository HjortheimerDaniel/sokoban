using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    int[,] map;
    GameObject[,] field;
    int playerIndex;
    // GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        //string debugText = "";


        map = new int[,]
        {

        { 0, 0, 0, 0, 0, 0 },
        { 0, 0, 1, 2, 0, 0 },
        { 0, 0, 0, 2, 0, 0 },
        { 0, 0, 0, 0, 0, 0 },

        };
        field = new GameObject
        [
         map.GetLength(0),
         map.GetLength(1)
        ];



        // PrintArray();

        // string debugText = "";

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
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

            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));

            //PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1,0));

            //PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));

            //PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, -1));

            //PrintArray();
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
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (field[y, x] == null)
                {
                    continue;
                }
                if (field[y,x].tag == "Player")
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

            field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, map.GetLength(0) - moveTo.y, 0);
        field[moveFrom.y, moveFrom.x] = null;
        return true;


    }}


    //if (playerIndex < map.Length - 1) //if were not at the end
    //{
    //    map[playerIndex + 1] = 1; //move to the right
    //    map[playerIndex] = 0; //the place we were at becomes zero
    //}



