using UnityEngine;
using System;
#if UNITY_IOS
// Include the IosSupport namespace if running on iOS:
using Unity.Advertisement.IosSupport;
#endif

public static class AttPermissionRequest
{
    public static void StartGetPermissionTracking(Action EventCallback)
    {
#if UNITY_IOS
        // Check the user's consent status.
        // If the status is undetermined, display the request request:
        if (CanTrackATT())
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            EventCallback?.Invoke();
        }
#endif
    }

    public static bool CanTrackATT()
    {
#if UNITY_IOS
        return ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED;
#endif
        return false;
    }
}