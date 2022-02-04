using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UnityDebugExtensions
{
    public static void EnsureInitialized<T>(this T t, MonoBehaviour item) where T: Component
    {
        if (item == null)
        {
            throw new Exception($"{typeof(T).Name} has not been initialized in {item.name}");
        }
    }

 
}



