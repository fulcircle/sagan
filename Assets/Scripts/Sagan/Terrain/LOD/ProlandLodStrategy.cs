using System;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Scripts.Sagan.Terrain {
    public class ProlandLodStrategy : LodStrategy {
        public float splitDistanceFactor = 1.2f;

        public ProlandLodStrategy(int terrainSize, HeightMap heightMap, int depth) : base(terrainSize, heightMap, depth) {}

        // Proland LOD implementation: http://proland.imag.fr/doc/proland-4.0/core/html/index.html
        protected override void Render(Camera cam, Quad quad, float scalingFactor = 1.0f) {
            var camPos = cam.transform.position;
            // TODO: If we spherify, the boundingbox test will be inaccurate since it will test against spherified quad bounding box
            // We want to test against non-transformed bounding box quad, so we'll have to somehow 'flatten' the quad again
            var closestPoint = quad.boundingBox.ClosestPoint(camPos);
            var distance = Vector3.Distance(closestPoint, camPos);

            if (quad.isLeaf) {
                quad.active = true;
            } else if (distance < splitDistanceFactor * quad.size) {
                quad.active = false;
                quad.children.ForEach(q => {
                    q.active = true;
                    this.Render(cam, q, scalingFactor);
                });
            // Is the root node
            } else if (quad.parent == null) {
                quad.active = true;
            }
        }

        public void Spherify() {
            throw new NotImplementedException();
        }
    }
}
