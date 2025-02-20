using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Structure_Iron
{
    public static int x = 3, y = 3, z = 3;
    public static Vector3Int offset = new Vector3Int(1,1,1);
    public static Structure_Node[] structure_Nodes = new Structure_Node[]
    {
                                                                new Structure_Node(1,0,0,new VoxelMap(BlockName.Iron)),
        new Structure_Node(0,0,1,new VoxelMap(BlockName.Iron)),                                                     new Structure_Node(2,0,1,new VoxelMap(BlockName.Iron)),
                                                                new Structure_Node(1,0,2,new VoxelMap(BlockName.Iron)),


        new Structure_Node(0,1,0,new VoxelMap(BlockName.Iron)),                                                     new Structure_Node(0,1,2,new VoxelMap(BlockName.Iron)),
                                                                new Structure_Node(1,1,1,new VoxelMap(BlockName.Iron)),
        new Structure_Node(2,1,0,new VoxelMap(BlockName.Iron)),                                                     new Structure_Node(2,1,2,new VoxelMap(BlockName.Iron)),


                                                                new Structure_Node(1,2,0,new VoxelMap(BlockName.Iron)),
        new Structure_Node(0,2,1,new VoxelMap(BlockName.Iron)),                                                     new Structure_Node(2,2,1,new VoxelMap(BlockName.Iron)),
                                                                new Structure_Node(1,2,2,new VoxelMap(BlockName.Iron)),
    };
}

public class Structure_Coal
{
    public static int x = 3, y = 3, z = 3;
    public static Vector3Int offset = new Vector3Int(1, 1, 1);
    public static Structure_Node[] structure_Nodes = new Structure_Node[]
    {
                                                                new Structure_Node(1,0,0,new VoxelMap(BlockName.Coal)),
        new Structure_Node(0,0,1,new VoxelMap(BlockName.Coal)),                                                     new Structure_Node(2,0,1,new VoxelMap(BlockName.Coal)),
                                                                new Structure_Node(1,0,2,new VoxelMap(BlockName.Coal)),


        new Structure_Node(0,1,0,new VoxelMap(BlockName.Coal)),                                                     new Structure_Node(0,1,2,new VoxelMap(BlockName.Coal)),
                                                                new Structure_Node(1,1,1,new VoxelMap(BlockName.Coal)),
        new Structure_Node(2,1,0,new VoxelMap(BlockName.Coal)),                                                     new Structure_Node(2,1,2,new VoxelMap(BlockName.Coal)),


                                                                new Structure_Node(1,2,0,new VoxelMap(BlockName.Coal)),
        new Structure_Node(0,2,1,new VoxelMap(BlockName.Coal)),                                                     new Structure_Node(2,2,1,new VoxelMap(BlockName.Coal)),
                                                                new Structure_Node(1,2,2,new VoxelMap(BlockName.Coal)),
    };
}
