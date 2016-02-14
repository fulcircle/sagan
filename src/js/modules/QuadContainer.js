// Represents a THREE.Group of Quad Meshes
import THREE from '../vendor/three.min.js'
import { getCentroid } from './Util.js'

export class QuadContainer {

    constructor(quadRoot) {

        this.group = new THREE.Group();
        this.root = quadRoot;
        this.quads = [];
    }

    get centroid() {
        return getCentroid(this.group);
    }

    drawQuads(camera) {
        this.quads.forEach((q) => {
            q.visible = false;
        });
        this.chunkedLOD(this.root, camera)
    }

    // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
    // TODO: Optimizations
    // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
    // Breadth-first search of quadtree?
    chunkedLOD(quad, camera) {

        // TODO: Need to get distance to nearest face, not centroid
        // We're translating from local mesh coordinates (in this QuadContainer's THREE.Group), to the global coordinates
        let worldPosition = new THREE.Vector3();
        worldPosition.setFromMatrixPosition(quad.mesh.matrixWorld);
        // Check the camera distance from world coordinates of this quad to the camera
        let distance = camera.getDistanceTo(worldPosition);

        // Screen space error
        let rho = (quad.error / distance) * camera.perspectiveScalingFactor;

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
                this.chunkedLOD(c, camera);
            }
        }
    }

}