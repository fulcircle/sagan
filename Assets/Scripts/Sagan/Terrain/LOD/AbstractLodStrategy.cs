using System;
using System.Collections.Generic;
using Sagan.Terrain;
using UnityEngine;
using Camera = Sagan.Framework.Camera;

public abstract class AbstractLodStrategy : ILodStrategy {
    public Quad rootQuad { get; private set; }

    public List<Quad> quads { get; private set; }

    public Sagan.Terrain.Terrain terrain;

    public Shader shader { get; protected set; }

    public virtual string shaderName {
        get { return "Sagan/Base"; }
    }

    public AbstractLodStrategy(Sagan.Terrain.Terrain terrain) {
        this.terrain = terrain;
        this.shader = Shader.Find(this.shaderName);
    }


    public virtual void Precalculate() {
        quads = new List<Quad>();
        rootQuad = this.NewChildQuad();
        GenerateQuadTree(rootQuad);
    }

    public virtual Quad NewChildQuad(Quad parentQuad=null) {
        if (parentQuad == null) {
            return new Quad(0, this.terrain.size, this.terrain.heightMap);
        }
        else {
            return new Quad(parentQuad.LOD + 1, parentQuad.size * 0.5f, this.terrain.heightMap);
        }

    }


    protected virtual void GenerateQuadTree(Quad parentQuad) {
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
            var childQuad = this.NewChildQuad(parentQuad);

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

    protected virtual void SetMaterial(Quad q) {
        q.material = new Material(this.shader);
        q.material.SetFloat("_MaxHeight", this.terrain.heightMap.maxHeightValue);
        q.material.SetFloat("_MinHeight", this.terrain.heightMap.minHeightValue);
    }

    protected abstract void Render(Camera cam, Quad quad, float scalingFactor = 1.0f);

    public abstract void Spherify();

    public void Create() {
        quads.ForEach(q => {
            q.Create();
            this.SetMaterial(q);
        });
    }

    public void Update(Camera cam) {
        quads.ForEach(q => q.active = false);
        Render(cam, rootQuad, cam.perspectiveScalingFactor);
    }
}
