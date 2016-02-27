using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Sagan.Terrain;
using Sagan.Framework;
using UnityEngine;
using Camera = Sagan.Framework.Camera;


namespace Scripts.Sagan.Terrain
{
    public class TUlrichLodStrategy : ILodStrategy
    {

        public Quad rootQuad { get; private set; }

        public int terrainSize { get; private set; }

        public HeightMap heightMap { get; private set; }

        public List<Quad> quads { get; private set; }

        private readonly int depth;

        public TUlrichLodStrategy(int terrainSize, HeightMap heightMap, int depth)
        {
            this.heightMap = heightMap;
            this.depth = depth;
            this.terrainSize = terrainSize;
        }


        public void Precalculate()
        {
            this.quads = new List<Quad>();
            this.rootQuad = new Quad(0, this.terrainSize, this.terrainSize * 0.5f, this.heightMap);
            this.GenerateQuadTree(this.rootQuad);
        }

        void GenerateQuadTree(Quad parentQuad) {


            this.quads.Add(parentQuad);

            parentQuad.PreCalculate();

            // Start at LOD = 0, so add 1 to check if we've went to the propert depth
            if (parentQuad.LOD + 1 == this.depth) {
                parentQuad.isLeaf = true;
                return;
            }

            parentQuad.isLeaf = false;

            var currX = parentQuad.transform.localPosition.x;
            var currY = parentQuad.transform.localPosition.y;
            var currZ = parentQuad.transform.localPosition.z;

            var stride = parentQuad.size * 0.5f;

            for (int i=0; i < 4; i++) {
                var childQuad = new Quad(parentQuad.LOD + 1,
                        parentQuad.size * 0.5f,
                        parentQuad.error * 0.5f,
                        this.heightMap);

                parentQuad.children.Add(childQuad);

                childQuad.transform.localPosition = new Vector3(currX, currY, currZ);

                this.GenerateQuadTree(childQuad);

                currX = currX + stride;
                if ((currX - parentQuad.transform.localPosition.x) >= parentQuad.size) {
                    currX = parentQuad.transform.localPosition.x;
                    currZ = currZ + stride;
                }


            }
        }

        // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
        // TODO: Optimizations
        // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
        void ChunkedLOD(Camera cam, Quad quad, float scalingFactor = 1.0f) {
            var camPos = cam.transform.position;
            Vector3 closestPoint = quad.boundingBox.ClosestPoint(camPos);
            float distance = Vector3.Distance(closestPoint, camPos);

            // Screen space error
            float rho = (quad.error/distance)*scalingFactor;

            // Largest allowable screen error
            float tau = 120;

            if (quad.isLeaf || rho <= tau) {
                quad.active = true;
            }
            else {
                // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
                quad.children.ForEach(q => this.ChunkedLOD(cam, q, scalingFactor));
            }
        }

        public void Spherify()
        {
            throw new NotImplementedException();
        }

        public void Create() {
            this.quads.ForEach(q => q.Create());
        }

        public void Update(Camera cam)
        {
            this.quads.ForEach(q => q.active = false);
            this.ChunkedLOD(cam, this.rootQuad, cam.perspectiveScalingFactor);
        }
    }
}
