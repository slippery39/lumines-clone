using UnityEngine;
using System;

public class CustomInputInfo
{
    public KeyCode keyCode;
    public float throttleDelay = 0.3f;
    public float timeDelayAfterThrottle = 0.01f;
    public float timeDown = 0.0f;
    public float lastTimeDown = 0.0f;
    public Action handler;
}