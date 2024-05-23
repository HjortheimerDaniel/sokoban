using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private float timeTaken = 0.2f;
    private float timeElapsed;
    private Vector3 destination;
    private Vector3 origin;
    public void MoveTo(Vector3 newDestination) //public in order to access it in GameManager
    {
        timeElapsed = 0;
        origin = destination;
        transform.position = origin;
        destination = newDestination;
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        origin = destination;
    }

    // Update is called once per frame
    void Update()
    {
        //if we reached our destination dont do below
        if(origin == destination) { return; }
        //calculate passed time
        timeElapsed += Time.deltaTime;
        //the time passed is the much of the total time
        float timeRate = timeElapsed / timeTaken;
        //if we passed over the total time make sure we dont
        if(timeRate > 1 ) { timeRate = 1; }

        float easing = timeRate;
        //calculate position
        Vector3 currentPosition = Vector3.Lerp(origin, destination, easing);
        //enter the calculated position into currentPosition
        transform.position = currentPosition;
    }
}
