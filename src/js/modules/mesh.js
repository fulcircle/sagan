import THREE from '../vendor/three.min.js'
import { randomNumber, initArray } from './util.js'

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

    update(vertexPositions) {


        let vertices = new Float32Array(vertexPositions.length * 3);

        for ( var i = 0; i < vertexPositions.length; i++ )
        {
            vertices[ i*3 + 0 ] = vertexPositions[i][0];
            vertices[ i*3 + 1 ] = vertexPositions[i][1];
            vertices[ i*3 + 2 ] = vertexPositions[i][2];
        }

        this.geometry.addAttribute('position', new THREE.BufferAttribute(vertices, 3));
        this.mesh.geometry.attributes.position.needsUpdate = true;

        this.geometry.computeBoundingBox();
    }

    //get position() {
    //    this.geometry.computeBoundingBox();
    //    return this.geometry.boundingBox.center();
    //}
}

export class TerrainMesh extends TriangleMesh {

    constructor({width, height, LOD=1, maxLOD=4}) {
        super();
        this.width = width+1;
        this.height = height+1;
        this.maxLOD = maxLOD;
        this.LOD = LOD;
    }

    setHeightMap(func) {
        this.heightMap = initArray(this.width*this.maxLOD, this.height*this.maxLOD);

        for (var a = 0; a < this.heightMap.length; a++) {
            for (var b = 0; b < this.heightMap[a].length; b++) {
                this.heightMap[a][b] = func(a, b);
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
        this.generateTerrain();
    }

    get LOD() {
        return this._lod;
    }

    // TODO: Think of optimizations
    getHeight(x, y) {
        // Get the ratio of the heightmap dimensions to the mesh dimensions
        let xratio = this.heightMap.length * ( 1 / this.width );
        let yratio = this.heightMap[0].length * (1 / this.height );

        // Get the height coordinates on the heightmap that correspond to the x,y vertex on the mesh
        // Note: this won't necessarily be an integer, which is why we perform a bilinear interpolation next
        let heightmapx = x*xratio;
        let heightmapy = y*yratio;

        // Interpolate the height value for x,y on the heightmap using bilinear interpolation
        // See: http://supercomputingblog.com/graphics/coding-bilinear-interpolation/

        let x1 = Math.floor(heightmapx);
        let x2 = Math.ceil(heightmapx);
        let y1 = Math.floor(heightmapy);
        let y2 = Math.ceil(heightmapy);

        // The four surrounding pixels to this pixel on the heightmap
        let q11 = this.heightMap[x1][y1];
        let q12 = this.heightMap[x1][y2];
        let q21 = this.heightMap[x2][y1];
        let q22 = this.heightMap[x2][y2];

        // The bilinear interpolation
        let r1, r2, height = null;
        if (x2 == x1) {
            r1 = q11;
            r2 = q12;
        } else {
            r1 = ((x2 - heightmapx)/(x2 - x1))*q11 + ((heightmapx - x1)/(x2 - x1))*q21;
            r2 = ((x2 - heightmapx)/(x2 - x1))*q12 + ((heightmapx - x1)/(x2 - x1))*q22;
        }

        if (y2 == y1) {
            height = r1;
        } else {
            height = ((y2 - heightmapy)/(y2 - y1))*r1 + ((heightmapy - y1)/(y2 - y1))*r2;
        }


        return height;
    }

    generateTerrain() {
        if (!this.heightMap) {
            return;
        }

        let vertexPositions = [];
        for (var i = 0; i < this.width-1; i = i + this.stride) {
            for (var j = 0; j < this.height-1; j = j + this.stride) {
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
