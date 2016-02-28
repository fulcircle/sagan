using System;
using System.Collections.Generic;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

public abstract class LodStrategy : ILodStrategy {
    public Quad rootQuad { get; private set; }

    public List<Quad> quads { get; private set; }

    public Sagan.Terrain.Terrain terrain;

    public LodStrategy(Sagan.Terrain.Terrain terrain) {
        this.terrain = terrain;
    }


    public void Precalculate() {
        quads = new List<Quad>();
        rootQuad = new Quad(0, this.terrain.terrainSize, this.terrain.terrainSize*0.5f, this.terrain.heightMap);
        GenerateQuadTree(rootQuad);
    }

    private void GenerateQuadTree(Quad parentQuad) {
        quads.Add(parentQuad);

        parentQuad.PreCalculate();

        // Start at LOD = 0, so add 1 to check if we've went to the propert depth
        if (parentQuad.LOD + 1 == this.terrain.depth) {
            parentQuad.isLeaf = true;
            return;
        }

        parentQuad.isLeaf = false;

        var currX = parentQuad.transform.localPosition.x;
        var currY = parentQuad.transform.localPosition.y;
        var currZ = parentQuad.transform.localPosition.z;

        var stride = parentQuad.size*0.5f;

        for (var i = 0; i < 4; i++) {
            var childQuad = new Quad(parentQuad.LOD + 1,
                parentQuad.size*0.5f,
                parentQuad.error*0.5f,
                this.terrain.heightMap);

            parentQuad.children.Add(childQuad);
            childQuad.parent = parentQuad;

            childQuad.transform.localPosition = new Vector3(currX, currY, currZ);

            GenerateQuadTree(childQuad);

            currX = currX + stride;
            if (currX - parentQuad.transform.localPosition.x >= parentQuad.size) {
                currX = parentQuad.transform.localPosition.x;
                currZ = currZ + stride;
            }
        }
    }

    protected abstract void Render(Camera cam, Quad quad, float scalingFactor = 1.0f);

    public abstract void Spherify(); 

    public void Create() {
        quads.ForEach(q => q.Create());
    }

    public void Update(Camera cam) {
        quads.ForEach(q => q.active = false);
        Render(cam, rootQuad, cam.perspectiveScalingFactor);
    }
}
