using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Razorhead.Core
{
    public static class AddressableUtils
    {
        public static bool IsNull(this AssetReference asset)
        {
            return asset == null || asset.AssetGUID.IsEmpty();
        }
    }
}