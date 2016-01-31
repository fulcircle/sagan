import THREE from '../vendor/three.min.js'
import { Camera } from '../modules/camera.js'

export class Renderer {

    constructor(container) {
        this.container = container;
        this.scene = new THREE.Scene();

        this.renderer = new THREE.WebGLRenderer();
        this.renderer.setSize(this.container.offsetWidth, this.container.offsetHeight);

        this.camera = new Camera(this.container);
        this.camera.position.z = 5;

        window.addEventListener( 'resize', () => {

            this.camera._camera.aspect = this.container.offsetWidth / this.container.offsetHeight;
            this.camera._camera.updateProjectionMatrix();

            this.renderer.setSize( this.container.offsetWidth, this.container.offsetHeight );

        }, false );
    }

    /*
    mesh:  Mesh object (defined in mesh.js)
     */
    addMesh(mesh) {
        this.scene.add(mesh.mesh);
    }

    get domElement() {
        return this.renderer.domElement;
    }

    addWireframe(mesh, color) {
        this.scene.add(mesh.wireFrame(color));
    }

    focus(mesh) {
        this.camera.focus(mesh);
    }

    render() {
        requestAnimationFrame(this.render.bind(this));
        this.renderer.render(this.scene, this.camera._camera);

    }

}
