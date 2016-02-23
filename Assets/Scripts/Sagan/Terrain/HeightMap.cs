using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sagan.Terrain {

    public class HeightMap {

        private float[,] _heightMap;

        public int size {
            get;
            private set;
        }

        public HeightMap(int size) {
            this.size = size;
            this.InitMap();
        }

        private void InitMap() {
            this._heightMap = new float[this.size, this.size];

            for (int a = 0; a < this._heightMap.GetLength(0); a++) {
                for (int b = 0; b < this._heightMap.GetLength(1); b++) {
                    this._heightMap[a, b] = this.HeightMapFunc(a, b);
                }
            }
        }

        public float GetHeight(float x, float y) {

            // Interpolate the height value for x,y on the heightmap using bilinear interpolation
            // See: http://supercomputingblog.com/graphics/coding-bilinear-interpolation/

            int x1 = (int)Mathf.Floor(x);
            int x2 = (int)Mathf.Ceil(x);
            int y1 = (int)Mathf.Floor(y);
            int y2 = (int)Mathf.Ceil(y);

            if (x1 >= this.size || x2 >= this.size
            || y1 >= this.size || y2 >= this.size ) {
                var coords = string.Format("({0}, {1}), ({2}, {3})", x1, y2, x2, y2);
                throw new Exception("Trying to get an (x,y) value that's off the heightMap: " + coords);
            }

            // The four surrounding pixels to this pixel on the heightmap
            float q11 = this._heightMap[x1, y1];
            float q12 = this._heightMap[x1, y2];
            float q21 = this._heightMap[x2, y1];
            float q22 = this._heightMap[x2, y2];

            // The bilinear interpolation
            float r1, r2, height;

            if (x2 == x1) {
                r1 = q11;
                r2 = q12;
            } else {
                r1 = ((x2 - x)/(x2 - x1))*q11 + ((x - x1)/(x2 - x1))*q21;
                r2 = ((x2 - x)/(x2 - x1))*q12 + ((x - x1)/(x2 - x1))*q22;
            }

            if (y2 == y1) {
                height = r1;
            } else {
                height = ((y2 - y)/(y2 - y1))*r1 + ((y - y1)/(y2 - y1))*r2;
            }

            return height;


        }

        private float HeightMapFunc(int x, int y) {
            // Sin curve with some noise with a random number
            return Random.Range(0.0f, 0.3f);
        }
    }
}