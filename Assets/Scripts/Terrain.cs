using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Terrain : MonoBehaviour {
    
    public List<Vector3> vertics = new List<Vector3>();
    
    public List<int> triangles = new List<int>();
    
    public List<Vector2> uv = new List<Vector2>();
    
    private Mesh mesh;
    
    public Terrain(int terrainSize) {
        mesh = new Mesh();
        
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
