using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UMA;

public class MaterialValuesCopier : ScriptableObject
{

    [MenuItem("HDmyUMA/ SELECT A FOLDER THAT CONTAINS (OR WHOSE SUBFOLDERs CONTAINS) ALL THE OverlayDataAssets TO CONVERT")]
    static void DoStep1()
    {

        Shader LitShader = Shader.Find("HDRenderPipeline/Lit"); 

        List<Object> MaterialListToSelect = new List<Object>();

        OverlayDataAsset[] mnar = Selection.GetFiltered<OverlayDataAsset>(SelectionMode.DeepAssets);

        string LastUMAMaterialName = string.Empty;

        if (mnar.Length == 0)
        {
            Debug.LogError("REMEMBER TO SELECT A FOLDER CONTAINING ALL THE MATERIALS");
            return;
        }

        foreach (OverlayDataAsset mn in mnar)
        {
            Debug.Log("Init for "+mn.overlayName);

            //  Material newMaterial = new Material(Shader.Find("Standard (Specular setup)"));
            if (!(mn.material.material.shader.name.Equals("HDRenderPipeline/Lit")))
            {
                Material newMaterial = new Material(Shader.Find(mn.material.material.shader.name));

                newMaterial.SetTexture("_MainTex", mn.textureList[0]);
                newMaterial.SetTexture("_BumpMap", mn.textureList[1]);
                newMaterial.SetTexture("_SpecGlossMap", mn.textureList[2]);
                newMaterial.SetTexture("_OcclusionMap", mn.textureList[3]);


                // build it from the old name.
                // FIXME ***** NOTE THIS IS PRETTY TERRIBLE SINCE IT RELIES ON THE MANUALLY SET name FIELD ACTUALLY MATCHING THE PHYSICAL NAME *****
                // TODO: Fix This to cover all cases

                // Fix up names to be coshesive.

                mn.overlayName = mn.name;


                // now make a name based on that.
                string newName = AssetDatabase.GetAssetPath(mn).ToString().Replace(mn.name + ".asset", "") + mn.overlayName + ".mat";

               // and make a file

                AssetDatabase.CreateAsset(newMaterial, newName);

                // and save it

                AssetDatabase.SaveAssets();

                // and take a reference

                Object thing = AssetDatabase.LoadMainAssetAtPath(newName);

                // and add that to a list that we can re-select later.

                MaterialListToSelect.Add(thing);


            }

            // TODO: Perhaps make an array to parse for uniques to service if we find generally more than one by this point.  So far just one in our work.
            if (!(LastUMAMaterialName.Equals(mn.material.name)))
            {
                LastUMAMaterialName = mn.material.name;
            }

        }

        // actually select the items.
        Selection.objects = MaterialListToSelect.ToArray();


        // MaterialListToSelect now contains all our newly minted material objects.

        //    do this bit automatically now since one-click is request

        EditorApplication.ExecuteMenuItem("Edit/Render Pipeline/Upgrade Selected Materials to High Definition Materials");

        // if all is well with the LastUMAMaterialName then select that in the Project view

        if (!(LastUMAMaterialName.Equals(string.Empty)))
        {
            string pathMade = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(LastUMAMaterialName)[0]);
            if (!(pathMade.Equals(string.Empty)))
            {
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(pathMade);
                Debug.Log("Updated Selection to " + pathMade + "'s active object");
            }
        }
    
    
        UMAMaterial ms1 = (UMAMaterial)Selection.activeObject;

        string innerPath = AssetDatabase.GetAssetPath(ms1);

        UMAMaterial OBM_Master_Overlay = AssetDatabase.LoadAssetAtPath<UMAMaterial>(innerPath);

        // we have to remember to change the shader on the material

        if (!(OBM_Master_Overlay.material.shader.name.Equals("HDRenderPipeline/Lit")))
        {
            OBM_Master_Overlay.material.shader = LitShader;
        }


        if (!(OBM_Master_Overlay.channels[0].materialPropertyName.Equals("_BaseColorMap")))
        {
            OBM_Master_Overlay.channels[0].materialPropertyName = "_BaseColorMap";

            OBM_Master_Overlay.channels[1].materialPropertyName = "_NormalMap";

            OBM_Master_Overlay.channels[2].materialPropertyName = "_MaskMap";

            OBM_Master_Overlay.channels[2].sourceTextureName = "";

            OBM_Master_Overlay.channels[3].materialPropertyName = "_SpecularColorMap";

            Debug.Log("Updated OBM_Master_Overlay");

            AssetDatabase.SaveAssets();

            Debug.Log("TOGGLED "+ innerPath +" to HDRP");

        }
        else
        {

            Debug.Log("Did Not Toggle " + innerPath + " as already HDRP");
        }

        //OBM_Master_Overlay.channels[0].materialPropertyName = "_MainTex";

        //   OBM_Master_Overlay.channels[1].materialPropertyName = "_BumpMap";

        //   OBM_Master_Overlay.channels[2].materialPropertyName = "_SpecGlossMap";

        //   OBM_Master_Overlay.channels[2].sourceTextureName = "_OcclusionMap";

        //   OBM_Master_Overlay.channels[3].materialPropertyName = "_OcclusionMap";

        //   Debug.Log("Reverted " + innerPath + " to NON HDRP"); }

        //  }

       
        // From all the selected materials loop through them and set the Overlay to the HD version

        Selection.objects = MaterialListToSelect.ToArray();

        Material[] ms = Selection.GetFiltered<Material>(SelectionMode.DeepAssets);

        foreach (Material mn in ms)
        {

            if (mn.shader.name.Equals("HDRenderPipeline/Lit"))
            {
                //   Material newMaterial = new Material(Shader.Find("Standard (Specular setup)"));
                string assetPath = AssetDatabase.GetAssetPath(mn).Replace(".mat", ".asset");
                OverlayDataAsset da = AssetDatabase.LoadAssetAtPath<OverlayDataAsset>(assetPath);

                Debug.Log("Processing " + assetPath);

                da.textureList[0] = mn.GetTexture("_BaseColorMap");

                da.textureList[1] = mn.GetTexture("_NormalMap");

                da.textureList[2] = mn.GetTexture("_MaskMap");

                da.textureList[3] = mn.GetTexture("_SpecularColorMap");
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("Processing Complete ");
    }
}