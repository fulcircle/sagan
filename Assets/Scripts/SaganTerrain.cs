using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaganTerrain : MonoBehaviour {

    public int terrainSize = 10;

    private List<QuadMesh> quads = new List<QuadMesh>();

    private GameObject rootQuadGO;

    void Start() {
        transform.position = new Vector3(0,0,0);

        rootQuadGO = newQuad();
        var rootQuad = rootQuadGO.GetComponent<QuadMesh>();
        rootQuad.size = terrainSize;
        rootQuad.LOD = 1;
        rootQuad.error = terrainSize;

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
            var quadGO = newQuad();
            var quadMesh = quadGO.GetComponent<QuadMesh>();

            quadMesh.size = parentQuad.size * 0.5f;
            quadMesh.LOD = parentQuad.LOD + 1;
            quadMesh.error = parentQuad.error * 0.5f;

            quadMesh.children.Add(quadMesh);

            quadMesh.transform.position = new Vector3(currX, currY, currZ);

            currX = currX + stride;
            if ((currX - parentQuad.transform.position.x) >= parentQuad.size) {
                currX = parentQuad.transform.position.x;
                currZ = currZ + stride;
            }
            GenerateQuadTree(quadGO);
        }
    }

    void Update() {
        foreach (var quadGO in GameObject.FindGameObjectsWithTag("Quad")) {
            quadGO.SetActive(false);
        }

        this.chunkedLOD(rootQuadGO);
    }

    GameObject newQuad() {
        var quadGO = new GameObject();
        quadGO.tag = "Quad";
        quadGO.AddComponent<MeshFilter>();
        quadGO.AddComponent<MeshRenderer>();

        return quadGO;
    }

    // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
    // TODO: Optimizations
    // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
    void chunkedLOD(GameObject quadGO, float scalingFactor=1) {

        // TODO: Need to get distance to nearest face, not centroid
        // TODO: Get closest point from camera, not origin
        QuadMesh quadMesh = quadGO.GetComponent<QuadMesh>();
        Vector3 closestPoint = quadGO.GetComponent<MeshFilter>().mesh.bounds.ClosestPoint(new Vector3(0,0,0));
        float distance = Vector3.Distance(closestPoint, new Vector3(0,0,0));

        // Screen space error
        float rho = (quadMesh.error / distance ) * scalingFactor;

        // distance = 0 so screenspace error should be 0
//        if (!isFinite(rho)) {
//            rho = 0;
//        }
        // Largest allowable screen error
        float tau = 45;

        if (quadMesh.isLeaf || rho <= tau) {
            quadGO.SetActive(true);
        } else {
            // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
            foreach (QuadMesh quad in quadMesh.children) {
                this.chunkedLOD(quad.gameObject, scalingFactor);
            }
        }
    }

}
