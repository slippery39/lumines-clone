using UnityEngine;
using System;

public class ThrottledInputInfo
{
    public KeyCode keyCode;
    public float throttleDelay = 0.3f;
    public float throttleTime = 0.01f;
    public float totalTimeDown = 0.0f;
    public float timeSinceLastThrottle = 0.0f;
    public Action handler;
}