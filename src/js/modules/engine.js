import THREE from '../vendor/three.min.js'
import { Camera } from './Camera.js'
import { Mesh } from './Mesh.js'
import { QuadContainer } from './QuadContainer.js'
import THREEx from '../vendor/threex.keyboardstate.js'

export class Engine  {

    constructor(container) {
        this.container = container;
        this.scene = new THREE.Scene();

        this.renderer = new THREE.WebGLRenderer();
        this.renderer.setSize(this.container.offsetWidth, this.container.offsetHeight);

        this.camera = new Camera(this.container);

        window.addEventListener( 'resize', () => {
            this.camera.aspect = this.container.offsetWidth / this.container.offsetHeight;
            this.renderer.setSize( this.container.offsetWidth, this.container.offsetHeight );

        }, false );

        // Our quadtrees for terrain LOD
        this.quadGroups = [];

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

        let quadContainer = new QuadContainer(quadRoot, this);
        this.quadGroups.push(quadContainer);

        // Add this quad group to the scene so it is a child of the scene
        this.add(quadContainer.group);

        let queue = [quadRoot];
        while (queue.length > 0) {
            let q = queue.shift();


            quadContainer.quads.push(q);


            q.visible = false;
            q._isLeaf = !q.children.length;

            queue.push(...q.children);

            // Add this as a child of to a group in the scene, so transformations to the group will apply to this quad as well
            quadContainer.group.add(q.mesh);

            // Generate the vertices of mesh here, since we are now added to the group
            q.generate();

        }

        return quadContainer;
    }

    get domElement() {
        return this.renderer.domElement;
    }

    handleKeyboard() {

        // TODO: Redo controls
        if (this.keyboard.pressed('w')) {
            this.camera.position.y += 1;
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.focus(focus);
        } else if (this.keyboard.pressed('s')) {
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.position.y -= 1;
            this.camera.focus(focus);
        } else if (this.keyboard.pressed('a')) {
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.position.x -= 1;
            this.camera.focus(focus);
        } else if (this.keyboard.pressed('d')) {
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.position.x += 1;
            this.camera.focus(focus);
        } else if (this.keyboard.pressed('e')) {
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.focus(focus);
            this.camera.position.z -= 1;
        } else if (this.keyboard.pressed('q')) {
            let focus = new THREE.Vector3(this.camera.position.x, this.camera.position.y, 0);
            this.camera.focus(focus);
            this.camera.position.z += 1;
        }
    };

    render() {
        requestAnimationFrame(this.render.bind(this));
        // TODO: Drawing quads on each render call is inefficient.  Only draw quads on camera move.
        this.handleKeyboard();

        for (let group of this.quadGroups) {
            group.drawQuads(this.camera);
        }

        this.renderer.render(this.scene, this.camera._camera);

    }

}
