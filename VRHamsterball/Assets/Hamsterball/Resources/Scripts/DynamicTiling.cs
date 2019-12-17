using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTiling : MonoBehaviour
{
    [SerializeField] private float tileX = 1;
    [SerializeField] private float tileY = 1;
    Mesh mesh;
    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mesh = GetComponent<MeshFilter>().mesh;

    }

    void Update()
    {
        if (transform.localScale.z > transform.localScale.x)
        {
            mat.mainTextureScale = new Vector2((mesh.bounds.size.z *
            transform.localScale.z)/10 * tileX, (mesh.bounds.size.y * transform.localScale.y)/10 * tileY);
        }
        else {
            mat.mainTextureScale = new Vector2((mesh.bounds.size.x *
            transform.localScale.x)/10 * tileX, (mesh.bounds.size.y * transform.localScale.y)/10 * tileY);
        }
    }
}
