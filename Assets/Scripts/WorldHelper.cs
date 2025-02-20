using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    //playerViewRadius 少绘制边缘一圈的区块以及树木，以免数组越界。populateRange多pop一圈留出空余。
public static class WorldHelper
{
    public static int chunkSize = 16;
    public static int chunkHeight = 128;

    public static int DrawRadius = 7;
    public static int populateRadius = DrawRadius + 1;

    public static int Continentalnesslevel = 30;
    public static int PeakAndValleyslevel = 50;
    public static int Erosionlevel = 30;
    public static int seaLevel = 30;

    public static readonly int TextureAtlasSizeInBlocks = 8;
    public static float NormalizedBlockTextureSize
    {
        get { return (1f / (float)TextureAtlasSizeInBlocks); }
    }
}
