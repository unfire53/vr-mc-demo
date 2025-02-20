using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PopulationManager:MonoBehaviour
{
    public static PopulationManager instance;
    public Queue<Vector3Int> emptyBed = new Queue<Vector3Int>();
    public int population;
    public int max_Population = 20;
    public GameObject aiPrefab;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance!=this)
            {
                Destroy(this);
            }
        }
    }
    private void Start()
    {
        InvokeRepeating("PopulationChange",0,5f);
    }
    public void PopulationChange()
    {
        while(emptyBed.Count > 0 && population < max_Population)
        {
            Vector3Int vector3 = emptyBed.Dequeue();
            Instantiate(aiPrefab, vector3 + Vector3Int.up, Quaternion.identity);
            population++;
        }
    }
}
