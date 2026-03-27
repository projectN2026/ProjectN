#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class CreateScriptFile
{
    #region
    // 이름 입력 완료 후 실제 파일 생성
    private class EndNameEdit : EndNameEditAction
    {
        public Func<string, string> FuncFromFilename;

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            string filename = Path.GetFileNameWithoutExtension(pathName);
            string content = FuncFromFilename?.Invoke(filename);

            File.WriteAllText(pathName, content);
            AssetDatabase.Refresh();

            // 생성된 파일 선택
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(pathName);
            Selection.activeObject = asset;
        }
    }

    private static void CreateFile(string defaultName, Func<String, String> funcFromFilename)
    {
        EndNameEdit endNameEdit = ScriptableObject.CreateInstance<EndNameEdit>();
        endNameEdit.FuncFromFilename = funcFromFilename;

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            instanceID: 0,
            endAction: endNameEdit,
            pathName: defaultName,
            icon: EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
            resourceFile: null
        );
    }
    #endregion

    private static string CreateContent(string[] usings = null, string head = "", string[] contents = null, bool contentsSpacing = false)
    {
        string str = "";

        if (usings != null)
        {
            foreach (string u in usings) str += "using " + u + ";\n";
            str += "\n";
        }

        if (head != null)
        {
            str += head + "\n{\n";
            if (contents != null)
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    str += "\t" + contents[i];
                    if (contentsSpacing && i < contents.Length - 1) str += "\n";
                }
            }
            str += "\n}\n";
        }

        return str;
    }

    [MenuItem("Assets/Create/CSharp Script - Interface", priority = -220)]
    public static void CreateInterface()
    {
        CreateFile("INewInterface.cs", (filename) =>
        CreateContent(
            usings: new[] { "System", "System.Collections.Generic" },
            head: $"public interface {filename}")
        );
    }

    [MenuItem("Assets/Create/CSharp Script - Enum", priority = -220)]
    public static void CreateEnum()
    {
        CreateFile("NewEnum.cs", (filename) =>
        CreateContent(
            head: $"public enum {filename}")
        );
    }

    [MenuItem("Assets/Create/CSharp Script - Class", priority = -220)]
    public static void CreateClass()
    {
        CreateFile("NewClass.cs", (filename) =>
        CreateContent(
            usings: new[] { "System", "System.Collections.Generic", "UnityEngine" },
            head: $"public class {filename}")
        );
    }

    [MenuItem("Assets/Create/CSharp Script - Struct", priority = -220)]
    public static void CreateStruct()
    {
        CreateFile("NewStruct.cs", (filename) =>
        CreateContent(
            usings: new[] { "System", "System.Collections.Generic" },
            head: $"public struct {filename}")
        );
    }
}

#endif