using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
using UnityEngine;

public class AutoInitPreprocessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    internal static readonly string packageName = "com.textclub.sdk";

    internal static readonly string initProductionAsset = $"Packages/{packageName}/Runtime/Init/init.js";

    internal static readonly string initMockAsset = $"Packages/{packageName}/Runtime/Init/initMock.js";

    internal static readonly string assetDestination = $"Assets/{packageName}/init.jspre";

    public void OnPreprocessBuild(BuildReport _)
    {
        RemoveInitScriptIfExists();

#if TEXTCLUB_SDK
        CopyInitScript(initProductionAsset);
#elif TEXTCLUB_SDK_MOCK
        CopyInitScript(initMockAsset);
#endif
    }

    internal static void CopyInitScript(string from)
    {
        try
        {
            Debug.Log($"Copying init script from {from}");
            Directory.CreateDirectory(Path.GetDirectoryName(assetDestination));
            if (AssetDatabase.CopyAsset(from, assetDestination))
            {
                AssetDatabase.Refresh();
            }
            else
            {
                throw new System.Exception("Copy operation failed.");
            }
        }
        catch (System.Exception e)
        {
            throw new System.Exception($"Failed to copy asset at {from} to {assetDestination}. {from} is needed to use Textclub SDK.", e);
        }
    }

    internal static void RemoveInitScriptIfExists()
    {
        if (AssetDatabase.DeleteAsset(assetDestination))
        {
            AssetDatabase.Refresh();
        }
    }
}
