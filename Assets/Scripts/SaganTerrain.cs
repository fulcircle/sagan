using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaganTerrain : MonoBehaviour {

    public int terrainSize = 10;

    private List<QuadMesh> quads = new List<QuadMesh>();

    private QuadMesh rootQuad;

    void Start() {
        transform.position = new Vector3(0,0,0);

        var rootQuadGO = newQuad(terrainSize, 1);

        GenerateQuadTree(rootQuadGO);
    }

    void GenerateQuadTree(GameObject parentQuadGO) {

        var parentQuad = parentQuadGO.GetComponent<QuadMesh>();

        if (parentQuad.LOD > 6) {
            return;
        }

        var currX = parentQuad.transform.position.x;
        var currY = parentQuad.transform.position.y;
        var currZ = parentQuad.transform.position.z;

        var stride = parentQuad.size * 0.5f;

        for (int i=0; i < 4; i++) {
            var quadGO = newQuad(parentQuad.size * 0.5f, parentQuad.LOD + 1);
            var quadMesh = quadGO.GetComponent<QuadMesh>();

            quadMesh.transform.position = new Vector3(currX, currY, currZ);

            currX = currX + stride;
            if ((currX - parentQuad.transform.position.x) >= parentQuad.size) {
                currX = parentQuad.transform.position.x;
                currZ = currZ + stride;
            }
            GenerateQuadTree(quadGO);
        }
    }

    GameObject newQuad(float size, int LOD) {
        var quadGO = new GameObject();
        quadGO.AddComponent<MeshFilter>();
        quadGO.AddComponent<MeshRenderer>();

        var quadMesh = quadGO.AddComponent<QuadMesh>();

        quadMesh.size = size;
        quadMesh.LOD = LOD;

        return quadGO;
    }

}
