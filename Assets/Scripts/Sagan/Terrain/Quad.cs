using System;
using Sagan.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Terrain {

    public class Quad : SaganMesh {

        public float size { get; private set; }

        public bool isLeaf = false;

        public List<Quad> children = new List<Quad>();

        public Quad parent;

        public List<Vector3> verts {
            get;
            private set;
        }

        public List<int> tris {
            get;
            private set;
        }

        public float subdivisions { get; private set; }

        private float _step;

        public int LOD { get; private set; }

        private float _maxHeight = 20;
        private HeightMap _heightMap;

        public HeightMap quadHeightMap { get; private set; }

        public Quad(int LOD, float size, HeightMap heightMap, int subdivisions=12) : base(name: "Quad") {
            this.size = size;

            this.LOD = LOD;

            this._heightMap = heightMap;

            this.subdivisions = subdivisions;

            this._step = size / this.subdivisions;
        }

        public virtual Texture2D GetHeightMapTexture() {
            var textureSize = Mathf.CeilToInt(this.size) + 1;
            var texture = new Texture2D(textureSize, textureSize, TextureFormat.Alpha8, false);
            var pixelBuffer = new byte[textureSize * textureSize];

            // Get the minimum coordinates of quad
            var min = this.transform.localPosition;

            // Get the min and max coordinates of the corresponding terrain patch
            var min_x = Mathf.FloorToInt(this.transform.localPosition.x);
            var max_x = textureSize;

            var min_z = Mathf.FloorToInt(this.transform.localPosition.z);
            var max_z = textureSize;

            // Iterate over each coordinate in the terrain patch and assign to pixelbuffer
            for (var x = min_x; x < max_x; x++) {
                for (var z = min_z; z < max_z; z++) {
                    var heightVal = this._heightMap.heightArray[x, z];
                    var result = Mathf.Lerp(0, 255, Mathf.InverseLerp(this._heightMap.minHeightValue, this._heightMap.maxHeightValue, heightVal));
                    result = Mathf.RoundToInt(result);
                    pixelBuffer[(x*max_x) + z] = (byte)result;
                }
            }
            texture.LoadRawTextureData(pixelBuffer);
            texture.Apply();
            return texture;
        }


        public void PreCalculate() {
            float start_time = Time.time;

            this.verts = new List<Vector3>();
            this.tris = new List<int>();

            int offset = 0;
            for (float x = 0; x <= size - this._step; x = x + this._step) {
                for (float z = 0; z <= size - this._step; z = z + this._step) {

                    //Create two triangles that will generate a square

                    float x0 = x;
                    float x1 = x + this._step;

                    float z0 = z;
                    float z1 = z + this._step;

                    // We want the height at the offset that this Quad is from it's parent (the Terrain object)
                    float xh0 = x0 + transform.localPosition.x;
                    float zh0 = z0 + transform.localPosition.z;
                    float xh1 = x1 + transform.localPosition.x;
                    float zh1 = z1 + transform.localPosition.z;

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

            float diff = Time.time - start_time;
        }

        /// <summary>
        /// Creates the mesh with the calculated vertices.
        /// Assumes PreCalculate() was already called.
        /// </summary>
        public void Create() {
            var mesh = new Mesh();
            mesh.vertices = this.verts.ToArray();
            mesh.triangles = this.tris.ToArray();
            this.mesh = mesh;
        }

        float GetHeight(float x, float z) {
            return 0;
            return this._heightMap.GetHeight(x, z);
        }
    }
}
