// Represents a THREE.Group of Quad Meshes
import THREE from '../vendor/three.min.js'

export class QuadGroup {

    constructor(quadRoot, engine) {

        this.group = new THREE.Group();
        this.root = quadRoot;
        this.quads = [];
        this.engine = engine;

    }
}