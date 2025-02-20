// VoxelMap类
public class VoxelMap
{
    public BlockName blockName;
    public IBlockState blockState;
    public VoxelMap()
    {
        blockName = new BlockName();
        blockState = null;
    }
    public VoxelMap(BlockName blockName)
    {
        this.blockName = blockName;
        blockState = null;
    }
    public VoxelMap(BlockName blockName, IBlockState blockState)
    {
        this.blockName = blockName;
        this.blockState = blockState;
    }
    public VoxelMap(BlockName blockName,Direction_Extended direction)
    {
        this.blockName = blockName;
        GetBlock(direction);
    }
    public IBlockState GetBlock(Direction_Extended direction)
    {
        if (blockState == null)
        {
            switch (blockName)
            {
                // case语句，为每个BlockNum类型创建对应的Block实例
                case BlockName.Wheat_1:
                case BlockName.Wheat_2:
                    blockState = new MatureAttribute_Extend();
                    break;
                case BlockName.Wood:
                case BlockName.Board:
                case BlockName.Bed:
                case BlockName.Slabs:
                case BlockName.Step:
                case BlockName.Glass:
                    blockState = new DirAttribute_Extend(direction);
                    break;
                default:
                    return null; // 默认情况，创建一个基础的Block实例
            }
            
        }
        return blockState;
    }
    public IBlockState GetBlock()
    {
        if (blockState == null)
        {
            blockState = new MatureAttribute_Extend();
        }
        return blockState;
    }
    public IBlockState GetBlockState()
    {
        return blockState;
    }
    public T GetExtend<T>() where T : IBlockState
    {
        if(blockState == null) { return default(T); }
        return (T)blockState;
    }
}
public interface IBlockState
{
    object GetData();
}
public class DirAttribute_Extend : IBlockState
{
    public Direction_Extended Direction_Extended { get; set; }
    public DirAttribute_Extend(Direction_Extended direction)
    { Direction_Extended = direction; }

    public object GetData()
    {
        return Direction_Extended;
    }
}
public class MatureAttribute_Extend : IBlockState
{
    public int Mature { get; set; }
    public bool Grow()
    {
        Mature++;
        if(Mature >= 10)
        {
            Mature = 0;
            return true;
        }
        return false;
    }

    public object GetData()
    {
        return Mature;
    }
}