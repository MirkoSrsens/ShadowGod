using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class PagerScrollingList : BaseScrollingList
    {
        public int itemsPerPage;
        public Button previousPage;
        public Button nextPage;
        public int currentPage;
        public TextMeshProUGUI pageCountText;
    
        public void Awake()
        {
            previousPage.onClick.AddListener(OnPreviousClicked);
            nextPage.onClick.AddListener(OnNextClicked);
        }
    
        private void OnNextClicked()
        {
            var newPage = Mathf.Clamp(currentPage + 1, 0, Mathf.RoundToInt(Mathf.Ceil(entities.Count / (float)itemsPerPage))-1);
            View(newPage);
        }
    
        private void OnPreviousClicked()
        {
            var newPage = Mathf.Clamp(currentPage -1, 0, Mathf.RoundToInt(Mathf.Ceil(entities.Count/(float)itemsPerPage)));
            View(newPage);
        }
    
    
    
        public void Refresh()
        {
            View(0, true);
        }
    
        public void View(int newPage, bool hardReset = false)
        {
            if (newPage == currentPage && !hardReset)
            {
                return;
            }
            
            currentPage = newPage;
            for(int i =0; i < entities.Count; i++)
            {
                var item = entities[i];
                if(item.spawnedElement != null)
                {
                    Pool.Inst.Return(item.spawnedElement);
                    item.spawnedElement = null;
                }
    
                entities[i] = item;
            }
    
            var startIndex = itemsPerPage * newPage;
            var endIndex = Mathf.Clamp(startIndex + itemsPerPage, 0, entities.Count);
            for(int i = startIndex; i < endIndex; i++)
            {
                var entity = entities[i];
                var inst = Pool.Inst.Spawn(entity.prefab, container, container.transform.position);
                inst.Setup(entities[i].data);
    
                var item = entities[i];
                item.spawnedElement = inst;
                entities[i] = item;
            }
    
            if (pageCountText != null)
            {
                pageCountText.text = string.Format("{0}/{1}", newPage + 1, Mathf.RoundToInt(Mathf.Ceil(entities.Count / (float)itemsPerPage)));
            }
        }
    }
}