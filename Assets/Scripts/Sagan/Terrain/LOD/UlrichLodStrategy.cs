using System;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Sagan.Terrain {
    public class UlrichLodStrategy : AbstractLodStrategy {
        public UlrichLodStrategy(Sagan.Terrain.Terrain terrain) : base(terrain) {}

        public override Quad NewChildQuad(Quad parentQuad=null) {
            if (parentQuad == null) {
                return new UlrichQuad(0, this.terrain.size, this.terrain.size, this.terrain.heightMap);
            }
            else {
                var error = ((UlrichQuad) parentQuad).error;
                return new UlrichQuad(parentQuad.LOD + 1, parentQuad.size * 0.5f, error * 0.5f, this.terrain.heightMap);
            }
        }

        // TODO: Optimizations
        // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
        protected override void Render(Camera cam, Quad quad, float scalingFactor = 1.0f) {
            var error = ((UlrichQuad) quad).error;

            var camPos = cam.transform.position;
            var closestPoint = quad.boundingBox.ClosestPoint(camPos);
            var distance = Vector3.Distance(closestPoint, camPos);

            // Screen space error
            var rho = error / distance * scalingFactor;

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

        public override void Spherify() {
            throw new NotImplementedException();
        }
    }

    public class UlrichQuad : Quad {

        public float error { get; private set; }
        public UlrichQuad(int LOD, float size, float error, HeightMap heightMap, int subdivisions=12) :
            base(LOD, size, heightMap, subdivisions) {

            this.error = error;

        }
    }
}
