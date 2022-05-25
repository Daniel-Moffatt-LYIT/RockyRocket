using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxBackground : MonoBehaviour
{
    [SerializeField]
    MeshRenderer[] meshRenderers;
    [SerializeField]
    float speedOfOffset;
    Material[] materials;
    private void Start()
    {
        materials = new Material[meshRenderers.Length];
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            materials[i] = meshRenderers[i].material;
        }
    }
    private void Update()
    {
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i].mainTextureOffset = new Vector2(0, materials[i].mainTextureOffset.y + (i + 1) * Time.deltaTime*speedOfOffset);
        }
    }
}
