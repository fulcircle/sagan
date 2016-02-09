import THREE from '../vendor/three.min.js'
import '../vendor/three.orbitcontrols.js'

export class Camera {

    constructor(container) {
        this.container = container;
        this._camera = new THREE.PerspectiveCamera( 75, this.container.offsetWidth / this.container.offsetHeight, .1, 1000 );

        this.orbit = new THREE.OrbitControls( this._camera,  this.container);

    }

    get position() {
        return this._camera.position;
    }

    set position(vec3) {
        this._camera.position.set(vec3.x, vec3.y, vec3.z);
    }

    focus(object) {
        if (object instanceof THREE.Vector3) {
            this.orbit.target.copy(object);
        } else {
            this.orbit.target.copy(object.position);
        }
        this.orbit.update();
    }

}
