using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class Particle : MonoBehaviour
{
   
    private float Lifetime;
    private float leftLifetime;
    private Vector3 velocity;
    private Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        Lifetime = 0.3f;
        leftLifetime = Lifetime;
        defaultScale = transform.localScale; //scale relevent to the parent object
        float maxVelocity = 5.0f;
        velocity = new Vector3
        (
         Random.Range(-maxVelocity, maxVelocity), //random X-value between -5 and 5
         Random.Range(-maxVelocity, maxVelocity),
         0 //z is 0
        );

    }

    // Update is called once per frame
    void Update()
    {
        UpdateParticle();
    }

    public void UpdateParticle()
    {
        leftLifetime -= Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        transform.localScale = Vector3.Lerp
            (
            new Vector3(0, 0, 0),
            defaultScale,
            leftLifetime / Lifetime
            );
        if (leftLifetime <= 0) { Destroy(gameObject); } //if the lifetime reaches 0 destroy the particle
    }
}
