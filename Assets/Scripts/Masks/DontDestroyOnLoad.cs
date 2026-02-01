#nullable enable

using System;
using UnityEngine;

namespace Masks
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}