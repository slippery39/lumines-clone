using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IGameText<T> where T : MonoBehaviour
    {
        public void SetText(string text);
    }

