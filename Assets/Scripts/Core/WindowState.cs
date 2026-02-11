using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

namespace Razorhead.Core
{
    [CreateAssetMenu(fileName = "WindowState", menuName = "Data/Windows/WindowState", order = 100)]
    public class WindowState : ScriptableObject
    {
        [Serializable]
        public class WindowOverides
        {
            public AssetReferenceGameObject landscape;
            public AssetReferenceGameObject portrait;
        }
    
        public List<string> requiredConditions;
        public List<WindowOverides> windows;
    
        public SerializableDictionary<string, WindowState> triggers;
        public bool consumeTrigger = true;
    
    
        public bool CanBeVisible(SerializableDictionary<string, bool> requiredConditions)
        {
            foreach (var item in this.requiredConditions)
            {
                requiredConditions.TryGetValue(item, out bool condition);
                if (!condition)
                {
                    return false;
                }
            }
    
            return true;
        }
    
        public void GetWindows(List<AssetReferenceGameObject> outWindow)
        {
            var isLandscape = Screen.width > Screen.height;

            foreach (var item in windows)
            {
                if (isLandscape)
                {
                    outWindow.Add(!item.landscape.IsNull() ? item.landscape : item.portrait);
                }
                else
                {
                    outWindow.Add(!item.portrait.IsNull() ? item.portrait : item.landscape);
                }
            }
        }
    
        public void GetContent(List<IDynamicContent.AssetDefinition> outList, string groupId, string overrideLabel, HashSet<WindowState> closedList)
        {
            closedList.Add(this);
            foreach (var item in windows)
            {
                outList.Add(new IDynamicContent.AssetDefinition()
                {
                    label = overrideLabel.IsEmpty() ? this.name : overrideLabel,
                    group = groupId,
                    asset = item.landscape,
                });
    
                outList.Add(new IDynamicContent.AssetDefinition()
                {
                    label = overrideLabel.IsEmpty() ? this.name : overrideLabel,
                    group = groupId,
                    asset = item.portrait,
                });
            }
    
            foreach(var item in triggers)
            {
                //We need to prevent circular loops with closed list (when one state transitons to others and vice versa)
                if(!closedList.Contains(item.Value))
                {
                    item.Value.GetContent(outList, groupId, overrideLabel, closedList);
                }
            }
        }
    }
}