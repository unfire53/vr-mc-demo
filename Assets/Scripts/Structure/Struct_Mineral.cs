using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Structure_Tree1
{
    public static int x = 5, y = 8, z = 5;
    public static Vector3Int offset = new Vector3Int(2,0,2);
    /// <summary>
    /// 树木模型
    /// </summary>
    
    public static Structure_Node[] structure_Nodes = new Structure_Node[]
    {
        new Structure_Node(2,0,2,new VoxelMap(BlockName.Wood,Direction_Extended.Cube_Up)),
        
        new Structure_Node(2,1,2,new VoxelMap(BlockName.Wood,Direction_Extended.Cube_Up)),
        
        new Structure_Node(2,2,2,new VoxelMap(BlockName.Wood,Direction_Extended.Cube_Up)),
        
        new Structure_Node(2,3,2,new VoxelMap(BlockName.Wood,Direction_Extended.Cube_Up)),
        
        new Structure_Node(0,4,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,4,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,4,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,4,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,4,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(1,4,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,4,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,4,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,4,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,4,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(2,4,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,4,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,4,2,new VoxelMap(BlockName.Wood, Direction_Extended.Cube_Up)),new Structure_Node(2,4,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,4,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(3,4,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,4,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,4,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,4,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,4,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(4,4,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,4,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,4,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,4,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,4,4,new VoxelMap(BlockName.Leaves)),

        new Structure_Node(0,5,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,5,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,5,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,5,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(0,5,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(1,5,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,5,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,5,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,5,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,5,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(2,5,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,5,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,5,2,new VoxelMap(BlockName.Wood, Direction_Extended.Cube_Up)),new Structure_Node(2,5,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,5,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(3,5,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,5,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,5,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,5,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,5,4,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(4,5,0,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,5,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,5,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,5,3,new VoxelMap(BlockName.Leaves)),new Structure_Node(4,5,4,new VoxelMap(BlockName.Leaves)),

                                                                 
        new Structure_Node(1,6,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,6,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(1,6,3,new VoxelMap(BlockName.Leaves)),
        new Structure_Node(2,6,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(2,6,2,new VoxelMap(BlockName.Wood, Direction_Extended.Cube_Up)),new Structure_Node(2,6,3,new VoxelMap(BlockName.Leaves)),  
        new Structure_Node(3,6,1,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,6,2,new VoxelMap(BlockName.Leaves)),new Structure_Node(3,6,3,new VoxelMap(BlockName.Leaves)),
        

        new Structure_Node(2,7,2,new VoxelMap(BlockName.Leaves)),
    };
}
