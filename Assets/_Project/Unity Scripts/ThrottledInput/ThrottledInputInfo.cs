using UnityEngine;
using System;

public class ThrottledInputInfo
{
    public KeyCode keyCode;
    public float throttleDelay = 0.25f; //delay
    public float throttleTime = 0.02f; //the amount of time that it will run.
    public float totalTimeDown = 0.0f;
    public float timeSinceLastThrottle = 0.0f;
    public Action handler;
}