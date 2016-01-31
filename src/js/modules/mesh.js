import THREE from '../vendor/three.min.js'

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

export class TriangleStripMesh extends Mesh {

    constructor() {
        super();
        this.geometry = new THREE.BufferGeometry();
    }

    addStrip(vertexPositions) {
        let vertices = new Float32Array(vertexPositions.length * 3);

        for ( var i = 0; i < vertexPositions.length; i++ )
        {
            vertices[ i*3 + 0 ] = vertexPositions[i][0];
            vertices[ i*3 + 1 ] = vertexPositions[i][1];
            vertices[ i*3 + 2 ] = vertexPositions[i][2];
        }

        this.geometry.addAttribute('position', new THREE.BufferAttribute(vertices, 3));

        this.mesh = new THREE.Mesh(this.geometry, this.material);
    }

    get position() {
        this.geometry.computeBoundingBox();
        return this.geometry.boundingBox.center();
    }
}
