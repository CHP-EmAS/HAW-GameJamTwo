using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinAnim : MonoBehaviour
{
    [SerializeField] private float speed, speed2 = 1;
    Vector3 ogPos, ogogPos;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        ogogPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * speed2) * speed, transform.position.z);
    }
}
