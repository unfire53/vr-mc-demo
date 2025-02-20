using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PerlinNoise : MonoBehaviour
{
    public static float Get2DPerlin(Vector2 pos, float offset, float scale)
    {
        return Mathf.PerlinNoise((pos.x + 0.1f) / WorldHelper.chunkSize * scale + offset, (pos.y + 0.1f) / WorldHelper.chunkSize * scale + offset);
    }
    public static float OctavePerlin(Vector2 pos, float offset, float scale)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float amplitudeSum = 0;
        for (int i = 0; i < 4; i++)
        {
            total += Get2DPerlin(pos * frequency, offset, scale) * amplitude;
            amplitudeSum += amplitude;
            amplitude /= 2;
            frequency *= 2;
        }
        return total / amplitudeSum;
    }
    public static float Spline_Evaluate(float octavePerlin,AnimationCurve animationCurve)
    {
        return animationCurve.Evaluate(octavePerlin);
    }
}
