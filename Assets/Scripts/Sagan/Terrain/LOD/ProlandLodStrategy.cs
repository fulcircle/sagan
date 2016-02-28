using System;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

namespace Sagan.Terrain {
    public class ProlandLodStrategy : LodStrategy {
        public float splitDistanceFactor = 1.2f;

        public ProlandLodStrategy(Sagan.Terrain.Terrain terrain) : base(terrain) {}

        // Proland LOD implementation: http://proland.imag.fr/doc/proland-4.0/core/html/index.html
        protected override void Render(Camera cam, Quad quad, float scalingFactor = 1.0f) {

            // TODO: If we spherify, the transform test will be inaccurate since it will test against spherified quad coordinates
            // We want to test against non-transformed quad coordinates, so we'll have to store non-transformed quad coordinates

            // Get the cam's position in the terrain's local space
            var camPos = terrain.transform.InverseTransformPoint(cam.transform.position);

            // Taxicab distance calculations
            var xdiff = camPos.x - quad.transform.localPosition.x;
            var ydiff = camPos.y - quad.transform.localPosition.y;
            var zdiff = camPos.z - quad.transform.localPosition.z;

            var xdistance = Mathf.Min(Mathf.Abs(xdiff), Mathf.Abs(xdiff - quad.size));
            var ydistance = Mathf.Abs(ydiff);
            var zdistance = Mathf.Min(Mathf.Abs(zdiff), Mathf.Abs(zdiff - quad.size));

            var distance = Mathf.Max(xdistance, ydistance, zdistance);

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

        public override void Spherify() {
            throw new NotImplementedException();
        }
    }
}
