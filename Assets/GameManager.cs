using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int[] map;
    int playerIndex;

    // Start is called before the first frame update
    void Start()
    {
        //string debugText = "";

        map = new int[] { 0, 2, 0, 1, 0, 2, 0, 2, 0 };
        PrintArray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) //GetKey Every frame//GetKeyDown only when you clicked it// GetKeyUp is when you stop pressing the button
        {

            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex + 1);

            PrintArray();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = GetPlayerIndex();

            MoveNumber(1, playerIndex, playerIndex - 1);

            PrintArray();
        }
    }

    void PrintArray()
    {
        string debugText = "";
        for (int i = 0; i < map.Length; i++)
        {
            debugText += map[i].ToString() + ",";
        }
        Debug.Log(debugText);
    }

    int GetPlayerIndex()
    {
        playerIndex = -1;

        for (int i = 0; i < map.Length; i++)
        {
            if (map[i] == 1) //Find the player
            {
                return i;
            }
        }
        return -1;
    }

    bool MoveNumber(int number, int moveFrom, int moveTo)
    {
        if(moveTo < 0 || moveTo >= map.Length) //if we are outside of the map
        {
            return false; //dont return anything
        }

        if (map[moveTo] == 2) //if there is a 2 where we are trying to go
        {
            int velocity = moveTo - moveFrom;
            bool success = MoveNumber(2, moveTo, moveTo + velocity);
            if (!success)
            {
                return false;
            }
        }

        map[moveTo] = number; //the place we moved to becomes 1
        map[moveFrom] = 0; //the place we came from becomes 0

        return true;
    }
}

//if (playerIndex < map.Length - 1) //if were not at the end
//{
//    map[playerIndex + 1] = 1; //move to the right
//    map[playerIndex] = 0; //the place we were at becomes zero
//}


