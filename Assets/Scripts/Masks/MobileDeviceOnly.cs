using System;
using UnityEngine;

namespace Masks
{
    public class MobileDeviceOnly : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR && UNITY_ANDROID || UNITY_IOS
            return;
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
            if (DeviceCheck.IsMobileBrowser())
            {
                return;
            }
#endif

            Destroy(gameObject);
        }
    }
}