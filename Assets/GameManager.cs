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

        map = new int[] { 0, 0, 0, 1, 0, 0, 0, 0, 0 };
        playerIndex = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) //Every frame//GetKeyDown only when you clicked it// GetKeyUp is when you stop pressing the button
        {

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1) //Find the player
                {
                    playerIndex = i;
                    break;
                }
            }

            if (playerIndex < map.Length - 1) //if were not at the end
            {
                map[playerIndex + 1] = 1; //move to the right
                map[playerIndex] = 0; //the place we were at becomes zero
            }

            string debugText = "";
            for (int i = 0; i < map.Length; i++)
            {
                debugText += map[i].ToString() + ",";
            }

            Debug.Log(debugText);

        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[i] == 1) //Find the player
                {
                    playerIndex = i;
                    break;
                }
            }

            if (playerIndex > 0) //if were not at the beginning
            {
                map[playerIndex - 1] = 1; //move to the left
                map[playerIndex] = 0; //the place we were at becomes zero
            }
            string debugText1 = "";
            for (int i = 0; i < map.Length; i++)
            {
                debugText1 += map[i].ToString() + ",";
            }
            Debug.Log(debugText1);
        }


    }

}


