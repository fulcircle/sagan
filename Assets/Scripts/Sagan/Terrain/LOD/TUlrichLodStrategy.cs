using System;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Scripts.Sagan.Terrain {
    public class TUlrichLodStrategy : LodStrategy {
        public TUlrichLodStrategy(int terrainSize, HeightMap heightMap, int depth) : base(terrainSize, heightMap, depth) {}

        // TODO: Optimizations
        // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
        protected override void Render(Camera cam, Quad quad, float scalingFactor = 1.0f) {
            var camPos = cam.transform.position;
            var closestPoint = quad.boundingBox.ClosestPoint(camPos);
            var distance = Vector3.Distance(closestPoint, camPos);

            // Screen space error
            var rho = quad.error/distance*scalingFactor;

            // Largest allowable screen error
            float tau = 120;

            if (quad.isLeaf || rho <= tau) {
                quad.active = true;
            }
            else {
                // TODO: When we implement excluding of whole subbranches, we'll have to cull all quads inside that branch
                quad.children.ForEach(q => Render(cam, q, scalingFactor));
            }
        }

        public void Spherify() {
            throw new NotImplementedException();
        }
    }
}
