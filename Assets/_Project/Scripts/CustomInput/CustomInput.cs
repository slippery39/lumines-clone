using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CustomInput : MonoBehaviour
{

    private Dictionary<KeyCode, CustomInputInfo> keyInputs = new Dictionary<KeyCode, CustomInputInfo>();
   
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
                if (inputInfo.timeDown >= inputInfo.throttleDelay && inputInfo.lastTimeDown >= inputInfo.timeDelayAfterThrottle)
                {
                    inputInfo.handler();
                    inputInfo.lastTimeDown = 0.0f;
                }
                else
                {
                    inputInfo.lastTimeDown += Time.deltaTime;
                }
                inputInfo.timeDown += Time.deltaTime;
            }
            else
            {
                inputInfo.lastTimeDown = 0.0f;
                inputInfo.timeDown = 0.0f;
            }
        });
    }

    public void AddHandler(KeyCode keyCode,Action handler)
    {
        if (keyInputs.ContainsKey(keyCode))
        {
            throw new Exception("Cannot add another handler to the CustomInput");
        }

        keyInputs.Add(keyCode, new CustomInputInfo
        {
            keyCode = keyCode,
            handler = handler
        });
    }

    public void ResetThrottleTime(KeyCode keyCode)
    {
        if (!keyInputs.ContainsKey(keyCode))
        {
            throw new Exception("Trying to access keycode " + keyCode.ToString() + " when it doesn't exist in the CustomInput");
        }

        keyInputs[keyCode].timeDown = 0.0f;

    }
}
