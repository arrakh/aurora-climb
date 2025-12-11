using System;
using UnityEngine;
using UnityEngine.Events;

namespace AuroraClimb.Controllers
{
    public class GlobalFlagListener : MonoBehaviour
    {
        [SerializeField] private string flagId;
        [SerializeField] private UnityEvent onFlagAdded;
        [SerializeField] private UnityEvent onFlagRemoved;
        [SerializeField] private UnityEvent<bool> onFlagModified;
        
        private void OnEnable()
        {
            GlobalFlagsController.OnFlagAdded += OnFlagAdded;
            GlobalFlagsController.OnFlagRemoved += OnFlagRemoved;
        }

        private void OnDisable()
        {
            GlobalFlagsController.OnFlagAdded -= OnFlagAdded;
            GlobalFlagsController.OnFlagRemoved -= OnFlagRemoved;
        }

        private void OnFlagRemoved(string id)
        {
            if (flagId.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            {
                onFlagRemoved?.Invoke();
                onFlagModified?.Invoke(false);
            }
        }

        private void OnFlagAdded(string id)
        {
            if (flagId.Equals(id, StringComparison.InvariantCultureIgnoreCase))
            {
                onFlagAdded?.Invoke();
                onFlagModified?.Invoke(true);
            }
        }
    }
}