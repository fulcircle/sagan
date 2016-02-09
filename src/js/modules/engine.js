import THREE from '../vendor/three.min.js'
import { Camera } from '../modules/camera.js'
import { Mesh } from '../modules/mesh.js'

export class Engine  {

    constructor(container) {
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

    get domElement() {
        return this.renderer.domElement;
    }

    render() {
        requestAnimationFrame(this.render.bind(this));
        this.renderer.render(this.scene, this.camera._camera);

    }

}
