using Sirenix.OdinInspector;
using UnityEngine;

namespace Razorhead.Core
{
    [HideMonoScript]
    public sealed class Note : MonoBehaviour
    {
        [SerializeField, TextArea(4, 8), HideLabel]
        string note;
    }
}
