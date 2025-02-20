using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance { get; private set; }
    private float gameTime;
    public float resetTime = 24f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            { Destroy(this); }
        }
    }
    private void Start()
    {
        InvokeRepeating("Timer", 0, 15);
    }
    void Timer()
    {
        gameTime++;
        if (gameTime >= resetTime)
        {
            ResetGameTime();
        }
    }
    public float GetGameTime()
    { return gameTime; }

    private void ResetGameTime()
    {
        gameTime = 0;
    }
}
