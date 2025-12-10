using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace AuroraClimb.Items
{
    [CreateAssetMenu(menuName = "Item DB")]
    public class ItemDatabase : ScriptableObject
    {
        private static Dictionary<string, ItemDefinition> _dict = new();

        [SerializeField] private List<ItemDefinition> items;

        [RuntimeInitializeOnLoadMethod]
        public static void Initialize()
        {
            var db = Resources.Load<ItemDatabase>("ItemDB");
            
            foreach (var item in db.items)
                _dict.Add(item.ID, item);
        }

        public static ItemDefinition Get(string id, [CallerFilePath] string caller = "")
        {
            if (!_dict.TryGetValue(id, out var item))
                throw new Exception($"[{caller}] Trying to get {id} BUT COULD NOT FIND IN DATABASE");

            return item;
        }

        public static IEnumerable<ItemDefinition> All => _dict.Values;

#if UNITY_EDITOR
        [ContextMenu("Populate Data")]
        protected void Populate()
        {
            items.Clear();
            
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(ItemDefinition).FullName}");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                var data = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);
                items.Add(data);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}