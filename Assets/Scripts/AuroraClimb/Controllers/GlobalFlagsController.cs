using System;
using System.Collections.Generic;
using UnityEngine;

namespace AuroraClimb.Controllers
{
    public class GlobalFlagsController : MonoBehaviour
    {
        public static event Action<string> OnFlagAdded; 
        public static event Action<string> OnFlagRemoved; 
        public static event Action OnFlagsModified; 

        private HashSet<string> flags = new ();

        public bool HasFlag(string id) => flags.Contains(id);

        public void AddFlag(string id)
        {
            if (HasFlag(id))
            {
                Debug.LogWarning($"TRYING TO ADD FLAG {id} BUT IT ALREADY EXIST");
                return;
            }

            flags.Add(id);
            OnFlagAdded?.Invoke(id);
            OnFlagsModified?.Invoke();
        }

        public void RemoveFlag(string id)
        {
            if (!HasFlag(id))
            {
                Debug.LogWarning($"TRYING TO REMOVE FLAG {id} BUT IT DOESNT EXIST");
                return;
            }

            flags.Remove(id);
            OnFlagRemoved?.Invoke(id);
            OnFlagsModified?.Invoke();
        }
    }
}