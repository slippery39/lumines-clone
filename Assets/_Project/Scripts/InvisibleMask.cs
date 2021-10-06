using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InvisibleMask : MonoBehaviour
{

    [SerializeField]
    List<GameObject> objectsToMask = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        
        var renders = objectsToMask.SelectMany(obj => obj.GetComponentsInChildren<Renderer>().ToList());
        
 
        foreach (Renderer rendr in renders)
        {
            rendr.material.renderQueue = 3052; // this render queue must be after the Invisible Mask shader render queue.
        }

        var textRenderers = objectsToMask.SelectMany(obj=>GetComponentsInChildren<TMP_Text>().ToList());

        textRenderers.ToList().ForEach(txt => txt.fontMaterial.renderQueue = 3052);

    }
}
