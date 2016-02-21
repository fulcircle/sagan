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
        private HeightMap _heightMap;

        public Quad(int LOD, float size, float error, HeightMap heightMap) : base("Quad") {
            this.size = size;
            this.LOD = LOD;

            this.error = error;

            this._heightMap = heightMap;
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

                    float offsetx = transform.position.x;
                    float offsetz = transform.position.z;

                    // We want the height at the offset that this Quad is from it's parent (the Terrain object)
                    float xh0 = x0 + offsetx;
                    float zh0 = z0 + offsetz;
                    float xh1 = x1 + offsetx;
                    float zh1 = z1 + offsetz;

                    // TODO: We're not sharing vertices, set triangles to share vertices
                    verts.Add(new Vector3(x1, GetHeight(xh1, zh0), z0)); // Shared vertex
                    verts.Add(new Vector3(x0, GetHeight(xh0, zh0), z0));
                    verts.Add(new Vector3(x0, GetHeight(xh0, zh1), z1)); // Shared vertex

                    verts.Add(new Vector3(x1, GetHeight(xh1, zh0), z0));
                    verts.Add(new Vector3(x0, GetHeight(xh0, zh1), z1));
                    verts.Add(new Vector3(x1, GetHeight(xh1, zh1), z1));

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

        float GetHeight(float x, float z) {
            return this._heightMap.GetHeight(x, z);
        }
    }
}
