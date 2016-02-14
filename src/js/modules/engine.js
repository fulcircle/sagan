import THREE from '../vendor/three.min.js'
import { Camera } from './Camera.js'
import { Mesh } from './Mesh.js'
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

        this.keyboard = new THREEx.KeyboardState();

        this.renderFuncs  = [];

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
        this.handleKeyboard();
        for (let func of this.renderFuncs) {
            func();
        }
        this.renderer.render(this.scene, this.camera._camera);

    }

}
