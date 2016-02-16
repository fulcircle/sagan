import THREE from '../vendor/three.min.js'
import { HeightMap } from './HeightMap.js'
import { randomNumber, getCentroid } from './Util.js'

// Abstract
export class Mesh {

    set position(pos) {
        this.mesh.position.copy(pos);
    }

    get position() {
        return this.mesh.position;
    }

    get visible() {
        return this.mesh.visible;
    }

    set visible(bool) {
        this.mesh.visible = bool;
    }

}

export class TriangleMesh extends Mesh {

    constructor() {
        super();
        this.geometry = new THREE.BufferGeometry();
        this.material = new THREE.MeshBasicMaterial( { color: 0xffffff });
        this.mesh = new THREE.Mesh(this.geometry, this.material);
        this.centroid = new THREE.Vector3();
    }

    set wireframe(bool) {
        this.mesh.material.wireframe = bool;
    }


    get boundingBox() {
        return this.geometry.boundingBox;
    }

    update(vertexPositions) {

        let vertices = new Float32Array(vertexPositions.length * 3);

        for (var i = 0; i < vertexPositions.length; i++) {
            vertices[i * 3 + 0] = vertexPositions[i][0];
            vertices[i * 3 + 1] = vertexPositions[i][1];
            vertices[i * 3 + 2] = vertexPositions[i][2];
        }

        // We can't change the number of vertices on a geometry, so we create a new geometry
        let updatedGeom = new THREE.BufferGeometry();
        updatedGeom.addAttribute('position', new THREE.BufferAttribute(vertices, 3));

        // Kill old geometry and update our reference to new one
        this.geometry.dispose();
        this.geometry = updatedGeom;

        this.mesh.geometry = updatedGeom;
        this.mesh.needsUpdate = true;

        this.recomputeBoundingBox();

    }

    recomputeBoundingBox() {
        this.geometry.computeBoundingBox();
        this.centroid = getCentroid(this.geometry.boundingBox);
    }
}

export class TerrainMesh extends TriangleMesh {

    constructor({width, height, heightMap=null, LOD=1}) {
        super();
        this.width = width;
        this.height = height;

        // Set heightMap before LOD so LOD calculates based on heightmap data
        this.heightMap = heightMap;

        this.LOD = LOD;
    }

    getHeight(x, y) {
        return 0;
        if (!this.heightMap) {
            return 0;
        } else {
            return this.heightMap.getHeight(x, y);
        }
    }

    set LOD(level) {
        this._lod = level;
        this.stride = (this.width / this.LOD);
    }

    get LOD() {
        return this._lod;
    }

    generate() {

        let vertexPositions = [];

        for (var i = 0; i <= this.width - this.stride; i = i + this.stride) {
            for (var j = 0; j <= this.height - this.stride; j = j + this.stride) {
                //Create two triangles that will generate a square

                let i0 = i;
                let i1 = i + this.stride;

                let j0 = j;
                let j1 = j + this.stride;

                let ih0 = i0 + this.mesh.position.x;
                let ih1 = i1 + this.mesh.position.x;

                let jh0 = j0 + this.mesh.position.y;
                let jh1 = j1 + this.mesh.position.y;

                vertexPositions.push([i0, j0, this.getHeight(ih0, jh0)]);
                vertexPositions.push([i1, j0, this.getHeight(ih1, jh0)]);
                vertexPositions.push([i0, j1, this.getHeight(ih0, jh1)]);

                vertexPositions.push([i1, j0, this.getHeight(ih1, jh0)]);
                vertexPositions.push([i1, j1, this.getHeight(ih1, jh1)]);
                vertexPositions.push([i0, j1, this.getHeight(ih0, jh1)]);

            }
        }

        this.update(vertexPositions);
    }
}

export class QuadMesh extends TerrainMesh {

    constructor({width, height, heightMap, LOD=1, error=0}) {
        super({width, height, heightMap, LOD});
        this.error = error;
        this.children = [];
    }

    spherify(radius) {
        let vertices = this.mesh.geometry.attributes.position.array;
        for (var i = 0; i < vertices.length; i+=3) {
            let x = vertices[i + 0];
            let y = vertices[i + 1];
            let z = vertices[i + 2];

            let v = new THREE.Vector3(x, y, z);
            this.mesh.localToWorld(v);
            v.normalize();

            vertices[i + 0] = v.x;
            vertices[i + 1] = v.y;
            vertices[i + 2] = v.z;

        }

        this.mesh.geometry.needsUpdate = true;
        this.recomputeBoundingBox();
    }

}
