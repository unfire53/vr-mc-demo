using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot_Sensor : MonoBehaviour
{
    public BoxCollider BoxCollider;
    private bool OnTheGround = false;
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
    }

    public bool OnGround()
    {
        return OnTheGround;
    }
    private void OnTriggerStay(Collider other)
    {
        OnTheGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        OnTheGround = false;
    }
}
