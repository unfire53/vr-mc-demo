using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Structure_MushRoom
{
    public static int x = 1, y = 1, z = 1;

    public static Structure_Node[] structure_Nodes = new Structure_Node[]
    {
        new Structure_Node(Vector3Int.zero,new VoxelMap(BlockName.Mushroom))
    };
}
public class Structure_Vegetable
{
    public static int x = 1, y = 1, z = 1;

    public static Structure_Node[] structure_Nodes = new Structure_Node[]
    {
        new Structure_Node(Vector3Int.zero,new VoxelMap(BlockName.Vegetable))
    };
}
