using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadMesh : MonoBehaviour {

	public int width = 10;
    public int height = 10;

    public float spacing = 1f;
	public float maxHeight = 3f;

    private float stride;
    private int lod = 1;

    public int LOD {
        set {
            this.lod = value;
            Debug.Log("Setting lod to " + this.lod);
            this.stride = this.width / (float)value;
            Debug.Log("Stride is " + this.stride);
            Generate();
        }

        get {
            return lod;
        }
    }

    void Start() {
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

	void Generate() {
		float start_time = Time.time;

        List<Vector3> verts = new List<Vector3>();
        List<Vector3> uv = new List<Vector3>();
        List<int> tris = new List<int>();

        Mesh mesh = new Mesh();

        int offset = 0;
        for (float x = 0; x <= width - stride; x = x + stride) {
            for (float z = 0; z <= height - stride; z = z + stride) {

                //Create two triangles that will generate a square

                float x0 = x;
                float x1 = x + stride;

                float z0 = z;
                float z1 = z + stride;

                // TODO: We're not sharing vertices, set triangles to share vertices
                Vector3 worldPoint0 = transform.TransformPoint(x0, 0, z0);
                Vector3 worldPoint1 = transform.TransformPoint(x1, 0, z1);

                verts.Add(new Vector3(x1, GetHeight(worldPoint1.x, worldPoint0.z), z0)); // Shared vertex
                verts.Add(new Vector3(x0, GetHeight(worldPoint0.x, worldPoint0.z), z0));
                verts.Add(new Vector3(x0, GetHeight(worldPoint0.x, worldPoint1.z), z1)); // Shared vertex

                verts.Add(new Vector3(x1, GetHeight(worldPoint1.x, worldPoint0.z), z0));
                verts.Add(new Vector3(x0, GetHeight(worldPoint0.x, worldPoint1.z), z1));
                verts.Add(new Vector3(x1, GetHeight(worldPoint1.x, worldPoint1.z), z1));

                // Add first triangle
                tris.AddRange(new int[] {offset, offset + 1, offset + 2});

                // Add second triangle
                tris.AddRange(new int[] {offset + 3, offset + 4, offset + 5});
                offset += 6;
            }
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;


		float diff = Time.time - start_time;
		Debug.Log("ProceduralTerrain was generated in " + diff + " seconds!");
	}

	float GetHeight(float x_coor, float z_coor) {
        return 0;
		float y_coor =
			Mathf.Min(
				0,
				maxHeight - Vector2.Distance(Vector2.zero, new Vector2(x_coor, z_coor)
			)
		);
		return y_coor;
	}
}