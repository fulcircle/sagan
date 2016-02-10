import THREE from '../vendor/three.min.js'
import { HeightMap } from './heightmap.js'
import { randomNumber } from './util.js'

// Abstract
export class Mesh {

    constructor() {
    }

    set position(pos) {
        this.mesh.position.set(pos.x, pos.y, pos.z);
    }

    get position() {
        return this.mesh.position;
    }
}

export class TriangleMesh extends Mesh {

    constructor() {
        super();
        this.geometry = new THREE.BufferGeometry();
        this.material = new THREE.MeshBasicMaterial( { color: 0xffffff });
        this.mesh = new THREE.Mesh(this.geometry, this.material);
    }

    set wireframe(bool) {
        this.mesh.material.wireframe = bool;
    }


    get boundingBox() {
        return this.geometry.boundingBox;
    }

    update(vertexPositions) {

        let vertices = new Float32Array(vertexPositions.length * 3);

        for ( var i = 0; i < vertexPositions.length; i++ )
        {
            vertices[ i*3 + 0 ] = vertexPositions[i][0];
            vertices[ i*3 + 1 ] = vertexPositions[i][1];
            vertices[ i*3 + 2 ] = vertexPositions[i][2];
        }

        this.geometry.addAttribute('position', new THREE.BufferAttribute(vertices, 3));
        this.geometry.attributes.position.needsUpdate = true;

        this.geometry.computeBoundingBox();

    }

    //get position() {
    //    this.geometry.computeBoundingBox();
    //    return this.geometry.boundingBox.center();
    //}
}

export class TerrainMesh extends TriangleMesh {

    constructor({width, height, heightMap=null, LOD=1, maxLOD=4}) {
        super();
        this.width = width;
        this.height = height;
        this.maxLOD = maxLOD;

        // Set heightMap before LOD so LOD calculates based on heightmap data
        this.heightMap = heightMap;

        this.LOD = LOD;


    }

    getHeight(x, y) {
        if (!this.heightMap) {
            return 0;
        } else {
            return 0;
            return this.heightMap.getHeight(x, y);
        }
    }


    setStride() {
        let tris = this.width*this.LOD;
        this.stride = this.width * ( 1 / tris );
    }

    set LOD(level) {
        this._lod = level;
        this.setStride();
        this.generate();
    }

    get LOD() {
        return this._lod;
    }

    generate() {

        let vertexPositions = [];

        for (var i = 0; i < this.width; i = i + this.stride) {
            for (var j = 0; j < this.height; j = j + this.stride) {
                //Create two triangles that will generate a square

                let i0 = i;
                let i1 = i + this.stride;

                let j0 = j;
                let j1 = j + this.stride;

                vertexPositions.push([i0, j0, this.getHeight(i0, j0)]);
                vertexPositions.push([i1, j0, this.getHeight(i1, j0)]);
                vertexPositions.push([i0, j1, this.getHeight(i0, j1)]);

                vertexPositions.push([i1, j0, this.getHeight(i1, j0)]);
                vertexPositions.push([i1, j1, this.getHeight(i1, j1)]);
                vertexPositions.push([i0, j1, this.getHeight(i0, j1)]);

            }
        }
        this.update(vertexPositions);
    }

}

export class QuadMesh extends TerrainMesh {

    constructor({width, height, heightMap, LOD=1, maxLOD=4}) {
        super({width, height, heightMap, LOD, maxLOD});
        this.children = [];
    }

}
