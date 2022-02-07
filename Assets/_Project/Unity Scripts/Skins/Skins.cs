using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skins : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private List<Skin> skins;




    protected static Skins instance;

    public static List<Skin> All()
    {
        return instance.skins;
    }

    public static Skin GetNext(string current)
    {

        int nextIndex = 0;
        for (var i = 0; i < instance.skins.Count; i++)
        {
            var skin = instance.skins[i];
            if (skin.name == current && i<instance.skins.Count-1)
            {
                nextIndex = i + 1;
                Debug.Log("NEXT SKIN: ");
                Debug.Log(nextIndex);

                break;
            }
        }
        if (nextIndex >= instance.skins.Count)
        {
            nextIndex = 0; //loop around;
        }
        Debug.Log(nextIndex);
        Debug.Log(instance.skins[nextIndex].Name);

        return instance.skins[nextIndex];
    }


    private void Awake()
    {
        if (instance != null)
        {
            throw new System.Exception("Only 1 instance of skins should exist in a scene");
        }
        instance = this;

        skins = this.GetComponentsInChildren<Skin>().ToList();

    }

}
