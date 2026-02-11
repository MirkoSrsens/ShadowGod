using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Razorhead.Core
{
    [CreateAssetMenu(fileName = "WindowsContainer", menuName = "Data/Windows/WindowsContainer", order = 100)]
    public class WindowsContainer : ScriptableObject, IDynamicContent
    {
        public List<WindowState> initalState;
    
        public string addressableGroupId => "Windows";
    
        public void GetContent(List<IDynamicContent.AssetDefinition> outList, string overrideLabel = null)
        {
            var closedList = new HashSet<WindowState>();
            foreach (var item in initalState)
            {
                item.GetContent(outList, addressableGroupId, overrideLabel, closedList);
            }
        }
    }
}