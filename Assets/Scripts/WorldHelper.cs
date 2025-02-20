using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    //playerViewRadius �ٻ��Ʊ�ԵһȦ�������Լ���ľ����������Խ�硣populateRange��popһȦ�������ࡣ
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
