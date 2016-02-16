using UnityEngine;
using System.Collections;

public class Terrain {
    
    public List<Vector3> vertices = new List<Vector3>();
    
    public List<int> triangles = new List<init>();
    
    public List<Vector2> uv = new List<Vector2>();
    
    private Mesh mesh;
    
    public Terrain(int terrainSize, ) {
        mesh = GetComponent<MeshFilter>().mesh;
        
        
    }
}
