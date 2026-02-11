using System;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Razorhead.Core.Editor;
#endif

namespace Razorhead.Core
{
    public class CreateAssetButtonAttribute : Attribute
    {

    }

#if UNITY_EDITOR
    [DrawerPriority(1000)]
    public sealed class CreateAssetButtonDrawer : OdinAttributeDrawer<CreateAssetButtonAttribute>
    {
        public override bool CanDrawTypeFilter(Type type)
        {
            return base.CanDrawTypeFilter(type) && !typeof(ICollection).IsAssignableFrom(type);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (typeof(ScriptableObject).IsAssignableFrom(Property.ValueEntry.BaseValueType))
            {
                var obj = Property.ValueEntry.WeakSmartValue as ScriptableObject;

                EditorGUILayout.BeginHorizontal();

                Property.ValueEntry.WeakSmartValue = EditorGUILayout.ObjectField(
                    label ?? new GUIContent(Property.NiceName),
                    obj,
                    Property.ValueEntry.BaseValueType,
                    false
                );

                var btnRect = GUILayoutUtility.GetRect(36, 18, GUILayout.MaxWidth(36));
                btnRect.y += 2;

                if (GUI.Button(btnRect, "New"))
                {
                    var dir = "Assets";
                    var name = $"New {Property.ValueEntry.BaseValueType.Name}";

                    var parentObject = Property.Tree.WeakTargets?.FirstOrDefault();

                    if (parentObject is UnityEngine.Object target && AssetDatabaseUtility.TryGetAssetPath(target, out var targetPath))
                    {
                        dir = Path.GetDirectoryName(targetPath);
                        name = $"{target.name}_{Property.NiceName.Replace(' ', '_')}";
                    }

                    if (AssetDatabaseUtility.TryCreateAsset(Property.ValueEntry.BaseValueType, name, dir, out var newAsset))
                    {
                        Property.ValueEntry.WeakSmartValue = newAsset;
                    }

                    GUIUtility.ExitGUI();
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            if (Property.GetAttribute<InlineEditorAttribute>() != null)
            {
                CallNextDrawer(label);
            }
        }
    }
#endif
}
