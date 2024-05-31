using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // private bool isGameCleared = false;
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;
    public GameObject wallPrefab;
    public GameObject clearText;
    [SerializeField] Material[] _mats; //List of all the materials we want to change between
    Renderer _r;
    private Dictionary<GameObject, bool> boxMaterialChanged;     // Dictionary to track boxes that have changed their material
    int _i;
    //private bool isStage1Clear = false;
    private float timer = 0;
    [SerializeField]
    private int stageNumber;
    SFXManager SFXManager;

    //Initialize
    int[,,] map;
    //updated position
    GameObject[,] field;
    int playerIndex;

    private void Awake()
    {
        SFXManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _r = GetComponent<Renderer>();
        clearText.SetActive(false);

        Screen.SetResolution(1280, 720, false);

        map = new int[,,] //1 = player, 2 = block, 3= goal, 4 wall
        {
          {
            { 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 0, 1, 2, 3, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 0, 0, 2, 0, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 3, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },

          },

          {
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0},
            { 4, 0, 1, 0, 3, 0, 2, 0, 4, 0, 0, 0, 0},
            { 4, 0, 0, 2, 0, 0, 0, 0, 4, 0, 0, 0, 0},
            { 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
            { 4, 3, 0, 0, 0, 0, 2, 0, 4, 0, 0, 0, 0},
            { 4, 0, 2, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0},
            { 4, 0, 0, 0, 3, 0, 0, 0, 4, 0, 0, 0, 0},
            { 4, 0, 0, 0, 0, 0, 3, 0, 4, 0, 0, 0, 0},
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
           },

           {
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            { 4, 0, 1, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4},
            { 4, 0, 0, 2, 0, 0, 4, 0, 0, 0, 0, 0, 4},
            { 4, 4, 4, 4, 4, 3, 0, 0, 0, 4, 2, 0, 4},
            { 4, 3, 0, 0, 0, 4, 0, 0, 0, 4, 0, 0, 4},
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 4},
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4},
            { 4, 0, 4, 0, 0, 0, 0, 0, 0, 0, 4, 0, 4},
            { 4, 2, 0, 0, 4, 0, 3, 0, 0, 2, 4, 3, 4},
            { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 4},
            { 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 4},
            { 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
           }
        };

        field = new GameObject //create a gameobject
        [
         map.GetLength(1), //that is the same as our map
         map.GetLength(2) // ^^
        ];
        boxMaterialChanged = new Dictionary<GameObject, bool>();
        InitializeField();
        


        // PrintArray();

        // string debugText = "";


        //Debug.Log(debugText);
    }


    // Update is called once per frame
    void Update()
    {
        if (StageClearTimer()) 
        {
            Scene scene = SceneManager.GetActiveScene();

            if (scene.name == "SampleScene")
            {
                timer = 0.0f;
                SceneManager.LoadScene("Stage2");
                
            }
            else if (scene.name == "Stage2")
            {
                timer = 0.0f;
                SceneManager.LoadScene("Stage3");

            }
            else if (scene.name == "Stage3")
            {
                timer = 0.0f;
                SceneManager.LoadScene("ClearScreen");

            }
        };

        if (Input.GetKeyDown(KeyCode.RightArrow)) //GetKey Every frame//GetKeyDown only when you clicked it// GetKeyUp is when you stop pressing the button
        {
            var playerIndex = GetPlayerIndex(); //same as auto, it finds the appropriate type
            
            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
            //PrintArray();
            CheckAndChangeBoxMaterial();
            if (IsCleared())
            {
                clearText.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            var playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(-1, 0));
            CheckAndChangeBoxMaterial();
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
            CheckAndChangeBoxMaterial();
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
            CheckAndChangeBoxMaterial();
            //PrintArray();
            if (IsCleared())
            {

                Scene scene = SceneManager.GetActiveScene();
                clearText.SetActive(true);


            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

    }


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

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            SFXManager.PlaySFX(SFXManager.touchedWall);

            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            {
                GameObject box = field[moveTo.y, moveTo.x];

                // Check if the box material has been changed
                if (boxMaterialChanged.ContainsKey(box) && boxMaterialChanged[box]) 
                {
                    SFXManager.PlaySFX(SFXManager.touchedWall);
                    return false; // Box has already been moved and its material changed
                }
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
        SFXManager.PlaySFX(SFXManager.walk);

        return true;


    }

    bool IsCleared()
    {
        //same as Vector in C++
        List<Vector2Int> goals = new List<Vector2Int>();

        

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(2); x++)
            {
               
                if (map[stageNumber, y, x] == 3) //found a goal
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

    void InitializeField()
    {
        var objects = GameObject.FindGameObjectsWithTag("Goal");
        foreach (var obj in objects) { Destroy(obj); }

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(2); x++)
            {
                if (field[y, x] != null)
                {
                    Destroy(field[y, x]);
                }
                if (map[stageNumber, y, x] == 1) //found player
                {
                    field[y, x] = Instantiate(playerPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);

                }

                if (map[stageNumber, y, x] == 2) //found block
                {
                    field[y, x] = Instantiate(boxPrefab, new Vector3(x, map.GetLength(0) - y, 0), Quaternion.identity);
                    //break;
                }

                if (map[stageNumber, y, x] == 3) //found goal
                {
                    field[y, x] = Instantiate(goalPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                    //break;
                }

                if (map[stageNumber, y, x] == 4) //found block
                {
                    field[y, x] = Instantiate(wallPrefab, new Vector3(x, map.GetLength(0) - y, 0.01f), Quaternion.identity);
                    //break;
                }
            }
        }
    }

    void CheckAndChangeBoxMaterial()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(2); x++)
            {
                if (map[stageNumber, y, x] == 3)
                {
                    GameObject f = field[y, x];

                    if (f != null && f.tag == "Box" && !boxMaterialChanged.ContainsKey(f))
                    {
                        Renderer boxRenderer = f.GetComponent<Renderer>();
                        if (boxRenderer != null)
                        {
                            SFXManager.PlaySFX(SFXManager.goal);
                            boxRenderer.material = _mats[1];
                            _i++;
                            boxMaterialChanged[f] = true;
                        }
                    }
                }
            }
        }
    }

    void ResetGame()
    {
        clearText.SetActive(false);

        InitializeField();
        boxMaterialChanged.Clear();
        _i = 0;

        //isGameCleared = false;

        Vector2Int startPosition = new Vector2Int(1, 2);
        GameObject player = field[startPosition.y, startPosition.x];
        if (player != null && player.tag == "Player")
        {
            player.transform.position = IndexToPosition(startPosition);
        }

    }

    bool StageClearTimer()
    {
        if (IsCleared())
        {
            timer += Time.deltaTime;
        }

        if(timer >= 2.0f)
        {
            //isStage1Clear = true;
            return true;
        }
        return false;
    }

    Vector3 IndexToPosition(Vector2Int index)
    {
        return new Vector3(
            index.x - map.GetLength(1) / 2 + 0.5f,
            index.y - map.GetLength(0) / 2 + 0.5f, 0
            );
    }

}


    



