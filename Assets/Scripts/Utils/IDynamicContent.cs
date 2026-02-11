using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Razorhead.Core
{
    public interface IDynamicContent
    {
        public struct AssetDefinition
        {
            public string group;
            public string label;
            public AssetReference asset;
        }

        public string addressableGroupId { get; }
        void GetContent(List<AssetDefinition> outList, string overrideLabel = null);
    }
}