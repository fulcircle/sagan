import THREE from '../vendor/three.min.js'
import { Camera } from '../modules/camera.js'
import { Mesh } from '../modules/mesh.js'
import THREEx from '../vendor/threex.keyboardstate.js'

export class Engine  {

    constructor(container) {
        this.lines = [];
        this.container = container;
        this.scene = new THREE.Scene();

        this.renderer = new THREE.WebGLRenderer();
        this.renderer.setSize(this.container.offsetWidth, this.container.offsetHeight);

        this.camera = new Camera(this.container);

        window.addEventListener( 'resize', () => {

            this.camera._camera.aspect = this.container.offsetWidth / this.container.offsetHeight;
            this.camera._camera.updateProjectionMatrix();

            this.renderer.setSize( this.container.offsetWidth, this.container.offsetHeight );

        }, false );

        // Our quadtrees for terrain LOD
        this.quads = [];

        this.keyboard = new THREEx.KeyboardState();

    }

    add(object) {
        if (object instanceof Mesh) {
            this.scene.add(object.mesh);
        } else {
            this.scene.add(object);
        }
    }

    remove(object) {
        if (object instanceof Mesh) {
            this.scene.remove(object.mesh);
        } else {
            this.scene.remove(object);
        }

    }

    addQuadTree(quadRoot) {

        let quadDict = {
            root: quadRoot,
            quads: []
        };

        let queue = [quadRoot];
        while (queue.length > 0) {
            let q = queue.shift();

            quadDict.quads.push(q);
            q.visible = false;
            this.add(q);
            q._isLeaf = !q.children;
            queue.push(...q.children);
        }

        this.quads.push(quadDict);
    }

    get domElement() {
        return this.renderer.domElement;
    }

    drawQuads() {
        for (let q of this.quads) {
            q.quads.forEach((q) => {
                q.visible = false;
            });
            this.chunkedLOD(q.root)
        }
    }

    // Chunked LOD implementation: http://tulrich.com/geekstuff/sig-notes.pdf
    // TODO: Optimizations
    // Store coordinates of bounding boxes and exclude branches in quadtree that are out of range
    // Breadth-first search of quadtree?
    chunkedLOD(quad) {

        // TODO: Use box3's distanceToPoint instead
        let distance = this.camera.getDistanceTo(quad.center);
        let rho = quad.error / distance;
        rho = Math.round(rho * 1000) / 1000;

        // distance = 0 so screenspace error should be 0
        if (!isFinite(rho)) {
            rho = 0;
        }
        // Largest allowable screen error
        let tau = 0.2;

        if (quad._isLeaf || rho <= tau) {
            //console.log(quad.center);
            quad.visible = true;
        } else {
            // TODO: When we implement excluding of whole subbranches, we'll have to turn off visibility for all chunks in that branch
            for (let c of quad.children) {
                this.chunkedLOD(c);
            }
        }
    }

    handleKeyboard() {
        if (this.keyboard.pressed('w')) {
            this.camera.position.y += 1;
        } else if (this.keyboard.pressed('s')) {
            this.camera.position.y -= 1;
        } else if (this.keyboard.pressed('a')) {
            this.camera.position.x -= 1;
        } else if (this.keyboard.pressed('d')) {
            this.camera.position.x += 1;
        } else if (this.keyboard.pressed('e')) {
            this.camera.position.z -= 1;
        } else if (this.keyboard.pressed('q')) {
            this.camera.position.z += 1;
}
    };

    render() {
        requestAnimationFrame(this.render.bind(this));
        // TODO: Drawing quads on each render call is inefficient.  Only draw quads on camera move.
        this.handleKeyboard();
        this.drawQuads();
        this.renderer.render(this.scene, this.camera._camera);

    }

}
