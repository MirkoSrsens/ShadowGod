using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Razorhead.Core
{
    public class BaseScrollingList : MonoBehaviour
    {
        public enum ScrollDirection
        {
            Horizontal,
            Vertical,
        }

        public class Entity
        {
            public object data;
            public ScrollableElement prefab;
            public ScrollableElement spawnedElement;
            public Rect? rect;
        }

        public RectTransform container;
        public int Count => entities.Count;
        public List<Entity> entities = new();
        public Dictionary<ScrollableElement, Queue<ScrollableElement>> localQueue = new();

        protected bool layoutDirty = true;

        public ScrollableElement GetElementAt(int index)
        {
            if (entities.Count > index)
            {
                return entities[index].spawnedElement;
            }

            return null;
        }

        public void SetLayoutDirty()
        {
            layoutDirty = true;
        }

        public virtual void Restart()
        {
            foreach (var item in entities)
            {
                if (item.spawnedElement != null)
                {
                    if (!localQueue.TryGetValue(item.prefab, out var queue))
                    {
                        queue = new Queue<ScrollableElement>();
                        localQueue.Add(item.prefab, queue);
                    }
                    queue.Enqueue(item.spawnedElement);
                    item.spawnedElement.gameObject.SetActive(false);
                }
            }
            entities.Clear();
            SetLayoutDirty();
        }

        public ScrollableElement SpawnItem(ScrollableElement prefab, Vector2 position)
        {
            if (localQueue.TryGetValue(prefab, out var queue) && queue.Count > 0)
            {
                var pooled = queue.Dequeue();
                pooled.GetComponent<RectTransform>().anchoredPosition = position;
                pooled.gameObject.SetActive(true);
                return pooled;
            }

            return Pool.Inst.Spawn(prefab, container, position);
        }

        public void ClearLocalPool()
        {
            foreach (var item in localQueue)
            {
                while (item.Value.Count > 0)
                {
                    Pool.Inst.Return(item.Value.Dequeue());
                }
            }
        }

        internal void AddItem(object data, ScrollableElement element)
        {
            this.entities.Add(new Entity()
            {
                data = data,
                prefab = element,
            });

            SetLayoutDirty();
        }

        public void SelectFirst()
        {
            if (!EventSystem.current) return;

            foreach (var item in entities)
            {
                if (item.spawnedElement != null)
                {
                    var selecable = item.spawnedElement.GetComponentInChildren<Selectable>(false);

                    if (selecable)
                    {
                        EventSystem.current.SetSelectedGameObject(selecable.gameObject);
                        return;
                    }
                }
            }
        }
    }
}
