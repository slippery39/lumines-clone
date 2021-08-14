using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ThrottledInput : MonoBehaviour
{

    private Dictionary<KeyCode, ThrottledInputInfo> keyInputs = new Dictionary<KeyCode, ThrottledInputInfo>();
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var registeredKeys = keyInputs.Keys;

        registeredKeys.ToList().ForEach(key =>
        {
            var inputInfo = keyInputs[key];

            if (Input.GetKeyDown(key))
            {
                inputInfo.handler();
            }
            if (Input.GetKey(key))
            {
                //Is Throttled, Can FireEvent
                if (inputInfo.totalTimeDown >= inputInfo.throttleDelay && inputInfo.timeSinceLastThrottle >= inputInfo.throttleTime)
                {
                    inputInfo.handler();
                    inputInfo.timeSinceLastThrottle = 0.0f;
                }
                else
                {
                    inputInfo.timeSinceLastThrottle += Time.deltaTime;
                }
                inputInfo.totalTimeDown += Time.deltaTime;
            }
            else
            {
                inputInfo.timeSinceLastThrottle = 0.0f;
                inputInfo.totalTimeDown = 0.0f;
            }
        });
    }

    public void AddHandler(KeyCode keyCode,Action handler)
    {
        if (keyInputs.ContainsKey(keyCode))
        {
            throw new Exception("Cannot add another handler to the CustomInput");
        }

        keyInputs.Add(keyCode, new ThrottledInputInfo
        {
            keyCode = keyCode,
            handler = handler
        });
    }

    public void ResetThrottleDelayTime(KeyCode keyCode)
    {
        if (!keyInputs.ContainsKey(keyCode))
        {
            throw new Exception("Trying to access keycode " + keyCode.ToString() + " when it doesn't exist in the CustomInput");
        }

        keyInputs[keyCode].totalTimeDown = 0.0f;

    }
}
