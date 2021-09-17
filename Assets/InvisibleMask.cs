using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvisibleMask : MonoBehaviour
{

    [SerializeField]
    List<GameObject> objectsToMask = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        var renders = objectsToMask.SelectMany(obj => obj.GetComponentsInChildren<Renderer>().ToList());
 
        foreach (Renderer rendr in renders)
        {
            Debug.Log(rendr.transform.gameObject.name);
            rendr.material.renderQueue = 2004; // set their renderQueue
        }
        
    }
}
