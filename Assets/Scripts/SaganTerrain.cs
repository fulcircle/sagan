using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaganTerrain : MonoBehaviour {

    public int terrainSize = 10;

    protected List<QuadMesh> quads = new List<QuadMesh>();

    protected QuadMesh rootQuad;

    void Start() {
        transform.position = new Vector3(0,0,0);

        rootQuad = GetComponent<GameObject>().AddComponent<QuadMesh>();

        rootQuad.width = 10;
        rootQuad.LOD = 2;
    }
}
