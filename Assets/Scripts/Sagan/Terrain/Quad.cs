using Sagan.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Terrain {

    public class Quad : SaganMesh {

        public float size { get; private set; }
        public bool isLeaf = false;
        public List<Quad> children = new List<Quad>();

        public int LOD {
            get {
                return _lod;
            }
            private set {
                this._lod = value;
                this._stride = this.size / (float)value;
            }
        }

        public float error {get; private set;}

        private int _lod;
        private float _stride;
        private float _maxHeight = 20;

        public Quad(int LOD, float size, float error) : base() {
            this.size = size;
            this.LOD = LOD;

            this.error = error;
        }


        public void Generate() {
            float start_time = Time.time;

            List<Vector3> verts = new List<Vector3>();
            List<Vector3> uv = new List<Vector3>();
            List<int> tris = new List<int>();

            Mesh mesh = new Mesh();

            int offset = 0;
            for (float x = 0; x <= size - this._stride; x = x + this._stride) {
                for (float z = 0; z <= size - this._stride; z = z + this._stride) {

                    //Create two triangles that will generate a square

                    float x0 = x;
                    float x1 = x + this._stride;

                    float z0 = z;
                    float z1 = z + this._stride;

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

            this.mesh = mesh;

            float diff = Time.time - start_time;
        }

        float GetHeight(float x_coor, float z_coor) {
            return 0;
            float y_coor =
            Mathf.Min(
                    0,
                    _maxHeight - Vector2.Distance(Vector2.zero, new Vector2(x_coor, z_coor)
                    )
            );
            return y_coor;
        }
    }
}
