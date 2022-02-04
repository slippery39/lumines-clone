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
