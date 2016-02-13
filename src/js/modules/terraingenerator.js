import THREE from '../vendor/three.min.js'
import { randomNumber } from './util.js'
import { HeightMap } from './heightmap.js'
import { QuadMesh } from './mesh.js'

export const HeightMapFuncs = {
    SinRandom:  {
        random: randomNumber(0,2),
        func: (x, y) => {
            // TODO: Can't use 'this' keyword here.. wtf?
            return 3*Math.sin(0.1*x) + 3*Math.sin(0.1*y) + HeightMapFuncs.SinRandom.random.next().value;
        }
    }
};

export class TerrainGenerator {

    constructor(engine, terrainHeight, terrainWidth, heightMapFunc) {

        this.engine = engine;
        this.terrainHeight = terrainHeight;
        this.terrainWidth = terrainWidth;

        // Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra data point beyond edge of quad
        this.heightMap = new HeightMap(terrainHeight + 1, terrainWidth + 1, heightMapFunc);

        this.rootQuad = new QuadMesh({
            height: terrainHeight,
            width: terrainWidth,
            heightMap: this.heightMap,
            error: terrainWidth
        });

        this.rootQuad.wireframe = true;
        // TODO: Not ideal, have to set position and LOD afterwards and not as a parameter during instantiation
        this.rootQuad.position = new THREE.Vector3();
        this.rootQuad.LOD = 1;
    }

    generate() {
        this.engine.add(this.rootQuad);
        this.generateQuadTree(this.rootQuad);
        this.engine.addQuadTree(this.rootQuad);
    }

    // TODO: Convert into breadth-first generation of tree
    generateQuadTree(parentQuad) {
        if (parentQuad.LOD > 6) {
            return;
        }
        let currX = parentQuad.position.x;
        let currY = parentQuad.position.y;
        let currZ = parentQuad.position.z;

        let xstride = parentQuad.width * 0.5;
        let ystride = parentQuad.height * 0.5;

        let LOD = parentQuad.LOD + 1;

        for (var i = 0; i < 4; i++) {
            let quad = new QuadMesh({
                width: parentQuad.width * 0.5,
                height: parentQuad.height * 0.5,
                LOD: LOD,
                heightMap: parentQuad.heightMap,
                error: parentQuad.error * 0.5
            });

            quad.wireframe = true;

            parentQuad.children.push(quad);
            this.engine.add(quad);

            // TODO: Not ideal, have to set position and LOD afterwards and not as a parameter during instantiation
            quad.position = new THREE.Vector3(currX, currY, currZ);
            quad.LOD = LOD;

            currX = currX + xstride;
            if ((currX - parentQuad.position.x) >= parentQuad.width) {
                currX = parentQuad.position.x;
                currY = currY + ystride;
            }
            this.generateQuadTree(quad);
        }
    }

}

