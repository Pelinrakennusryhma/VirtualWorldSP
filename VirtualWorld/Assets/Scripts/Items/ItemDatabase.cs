using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Dev;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemDB", menuName = "Items/ItemDatabase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] string itemsRootPath;
        List<string> folderPaths;
        //[SerializeField] SerializableDictionary<string, Item> AllItems = new SerializableDictionary<string, Item>();
        //[SerializeField] Dictionary<string, Item> AllItems = new Dictionary<string, Item>();
        [SerializeField] List<Item> AllItems;

        public void Init()
        {
#if UNITY_EDITOR
            LoadFolders();

            foreach (string subFolder in folderPaths)
            {
                Debug.Log(subFolder);
            }

            LoadItems();
#endif
        }

#if UNITY_EDITOR
        void LoadFolders()
        {
            folderPaths = new List<string>();
            //folderPaths.Add(itemsRootPath);
            string[] folders = AssetDatabase.GetSubFolders(itemsRootPath);
            folderPaths.AddRange(folders);
            foreach (string folder in folders)
            {
                RecursiveLoadFolder(folder);
            }
        }

        void RecursiveLoadFolder(string folder)
        {
            var folders = AssetDatabase.GetSubFolders(folder);
            folderPaths.AddRange(folders);
            foreach (string fld in folders)
            {
                RecursiveLoadFolder(fld);
            }
        }

        void LoadItems()
        {
            AllItems = new List<Item>();
            
            foreach (string folder in folderPaths)
            {
                Item[] itemsInFolder = Utils.GetAtPath<Item>(folder);
                AddItems(itemsInFolder);
            }

        }

#endif

        public void AddItem(Item item)
        {
            //AllItems.Add(item.Id, item);
            AllItems.Add(item);
            //Save();
        }

        void AddItems(Item[] items)
        {
            AllItems.AddRange(items);
        }

        public void RemoveItem(Item item)
        {
            //AllItems.Remove(item.Id);
            AllItems.Remove(item);
            //Save();
        }

        //public void RemoveItem(string id, Item item)
        //{
        //    Item oldItem;
        //    AllItems.TryGetValue(id, out oldItem);

        //    if(oldItem != null)
        //    {
        //        // Just renaming an item
        //        if(oldItem == item)
        //        {
        //            AllItems.Remove(id);
        //        } 
        //        // else would likely be caused by duplicating an asset, in which case
        //        // we don't want to replace the old one
        //    }
        //    Save();

        //}

        void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.Refresh();
#endif
        }

        public Item GetItemById(string id)
        {
            return AllItems.Find(item => item.Id == id);
            //Debug.Log("AllItems: " + AllItems.Count);
            //Item item;
            //AllItems.TryGetValue(id, out item);
            //return item;
        }
    }
}

