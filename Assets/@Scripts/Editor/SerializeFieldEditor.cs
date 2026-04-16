using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    static bool ClearFields(MonoBehaviour target)
    {
        if (!IsTargetComponent(target))
            return false;

        bool isChanged = false;

        foreach (var field in GetFields(target))
        {
            if (Attribute.IsDefined(field, typeof(ComponentField)) ||
                Attribute.IsDefined(field, typeof(DescendantField)))
            {
                if (field.FieldType.IsValueType)
                    continue;

                if (field.GetValue(target) == null)
                    continue;

                field.SetValue(target, null);
                isChanged = true;
            }
        }

        if (isChanged)
            EditorUtility.SetDirty(target);
        
        return isChanged;
    }
    static bool InjectFields(MonoBehaviour target)
    {
        if (!IsTargetComponent(target))
            return false;

        bool isChanged = false;

        foreach (var field in GetFields(target))
        {
            if (InjectComponentField(target, field))
                isChanged = true;
            else
            if (InjectDescendantField(target, field))
                isChanged = true;
            else
            if (InjectDescendantsField(target, field))
                isChanged = true;
        }

        if (isChanged)
            EditorUtility.SetDirty(target);

        return isChanged;
    }

    static bool InjectComponentField(MonoBehaviour target, FieldInfo field)
    {
        if (!Attribute.IsDefined(field, typeof(ComponentField)))
            return false;

        var fieldType = field.FieldType;
        var component = target.GetComponent(fieldType);
        if (component == null)
        {
            Debug.LogWarning($"{target.gameObject.name}에서 {fieldType.Name} 컴포넌트를 찾지 못함", target);
            return false;
        }

        if (Equals(field.GetValue(target), component))
            return false;

        field.SetValue(target, component);
        return true;
    }
    static bool InjectDescendantField(MonoBehaviour target, FieldInfo field)
    {
        if (!Attribute.IsDefined(field, typeof(DescendantField)))
            return false;

        var fieldType = field.FieldType;
        var nameTofind = field.Name;
        nameTofind = nameTofind.Replace("<", "").Replace(">k__BackingField", "");
        nameTofind = nameTofind.TrimStart('_');
        nameTofind = char.ToUpperInvariant(nameTofind[0]) + nameTofind[1..];

        var descendant = GetDescendantRecursive(target.transform, nameTofind);
        var descendantComponent = descendant?.GetComponent(fieldType);

        if (descendant == null)
        {
            Debug.LogWarning($"{target.gameObject.name}에서 {nameTofind} 이름의 자식을 찾지 못함", target);
            return false;
        }
        else if (descendantComponent == null)
        {
            Debug.LogWarning($"{target.gameObject.name}의 자식 {descendant.gameObject.name}에서 {fieldType.Name} 컴포넌트를 찾지 못함", target);
            return false;
        }

        if (Equals(field.GetValue(target), descendantComponent))
            return false;

        field.SetValue(target, descendantComponent);
        return true;
    }
    static bool InjectDescendantsField(MonoBehaviour target, FieldInfo field)
    {
        if (!Attribute.IsDefined(field, typeof(DescendantsField)))
            return false;

        var fieldType = field.FieldType;
        if (!fieldType.IsArray)
        {
            Debug.LogWarning($"{nameof(DescendantsField)}가 붙은 필드 {field.Name}필드가 배열 타입이 아님.");
            return false;
        }

        var elementType = fieldType.GetElementType();
        var components = target?.GetComponentsInChildren(elementType);
        var components2 = Array.CreateInstance(elementType, components.Length);
        Array.Copy(components, components2, components.Length);

        if (components2 == null || components2.Length == 0)
        {
            Debug.LogWarning($"{target.gameObject.name}의 하위 오브젝트에서 {fieldType.Name} 컴포넌트를 하나도 찾지 못함", target);
            return false;
        }

        if (Equals(field.GetValue(target), components2))
            return false;

        field.SetValue(target, components2);
        return true;
    }

    static void InjectFromSceneObject(GameObject root)
    {
        var components = root.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var com in components)
        {
            if (PrefabUtility.IsPartOfNonAssetPrefabInstance(com.gameObject))
                continue;

            InjectFields(com);
        }
    } 
    static void InjectFromPrefabAsset(GameObject prefabRoot)
    {
        // 읽기 전용 프리팹 스킵
        var assetPath = AssetDatabase.GetAssetPath(prefabRoot);
        if (!AssetDatabase.IsOpenForEdit(assetPath))
        {
            Debug.Log($"읽기 전용 프리펩 스킵: {prefabRoot.name}", prefabRoot);
            return;
        }

        var components = prefabRoot.GetComponentsInChildren<MonoBehaviour>(true);
        if (components.Any(c => c == null))
        {
            Debug.Log($"missing script가 있는 프리펩 스킵: {prefabRoot.name}", prefabRoot);
            return;
        }

        bool isChanged = false;

        foreach (var com in components)
        {
            // 이 컴포넌트의 원본 프리팹이 prefabRoot인지 확인
            var source = PrefabUtility.GetCorrespondingObjectFromSource(com.gameObject);
            if (source != null && AssetDatabase.GetAssetPath(source) != assetPath)
            {
                Debug.Log($"프리펩 속의 프리펩 스킵: {prefabRoot.name}", prefabRoot);
                continue;
            }

            if (InjectFields(com))
                isChanged = true;
        }

        if (isChanged)
            PrefabUtility.SavePrefabAsset(prefabRoot);
    }

    static void InjectFromAllScenes()
    {
        // 현재 열린 씬들 처리 및 경로 기억
        var openScenePaths = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded == false)
                continue;

            openScenePaths.Add(scene.path);

            foreach (var root in scene.GetRootGameObjects())
                InjectFromSceneObject(root);

            if (scene.isDirty)
                EditorSceneManager.SaveScene(scene);
        }

        foreach (var buildSettingsScene in EditorBuildSettings.scenes)
        {
            // 빌드세팅에서 체크박스 꺼진 씬 스킵
            if (!buildSettingsScene.enabled)
                continue;
            // 위에서 처리한 현재 열린 씬 스킵
            if (openScenePaths.Contains(buildSettingsScene.path))
                continue;

            var scene = EditorSceneManager.OpenScene(buildSettingsScene.path, OpenSceneMode.Single);
            foreach (var root in scene.GetRootGameObjects())
                InjectFromSceneObject(root);

            EditorSceneManager.SaveScene(scene);
        }

        // 원래 열려있던 씬들 복구
        for (int i = 0; i < openScenePaths.Count; i++)
        {
            if (i == 0)
                EditorSceneManager.OpenScene(openScenePaths[i], OpenSceneMode.Single);
            else
                EditorSceneManager.OpenScene(openScenePaths[i], OpenSceneMode.Additive);
        }
    }
    static void InjectFromAllPrefabs()
    {
        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) 
                continue;

            InjectFromPrefabAsset(prefab);
        }
    }

    #region Custom Editor
    const string TopMenuPath = "Tools/** Auto Inject Fields **";
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

    [MenuItem(TopMenuPath)]
    static void TopMenu()
    {
        try
        {
            InjectFromAllScenes();
        }
        finally
        {
            InjectFromAllPrefabs();
        }
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
            InjectFromSceneObject(obj);
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
            InjectFromPrefabAsset(obj);
        }
    }
    #endregion
}