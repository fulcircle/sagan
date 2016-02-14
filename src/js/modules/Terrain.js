import THREE from '../vendor/three.min.js'
import { randomNumber } from './Util.js'
import { HeightMap } from './HeightMap.js'
import { QuadMesh } from './Mesh.js'

export const HeightMapFuncs = {
    SinRandom:  {
        random: randomNumber(0,2),
        func: (x, y) => {
            // TODO: Can't use 'this' keyword here.. wtf?
            return 3*Math.sin(0.1*x) + 3*Math.sin(0.1*y) + HeightMapFuncs.SinRandom.random.next().value;
        }
    }
};

export class Terrain {

    constructor(terrainHeight, terrainWidth, heightMapFunc) {

        // Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra data point beyond edge of quad
        this.heightMap = new HeightMap(terrainHeight + 1, terrainWidth + 1, heightMapFunc);

        this.mesh = new THREE.Group();

        this.quads = [];

        this.rootQuad = new QuadMesh({
            height: terrainHeight,
            width: terrainWidth,
            position: new THREE.Vector3(),
            heightMap: this.heightMap,
            LOD: 1,
            error: terrainWidth
        });

        this.generateQuadTree(this.rootQuad);

    }

    // TODO: Convert into breadth-first generation of tree
    generateQuadTree(parentQuad) {

        parentQuad.wireframe = true;
        parentQuad.visible = false;
        parentQuad.generate();

        // Add this to our list of quads
        this.quads.push(parentQuad);
        // Add this as a child of a group in the scene, so transformations to the group will apply to this quad as well
        this.mesh.add(parentQuad.mesh);


        if (parentQuad.LOD > 6) {
            parentQuad._isLeaf = true;
            return;
        }

        let currX = parentQuad.position.x;
        let currY = parentQuad.position.y;
        let currZ = parentQuad.position.z;

        let xstride = parentQuad.width * 0.5;
        let ystride = parentQuad.height * 0.5;

        for (var i = 0; i < 4; i++) {
            let quad = new QuadMesh({
                width: parentQuad.width * 0.5,
                height: parentQuad.height * 0.5,
                position: new THREE.Vector3(currX, currY, currZ),
                LOD: parentQuad.LOD + 1,
                heightMap: parentQuad.heightMap,
                error: parentQuad.error * 0.5
            });

            quad.wireframe = true;

            parentQuad.children.push(quad);

            currX = currX + xstride;
            if ((currX - parentQuad.position.x) >= parentQuad.width) {
                currX = parentQuad.position.x;
                currY = currY + ystride;
            }
            this.generateQuadTree(quad);
        }
    }


    get centroid() {
        return getCentroid(this.mesh);
    }

    draw(pos, perspectiveScalingFactor=1) {
        for (let q of this.quads) {
            q.visible = false;
        }
        this.chunkedLOD(this.rootQuad, pos, perspectiveScalingFactor);
    }

    // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
    // TODO: Optimizations
    // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
    chunkedLOD(quad, pos, scalingFactor=1) {

        // TODO: Need to get distance to nearest face, not centroid
        // We're translating from local mesh coordinates (in this Quad Mesh), to the global coordinates
        let centroid = new THREE.Vector3().copy(quad.centroid);
        quad.mesh.localToWorld(centroid);
        // Check the camera distance from world coordinates of this quad to the specified position
        let distance = pos.distanceTo(centroid);

        // Screen space error
        let rho = (quad.error / distance ) * scalingFactor;

        // distance = 0 so screenspace error should be 0
        if (!isFinite(rho)) {
            rho = 0;
        }
        // Largest allowable screen error
        let tau = 45;

        if (quad._isLeaf || rho <= tau) {
            quad.visible = true;
        } else {
            // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
            for (let c of quad.children) {
                this.chunkedLOD(c, pos, scalingFactor);
            }
        }
    }

}

