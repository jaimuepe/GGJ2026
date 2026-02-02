namespace Masks
{
    public static class DeviceCheck
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsMobileBrowser();
      
        [System.Runtime.InteropServices.DllImport("__Internal")]
        public static extern bool IsPreferredDesktopPlatform();
#else
        public static bool IsMobileBrowser() => false;
        public static bool IsPreferredDesktopPlatform() => true;
#endif
    }
}