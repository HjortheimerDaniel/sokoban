using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonText : MonoBehaviour
{
    public GameObject text;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 1 && text.activeSelf)
        {
            text.SetActive(false);
            timer = 0;
        } else if(timer > 1)
        {
            text.SetActive(true);
            timer = 0;

        }



    }
}
