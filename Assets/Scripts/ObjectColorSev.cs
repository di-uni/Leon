using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectColorSev : MonoBehaviour
{
    private Material[] skinMaterial;
    void OnSetColor(Color color)
	{

        // SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        // skinMaterial= new Material[renderers.Length];
        // for(int i=0; i<renderers.Length;i++){
        //     Debug.Log(renderers.Length);
        //   skinMaterial[i] = renderers[i].material;
        //   skinMaterial[i].color = color;  
        // }



		Material mt = new Material(GetComponent<SkinnedMeshRenderer>().sharedMaterial);
        //Material mt = new Material(GetComponent<SkinnedMeshRenderer>().sharedMaterial);
		mt.color = color;
		GetComponent<SkinnedMeshRenderer>().material = mt;
		Debug.Log(mt+"this is material2");
		Debug.Log(GetComponent<SkinnedMeshRenderer>().material.color+"this is color2");
	}

	void OnGetColor(ColorPicker picker)
	{
		picker.NotifyColor(GetComponent<SkinnedMeshRenderer>().material.color);
	}
}
