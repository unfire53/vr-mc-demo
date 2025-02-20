using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{

    public Transform attachPoint;
    public float lerpSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, attachPoint.position, lerpSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, attachPoint.rotation, lerpSpeed * Time.deltaTime);
    }
}
