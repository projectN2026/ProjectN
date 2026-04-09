using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
[CanEditMultipleObjects]
public class SerializeFieldEditor : Editor
{
    static Transform GetDescendantRecursive(Transform target, string name)
    {
        foreach (Transform child in target)
        {
            if (child.name == name)
                return child;

            var descendant = GetDescendantRecursive(child, name);
            if (descendant != null)
                return descendant;
        }
        return null;
    }

    static bool IsTargetComponent(MonoBehaviour target)
    {
        return target.GetType().IsDefined(typeof(InjectionByEditor), true);
    }
    static IEnumerable<FieldInfo> GetFields(MonoBehaviour target)
    {
        var type = target.GetType();

        // 현재 타입부터 MonoBehaviour 타입 전까지 검사
        while (type != typeof(MonoBehaviour))
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            var fields = type.GetFields(flags);

            foreach (var field in fields)
            {
                // 필드가 public 또는 SerializeField인지 확인
                var isPublic = field.IsPublic;
                var isSerializeField = Attribute.IsDefined(field, typeof(SerializeField));
                if (!isPublic && !isSerializeField)
                    continue;

                yield return field;
            }

            // 현재 타입의 상속 타입 검사
            type = type.BaseType;
        }
    }
    static void ClearFields(MonoBehaviour target)
    {
        foreach (var field in GetFields(target))
        {
            if (Attribute.IsDefined(field, typeof(ComponentField)) ||
                Attribute.IsDefined(field, typeof(DescendantField)))
                field.SetValue(target, null);
        }
        EditorUtility.SetDirty(target);
    }
    static void InjectFields(MonoBehaviour target)
    {
        if (!IsTargetComponent(target))
            return;

        foreach (var field in GetFields(target))
        {
            var fieldType = field.FieldType;

            // ComponentField injection
            if (Attribute.IsDefined(field, typeof(ComponentField)))
            {
                var component = target.GetComponent(fieldType);
                field.SetValue(target, component);

                if (component == null)
                    Debug.LogWarning($"{target.gameObject.name}에서 {fieldType.Name} 컴포넌트를 찾지 못함", target);
            }

            // DescendantField injection
            else
            if (Attribute.IsDefined(field, typeof(DescendantField)))
            {
                var nameTofind = field.Name;
                nameTofind = nameTofind.Replace("<", "").Replace(">k__BackingField", "");
                nameTofind = nameTofind.TrimStart('_');
                nameTofind = char.ToUpperInvariant(nameTofind[0]) + nameTofind[1..];

                var descendant = GetDescendantRecursive(target.transform, nameTofind);
                var descendantComponent = descendant?.GetComponent(fieldType);
                field.SetValue(target, descendantComponent);

                if (descendant == null)
                    Debug.LogWarning($"{target.gameObject.name}에서 {nameTofind} 이름의 자식을 찾지 못함", target);
                else
                if (descendantComponent == null)
                    Debug.LogWarning($"{target.gameObject.name}의 자식 {descendant.gameObject.name}에서 {fieldType.Name} 컴포넌트를 찾지 못함", target);
            }
        }
        EditorUtility.SetDirty(target);
    }

    #region Custom Editor
    const string HierarchyMenuPath = "GameObject/** Inject Fields From Hierarchy **";
    const string PrefabMenuPath = "Assets/** Inject Fields From Prefabs **";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetObject = (MonoBehaviour)target;

        if (!IsTargetComponent(targetObject))
            return;

        if (GUILayout.Button("Find Components"))
            InjectFields(targetObject);

        if (GUILayout.Button("Clear Components"))
            ClearFields(targetObject);
    }

    [MenuItem(HierarchyMenuPath, true)]
    static bool ValidateHierarchyMenu()
    {
        var objects = Selection.objects;
        var gameObjects = Selection.gameObjects;
        if (gameObjects == null || gameObjects.Length == 0 || gameObjects.Length != objects.Length)
            return false;

        foreach (var obj in gameObjects)
        {
            if (PrefabUtility.IsPartOfPrefabAsset(obj))
                return false;
        }
        return true;
    }
    [MenuItem(HierarchyMenuPath, false, -900)]
    static void HierarchyMenu()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (PrefabUtility.IsPartOfPrefabAsset(obj) ||
                PrefabUtility.IsPartOfNonAssetPrefabInstance(obj))
                continue;

            var targets = obj.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var target in targets)
            {
                if (PrefabUtility.IsPartOfNonAssetPrefabInstance(target))
                    continue;

                InjectFields(target);
            }
        }
    }

    [MenuItem(PrefabMenuPath, true)]
    static bool ValidatePrefabMenu()
    {
        var objects = Selection.objects;
        var gameObjects = Selection.gameObjects;
        if (gameObjects == null || gameObjects.Length == 0 || gameObjects.Length != objects.Length)
            return false;

        foreach (var obj in gameObjects)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
                return false;
        }
        return true;
    }
    [MenuItem(PrefabMenuPath, false, -900)]
    static void Prefabmenu()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (!PrefabUtility.IsPartOfPrefabAsset(obj))
                continue;

            var targets = obj.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var target in targets)
                InjectFields(target);

            PrefabUtility.SavePrefabAsset(obj);
        }
    }
    #endregion
}