import THREE from '../vendor/three.min.js'
import Smooth from "../vendor/smooth.js";
import { randomNumber, initArray } from './util.js'

// Abstract
export class Mesh {

    constructor() {
        this.material = new THREE.MeshBasicMaterial( { color: 0xff0000 });
    }

    wireFrame(wire_color) {
        return new THREE.WireframeHelper(this.mesh, wire_color);
    }

    get position() {
        return new THREE.Vector3();
    }

}

export class TriangleMesh extends Mesh {

    constructor() {
        super();
        this.geometry = new THREE.BufferGeometry();
    }

    generateMesh(vertexPositions) {
        let vertices = new Float32Array(vertexPositions.length * 3);

        for ( var i = 0; i < vertexPositions.length; i++ )
        {
            vertices[ i*3 + 0 ] = vertexPositions[i][0];
            vertices[ i*3 + 1 ] = vertexPositions[i][1];
            vertices[ i*3 + 2 ] = vertexPositions[i][2];
        }

        this.geometry.addAttribute('position', new THREE.BufferAttribute(vertices, 3));

        this.geometry.computeBoundingBox();

        this.mesh = new THREE.Mesh(this.geometry, this.material);
    }

    get position() {
        this.geometry.computeBoundingBox();
        return this.geometry.boundingBox.center();
    }
}

export class TerrainMesh extends TriangleMesh {

    constructor(width, height) {
        super();
        this.width = width;
        this.height = height;
        this.maxLOD = 4;
        this.LOD = 1;
    }

    randomHeightMap() {
        let min_height = 0;
        let max_height = 2;

        this.heightMap = initArray(this.width*this.maxLOD, this.height*this.maxLOD);
        var random = randomNumber(min_height, max_height);

        for (var a = 0; a < this.heightMap.length; a++) {
            for (var b = 0; b < this.heightMap[a].length; b++) {
                this.heightMap[a][b] = random.next().value;
            }
        }
    }

    setStride() {
        let tris = this.width*this.LOD;
        this.stride = this.width * ( 1 / tris );
    }

    set LOD(level) {
        this._lod = level;
        this.setStride();
    }

    get LOD() {
        return this._lod;
    }

    getHeight(x, y) {
        var smoothedHeightMap = Smooth(this.heightMap, {
            scaleTo: this.width,
            method: Smooth.METHOD_CUBIC
        });

        var scaled = smoothedHeightMap(x)[y];
        return scaled;
    }

    randomTerrain() {
        let vertexPositions = [];

        this.randomHeightMap();

        for (var i = 0; i < this.width; i = i + this.stride) {
            for (var j = 0; j < this.height; j = j + this.stride) {
                //Create two triangles that will generate a square
                vertexPositions.push([i, j, this.getHeight(i, j)]);
                vertexPositions.push([i+this.stride, j, this.getHeight(i, j)]);
                vertexPositions.push([i, j+this.stride, this.getHeight(i, j)]);

                vertexPositions.push([i+this.stride, j, this.getHeight(i, j)]);
                vertexPositions.push([i+this.stride, j+this.stride, this.getHeight(i, j)]);
                vertexPositions.push([i, j+this.stride, this.getHeight(i, j)]);

            }
        }
        this.generateMesh(vertexPositions);
    }
}
