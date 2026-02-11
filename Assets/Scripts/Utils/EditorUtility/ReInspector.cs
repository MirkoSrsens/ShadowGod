#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Razorhead.Core.Editor
{
    public class ReInspector : EditorWindow, IDisposable
    {
        [NonSerialized]
        PropertyTree propertyTree;

        readonly List<InspectorProperty> breadcrumbs = new();

        InspectorProperty activeProperty;
        InspectorProperty queueNextProperty;
        InspectorProperty overrideProperty;
        InspectorProperty queueOverrideProperty;

        [SerializeField]
        UnityEngine.Object target;

        [SerializeField]
        string targetPath;

        [SerializeField]
        string targetPropertyPath;

        [SerializeField]
        Vector2 mainWindowScroll;

        [SerializeField]
        Vector2 hierarchyWindowScroll;

        static bool ShowHierarchy
        {
            get => EditorPrefs.GetBool("ReInspector_ShowHierarchy", false);
            set => EditorPrefs.SetBool("ReInspector_ShowHierarchy", value);
        }

        static int HierarchyWidth
        {
            get => EditorPrefs.GetInt("ReInspector_HierarchyWidth", 300);
            set => EditorPrefs.SetInt("ReInspector_HierarchyWidth", value);
        }

        bool IsHierarchiResizing { get; set; }

        readonly Dictionary<InspectorProperty, bool> hierarchyExpandedInfo = new();

        public static ReInspector Current { get; private set; }

        public static ReInspector Inspect(UnityEngine.Object target)
        {
            return Inspect(target, typeof(ReInspector));
        }

        public static ReInspector Inspect(UnityEngine.Object target, InspectorProperty property)
        {
            return Inspect(target, property, typeof(ReInspector));
        }

        public static ReInspector Inspect(UnityEngine.Object target, params Type[] desiredDockNextTo)
        {
            return Inspect(target, null, desiredDockNextTo);
        }

        public static ReInspector Inspect(UnityEngine.Object target, InspectorProperty property, params Type[] desiredDockNextTo)
        {
            var targetWindow = default(ReInspector);

            if (Current && Current.target == target)
            {
                targetWindow = Current;
            }

            if (!targetWindow)
            {
                targetWindow = Resources.FindObjectsOfTypeAll<ReInspector>().FirstOrDefault(x => x.target == target);
            }

            if (!targetWindow)
            {
                targetWindow = CreateWindow<ReInspector>("ReInspector", desiredDockNextTo);
            }

            targetWindow.SetTarget(target);

            targetWindow.SetActiveProperty(property);

            if (targetWindow.docked)
            {
                targetWindow.ShowTab();
            }

            targetWindow.Focus();

            return targetWindow;
        }

        private void SetTarget(UnityEngine.Object target)
        {
            if (this.target == target) return;

            if (this.target) Dispose();

            this.target = target;

            if (target)
            {
                targetPath = AssetDatabase.GetAssetPath(target);
                titleContent = new GUIContent(target.name);

                AssertPropertyTree();
            }
            else
            {
                targetPath = null;
                targetPropertyPath = null;
                queueNextProperty = null;
                titleContent = new GUIContent("(None)");
            }
        }

        void AssertPropertyTree()
        {
            if (target && propertyTree == null)
            {
                propertyTree = PropertyTree.Create(target);
                SetActiveProperty(propertyTree.RootProperty);
            }
        }

        private void SetActiveProperty(InspectorProperty property)
        {
            AssertPropertyTree();

            if (property == null || propertyTree == null || property == activeProperty) return;

            if (property.Tree != propertyTree)
            {
                property = propertyTree.GetPropertyAtPath(property.Path);
            }

            if (property == null) return;

            activeProperty = property;

            mainWindowScroll = default;

            activeProperty.State.Visible = true;
            activeProperty.State.Expanded = true;

            if (!breadcrumbs.Contains(property))
            {
                breadcrumbs.Clear();

                while (property != null)
                {
                    if (!IsPasstroughInHierarchy(property))
                    {
                        breadcrumbs.Add(property);
                    }

                    property = property.Parent;

                    if (property != null && property.TryGetTypedValueEntry<ICollection>() != null)
                    {
                        property = property.Parent;
                    }
                }

                breadcrumbs.Reverse();
            }

            if (breadcrumbs.Count > 0)
            {
                targetPropertyPath = breadcrumbs[^1].Path;
            }

            Repaint();
        }

        void OnEnable()
        {
            autoRepaintOnSceneChange = true;

            if (!string.IsNullOrEmpty(targetPath))
            {
                SetTarget(AssetDatabase.LoadAssetAtPath(targetPath, typeof(UnityEngine.Object)));

                if (target)
                {
                    titleContent = new GUIContent(target.name);

                    if (propertyTree != null && !string.IsNullOrEmpty(targetPropertyPath))
                    {
                        SetActiveProperty(propertyTree.GetPropertyAtPath(targetPropertyPath));
                    }
                }
            }
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!target) return;

            propertyTree?.Dispose();
            propertyTree = null;
            activeProperty = null;
            queueNextProperty = null;
            overrideProperty = null;

            target = null;
            hierarchyExpandedInfo.Clear();
        }

        void OnGUI()
        {
            minSize = new Vector2(ShowHierarchy ? 700 : 400, 200);

            if (target == null) return;

            AssertPropertyTree();

            if (Event.current.type == EventType.Layout)
            {
                overrideProperty = queueOverrideProperty;
                queueOverrideProperty = null;

                if (queueNextProperty != null)
                {
                    SetActiveProperty(queueNextProperty);
                    queueNextProperty = null;
                }
            }

            Current = this;

            try
            {
                DrawGui();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                Current = null;
            }

            Repaint();
        }

        void DrawGui()
        {
            EditorGUIUtility.labelWidth = (EditorGUIUtility.currentViewWidth - (ShowHierarchy ? HierarchyWidth : 0f)) / 2.5f;

            DrawToolbar();

            EditorGUILayout.BeginHorizontal();
            {
                if (propertyTree != null)
                {
                    DrawHierarchy();
                    DrawPropertyTree();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawHierarchy()
        {
            if (!ShowHierarchy) return;

            hierarchyWindowScroll = EditorGUILayout.BeginScrollView(hierarchyWindowScroll, GUILayout.Width(HierarchyWidth));

            var prop = propertyTree.RootProperty;
            var row = 0;

            while (prop != null)
            {
                if (!IsVisibleInHierarchy(prop))
                {
                    prop = prop.NextProperty(false, false);
                    continue;
                }

                if (prop.ValueEntry == null)
                {
                    prop = prop.NextProperty(true, false);
                    continue;
                }

                row++;
                var size = 18;

                var indent = CountParents(prop);
                var isCollapsed = IsExpandedInHierachy(prop);
                var childCount = CountVisibleChildrenInHierarchy(prop);
                var inBreadcrumbs = breadcrumbs.Contains(prop);

                var propRect = GUILayoutUtility.GetRect(0, size, GUILayout.ExpandWidth(true));

                var contentRect = propRect.Expand(-indent * 10, 0, 0, 0);

                if (row % 2 == 0)
                {
                    SirenixEditorGUI.DrawSolidRect(propRect, new Color(0, 0, 0, 0.06f));
                }

                if (activeProperty == prop)
                {
                    SirenixEditorGUI.DrawSolidRect(propRect, new Color(0.1f, 0.5f, 0.9f, 0.3f));
                }

                if (!IsHierarchiResizing && Event.current.IsMouseOver(propRect))
                {
                    SirenixEditorGUI.DrawSolidRect(propRect, new Color(1, 1, 1, 0.03f));

                    queueOverrideProperty = prop;
                    queueOverrideProperty.State.Visible = true;
                    queueOverrideProperty.State.Expanded = true;
                }

                var propType = prop.ValueEntry.TypeOfValue;

                var isList = typeof(ICollection).IsAssignableFrom(propType);
                var isNameless = prop.Parent == null || (prop.Parent.ValueEntry != null && typeof(ICollection).IsAssignableFrom(prop.Parent.ValueEntry.TypeOfValue));
                //var isReferenceType = prop.Info.GetAttribute<SerializeReference>() != null;

                var propName = prop.NiceName;

                if (isList) propName += $" [{prop.Children.Count}]";
                else if (isNameless) propName = propType.Name;

                //var propIcon = SdfIconType.Box;
                //if (isList) propIcon = SdfIconType.List;
                //else if (isReferenceType) propIcon = SdfIconType.Braces;

                GUIHelper.PushColor(new Color(1, 1, 1, 0.2f));
                if (childCount > 0 && SirenixEditorGUI.SDFIconButton(contentRect.MaxWidth(size).Expand(-2), !isCollapsed ? SdfIconType.CaretDownFill : SdfIconType.CaretRightFill, EditorStyles.label))
                {
                    hierarchyExpandedInfo[prop] = !isCollapsed;
                }
                GUIHelper.PopColor();

                if (Event.current.OnMouseDown(propRect, 0))
                {
                    queueNextProperty = prop;
                }

                //SirenixEditorGUI.SDFIconButton(contentRect.MaxWidth(size).AddX(size), propIcon, EditorStyles.label);
                GUI.Label(contentRect.Expand(-size * 1, 0, 0, 0), propName);

                prop = prop.NextProperty(!isCollapsed, false);
            }

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndScrollView();

            var hierarchyRect = GUILayoutUtility.GetLastRect();

            SirenixEditorGUI.DrawBorders(hierarchyRect, 0, 1, 0, 1, SirenixGUIStyles.BorderColor);

            var dragHandleRect = hierarchyRect.AlignRight(6).AddX(6);

            EditorGUIUtility.AddCursorRect(dragHandleRect, MouseCursor.ResizeHorizontal);

            HandleHierarchyRisizeEvents(dragHandleRect);

            GUILayout.Space(4);

            static int CountParents(InspectorProperty prop)
            {
                var parents = 0;

                while (prop.Parent != null)
                {
                    if (!IsPasstroughInHierarchy(prop.Parent))
                    {
                        parents++;
                    }

                    prop = prop.Parent;
                }

                return parents;
            }
        }

        private bool IsExpandedInHierachy(InspectorProperty prop)
        {
            return hierarchyExpandedInfo.TryGetValue(prop, out var isExpanded) && isExpanded;
        }

        private int CountVisibleChildrenInHierarchy(InspectorProperty prop)
        {
            var childCount = 0;

            for (int i = 0; i < prop.Children.Count; i++)
            {
                if (!IsPasstroughInHierarchy(prop.Children[i]) && IsVisibleInHierarchy(prop.Children[i]))
                {
                    childCount++;
                }
            }

            return childCount;
        }

        private static bool IsVisibleInHierarchy(InspectorProperty prop)
        {
            if (prop.ValueEntry == null) return true;

            var propType = prop.ValueEntry.TypeOfValue;

            if (propType.IsValueType) return false;

            var isParentList = prop.Parent != null &&
                prop.Parent.ValueEntry != null &&
                typeof(ICollection).IsAssignableFrom(prop.Parent.ValueEntry.TypeOfValue);

            var isReferenceType = prop.Info.GetAttribute<SerializeReference>() != null;

            if (isParentList && isReferenceType) return true;

            var isUnityObject = typeof(UnityEngine.Object).IsAssignableFrom(propType);

            var isInlineEditor = prop.Info.GetAttribute<InlineEditorAttribute>() != null;

            if (isUnityObject && isInlineEditor) return true;

            if (prop.Children.Count == 0) return false;

            return true;
        }

        private static bool IsPasstroughInHierarchy(InspectorProperty prop)
        {
            return prop.Info.PropertyType != PropertyType.Value;
        }

        private void HandleHierarchyRisizeEvents(Rect handleRect)
        {
            var e = Event.current;
            Vector2 mouse = e.mousePosition;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0 && handleRect.Contains(mouse))
                    {
                        IsHierarchiResizing = true;
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (IsHierarchiResizing)
                    {
                        // Compute new width based on mouse x position relative to rect.x
                        float newWidth = Mathf.Max(200, mouse.x);
                        HierarchyWidth = Mathf.RoundToInt(newWidth);
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if (IsHierarchiResizing && e.button == 0)
                    {
                        IsHierarchiResizing = false;
                        e.Use();
                    }
                    break;
                case EventType.Repaint:
                    break;
            }
        }

        private void DrawPropertyTree()
        {
            propertyTree.BeginDraw(true);

            mainWindowScroll = EditorGUILayout.BeginScrollView(mainWindowScroll);

            GUILayout.Space(4);

            try
            {
                if (overrideProperty == null)
                {
                    activeProperty?.Draw();
                }
                else
                {
                    overrideProperty.Draw();
                }
            }
            catch (ExitGUIException)
            {
                return;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                EditorGUILayout.EndScrollView();
            }

            propertyTree.EndDraw();
        }

        private void DrawToolbar()
        {
            var toolbarRect = SirenixEditorGUI.BeginHorizontalToolbar();

            if (SirenixEditorGUI.ToolbarButton(SdfIconType.List))
            {
                DoSelectTargetDropdown();
            }

            if (ShowHierarchy)
            {
                GUILayout.Space(HierarchyWidth - SirenixEditorGUI.currentDrawingToolbarHeight * 2);
            }

            if (SirenixEditorGUI.ToolbarButton(ShowHierarchy ? SdfIconType.ArrowBarLeft : SdfIconType.ArrowBarRight))
            {
                ShowHierarchy = !ShowHierarchy;
            }

            foreach (var property in breadcrumbs)
            {
                GUIHelper.PushColor(property == activeProperty ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f));

                if (property == propertyTree.RootProperty)
                {
                    if (SirenixEditorGUI.ToolbarButton(target.name)) queueNextProperty = property;
                }
                else
                {
                    if (SirenixEditorGUI.ToolbarButton(property.NiceName)) queueNextProperty = property;
                }

                GUIHelper.PopColor();

                if (property == activeProperty)
                {
                    var borderRect = GUILayoutUtility.GetLastRect().Expand(-1, 0).AlignBottom(3f);
                    SirenixEditorGUI.DrawSolidRect(borderRect, new Color(0.1f, 0.5f, 0.9f, 1f));
                }
            }

            GUILayout.FlexibleSpace();

            SirenixEditorGUI.DrawSolidRect(toolbarRect.MaxHeight(1), SirenixGUIStyles.BorderColor);

            SirenixEditorGUI.EndHorizontalToolbar();
        }

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            var asset = EditorUtility.InstanceIDToObject(instanceID);

            if (asset && asset.GetType().Assembly == typeof(ReInspector).Assembly)
            {
                return Inspect(asset);
            }

            return false;
        }

        class GenericSelectorItem
        {
            public string name;
            public object data;
            public Action<GenericSelectorItem> callback;

            public GenericSelectorItem(string name, object data, Action<GenericSelectorItem> callback)
            {
                this.name = name;
                this.data = data;
                this.callback = callback;
            }
        }

        void DoSelectTargetDropdown()
        {
            var type = target ? target.GetType() : typeof(ScriptableObject);

            var items = AssetDatabaseUtility.FindAllAssetsOfType(type);

            var menuItems = new List<GenericSelectorItem>
            {
                new($"★ Create New {type.Name}", type, (item) =>
                {
                    if (item.data is Type type)
                    {
                        item.data = null;

                        if (AssetDatabaseUtility.TryCreateAsset(type, type.Name, "Assets", out var newAsset))
                        {
                            Inspect(newAsset);
                        }
                    }
                })
            };

            menuItems.AddRange(items.Select<UnityEngine.Object, GenericSelectorItem>(x => new($"{x.name}", x, (x) => Inspect(x.data as UnityEngine.Object))));

            var selector = new GenericSelector<GenericSelectorItem>($"", false, x => x.name, menuItems);

            selector.SelectionTree.Config.DrawSearchToolbar = true;
            selector.SelectionConfirmed += selection =>
            {
                var selected = selection.FirstOrDefault();
                selected?.callback?.Invoke(selected);
            };
            selector.EnableSingleClickToSelect();
            var window = selector.ShowInPopup();
            window.OnClose += selector.SelectionTree.Selection.ConfirmSelection;
        }
    }

    [DrawerPriority(0, 0, 2001)] // This is in reference to NullableReferenceDrawer<T>
    internal sealed class ReInspectorPolymorphDrawer : OdinAttributeDrawer<SerializeReference>
    {
        protected override bool CanDrawAttributeProperty(InspectorProperty property)
        {
            return base.CanDrawAttributeProperty(property) &&
                property.ValueEntry != null &&
                property.ValueEntry.TypeOfValue.Assembly == typeof(ReInspector).Assembly &&
                !typeof(ICollection).IsAssignableFrom(property.ValueEntry.TypeOfValue) &&
                property.Tree.UnitySerializedObject != null &&
                property.Tree.UnitySerializedObject.targetObject;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var hasValue = Property.ValueEntry.WeakSmartValue != null;

            var btnRect = GUILayoutUtility.GetRect(0, 0);

            btnRect.x += btnRect.width - 42f;
            btnRect.y += 2f;
            btnRect.width = 22;
            btnRect.height = 18;

            if (hasValue && Event.current.OnMouseDown(btnRect, 0))
            {
                ReInspector.Inspect(Property.Tree.UnitySerializedObject.targetObject, Property);
                GUIUtility.ExitGUI();
            }

            CallNextDrawer(label);

            if (hasValue)
            {
                SirenixEditorGUI.DrawSolidRect(btnRect.Expand(2f, -1f), new Color(0.16f, 0.16f, 0.16f, 1f));
                SirenixEditorGUI.IconButton(btnRect, EditorIcons.SettingsCog);
            }
        }
    }

    [DrawerPriority(0, 0, 0.5)]
    internal sealed class ReInspectorContextDrawer : OdinValueDrawer<ScriptableObject>, IDefinesGenericMenuItems
    {
        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddSeparator(string.Empty);

            if (Property.ValueEntry.WeakSmartValue is ScriptableObject)
            {
                genericMenu.AddItem(new GUIContent("Open in ReInspector"), false, ReInspect, property);
            }
        }

        private void ReInspect(object property)
        {
            if (property is InspectorProperty inspectorProperty &&
                inspectorProperty.ValueEntry.WeakSmartValue is ScriptableObject target)
            {
                ReInspector.Inspect(target);
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var btnRect = GUILayoutUtility.GetRect(0, 0);

            btnRect.x += btnRect.width - 46f;
            btnRect.y += 2f;
            btnRect.width = 22;
            btnRect.height = 18;

            var value = Property.ValueEntry.WeakSmartValue as ScriptableObject;

            if (value && Event.current.OnMouseDown(btnRect, 0))
            {
                ReInspector.Inspect(value);
                GUIUtility.ExitGUI();
            }

            if (label == null)
            {
                Property.ValueEntry.WeakSmartValue = EditorGUILayout.ObjectField(value, Property.ValueEntry.TypeOfValue, false);
            }
            else
            {
                Property.ValueEntry.WeakSmartValue = EditorGUILayout.ObjectField(label, value, Property.ValueEntry.TypeOfValue, false);
            }

            if (value)
            {
                SirenixEditorGUI.DrawSolidRect(btnRect.Expand(2f, -1f), new Color(0.16f, 0.16f, 0.16f, 1f));
                SirenixEditorGUI.IconButton(btnRect, EditorIcons.SettingsCog);
            }

            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
        }
    }
}
#endif