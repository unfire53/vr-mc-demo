using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navi_Mannager : MonoBehaviour
{
    //public Queue<Resident_Navi> navi_Queue = new Queue<Resident_Navi>();
    public Queue<FSM> navi_Queue = new Queue<FSM>();
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Queue_Navigate", 5f,0.5f);
    }

    // Update is called once per frame
    public void Queue_Navigate()
    {
        if(navi_Queue.Count > 0)
        {
            //Debug.Log("navi");
            FSM navigation = navi_Queue.Dequeue();
            navigation.navigate();
            //navigation.GetRandomPos();
            //navigation.Addend = true;
            //Debug.Log(navi_Queue.Count);
        }
    }
}
