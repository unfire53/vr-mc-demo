using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PredictVoxel_Pool : MonoBehaviour
{
    public static PredictVoxel_Pool instance;
    public Queue<PredictVoxel> predictVoxels = new Queue<PredictVoxel>();
    public World world;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this);
            }
        }
    }

    public void EnQueue(PredictVoxel block)
    {
        block.predictVoxelObject.SetActive(false);
        predictVoxels.Enqueue(block);
    }
    public PredictVoxel DeQueue(List<Vector3> _vertices, List<int> _triangles)
    {
        PredictVoxel block;
        if (predictVoxels.Count <= 0)
        {
            block = new PredictVoxel(_vertices, _triangles,world);
        }
        else
        {
            block = predictVoxels.Dequeue();
            block.predictVoxelObject.SetActive(true);
            block.Set(_vertices, _triangles);
        }
        return block;
    }

}
