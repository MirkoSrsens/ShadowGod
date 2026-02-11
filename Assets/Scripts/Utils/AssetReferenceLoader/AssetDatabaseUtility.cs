#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Razorhead.Core.Editor
{
    public static class AssetDatabaseUtility
    {
        public static IEnumerable<T> FindAllAssetsOfType<T>(string query = "", Type filterType = null) where T : UnityEngine.Object
        {
            var type = filterType ?? typeof(T);

            return AssetDatabase.FindAssets(string.Format("t:{0} {1}", type.Name, query))
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .SelectMany(x => AssetDatabase.LoadAllAssetsAtPath(x))
                .Where(x => x && x is T)
                .Cast<T>();
        }

        public static IEnumerable<UnityEngine.Object> FindAllAssetsOfType(Type type, string query = "")
        {
            return AssetDatabase.FindAssets(string.Format("t:{0} {1}", type.Name, query))
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .SelectMany(x => AssetDatabase.LoadAllAssetsAtPath(x))
                .Where(x => x)
                .Cast<UnityEngine.Object>();
        }

        public static T FindAssetOfType<T>(string query = "", Type filterType = null) where T : UnityEngine.Object
        {
            return FindAllAssetsOfType<T>(query, filterType).FirstOrDefault();
        }

        public static T FindAssetWithName<T>(string name, Type filterType = null) where T : UnityEngine.Object
        {
            var type = filterType ?? typeof(T);

            return AssetDatabase.FindAssets(string.Format("t:{0} {1}", type.Name, name))
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .SelectMany(x => AssetDatabase.LoadAllAssetsAtPath(x))
                .Where(x => x && x is T)
                .Cast<T>()
                .Where(x => x.name == name)
                .FirstOrDefault();
        }

        public static List<T> GetPrefabsWithComponent<T>(string filter = "") where T : Component
        {
            return AssetDatabase.FindAssets(string.Format("t:Prefab {0}", filter))
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x))
                .Select(x => x.GetComponent<T>())
                .Where(x => x != null)
                .ToList();
        }

        public static List<GameObject> GetPrefabsWithComponentInChildren<T>(string filter = "", bool includeInactive = false) where T : Component
        {
            return AssetDatabase.FindAssets(string.Format("t:Prefab {0}", filter))
                .Select(x => AssetDatabase.GUIDToAssetPath(x))
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(x))
                .Where(x => x.GetComponentInChildren<T>(includeInactive) != null)
                .ToList();
        }

        public static void SaveAsset<T>(T asset) where T : UnityEngine.Object
        {
            EditorUtility.SetDirty(asset);
        }

        public static IEnumerable<T> SaveAssets<T>(this IEnumerable<T> assets) where T : UnityEngine.Object
        {
            foreach (var asset in assets) EditorUtility.SetDirty(asset);
            return assets;
        }

        public static void DestroyAllSubassetsAtPath(UnityEngine.Object mainAsset)
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(mainAsset));

            Undo.SetCurrentGroupName($"Destroy All Subassets of ({mainAsset.name})");
            var group = Undo.GetCurrentGroup();

            foreach (var asset in assets)
            {
                if (!AssetDatabase.IsMainAsset(asset)) Undo.DestroyObjectImmediate(asset);
            }

            Undo.CollapseUndoOperations(group);
        }

        public static bool TryCreateAsset<T>(string name, string directory, out T asset) where T : ScriptableObject
        {
            if (TryCreateAsset(typeof(T), name, directory, out var genericAsset) && genericAsset is T tAsset)
            {
                asset = tAsset;
                return true;
            }
            else
            {
                asset = null;
                return false;
            }
        }

        public static bool TryCreateAsset(Type type, string name, string directory, out ScriptableObject asset)
        {
            var path = EditorUtility.SaveFilePanelInProject(
                $"Create new {type.Name}",
                name,
                "asset",
                "Please enter a file name to save the asset to",
                directory
            );

            if (string.IsNullOrEmpty(path))
            {
                asset = null;
                return false;
            }

            asset = ScriptableObject.CreateInstance(type);
            asset.name = Path.GetFileNameWithoutExtension(path);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            return true;
        }

        public static bool TryGetAssetPath(UnityEngine.Object obj, out string path)
        {
            path = null;
            if (obj == null) return false;
            path = AssetDatabase.GetAssetPath(obj);
            return !string.IsNullOrEmpty(path);
        }
    }
}
#endif