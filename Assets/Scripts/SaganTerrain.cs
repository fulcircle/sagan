using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaganTerrain : MonoBehaviour {

    public int terrainSize = 10;

    private List<QuadMesh> quads = new List<QuadMesh>();

    private QuadMesh rootQuad;

    void Start() {
        transform.position = new Vector3(0,0,0);

        GameObject rootQuadGO = new GameObject("RootQuad");

        rootQuad = rootQuadGO.AddComponent<QuadMesh>();
        rootQuadGO.AddComponent<MeshFilter>();
        rootQuadGO.AddComponent<MeshRenderer>();

        rootQuad.width = terrainSize;
        rootQuad.LOD = 2;
    }

}
