using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace Unity.Entities.Editor
{
    internal class ScriptTemplates
    {
        public const string TemplatesRoot = "Assets/ScriptTemplates";

        [MenuItem("Assets/ECS/Runtime Component Type", priority = 10)]
        public static void CreateRuntimeComponentType()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"{TemplatesRoot}/RunTimeComponent.txt",
                "NewComponent.cs");
        }

        [MenuItem("Assets/ECS/Authoring Component Type", priority = 11)]
        public static void CreateAuthoringComponentType()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"{TemplatesRoot}/AuthoringComponent.txt",
                "NewComponent.cs");
        }

        [MenuItem("Assets/ECS/System", priority = 12)]
        public static void CreateSystem()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"{TemplatesRoot}/System.txt",
                "NewSystem.cs");
        }

        [MenuItem("Assets/ECS/System_IJobChunk", priority = 13)]
        public static void System_IJobChunk()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
                $"{TemplatesRoot}/IJobChunk.txt",
                "NewSystem.cs");
        }

    }
}
