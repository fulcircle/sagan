import THREE from '../vendor/three.min.js'
import '../vendor/three.orbitcontrols.js'

export class Camera {

    constructor(container) {
        this.container = container;
        this._camera = new THREE.PerspectiveCamera( 75, this.container.offsetWidth / this.container.offsetHeight, .1, 5000 );

        this.orbit = new THREE.OrbitControls( this._camera,  this.container);

        this.updateFOV();

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

    get aspect() {
        return this._camera.aspect;
    }

    set aspect(aspect) {
        this._camera.aspect = aspect;
        this._camera.updateProjectionMatrix();

        this.updateFOV();
    }

    set zoom(zoom) {
        this._camera.zoom = zoom;
        this._camera.updateProjectionMatrix();

        this.updateFOV();
    }

    updateFOV() {
        this.vFOV = THREE.Math.degToRad(this._camera.fov);
        this.hFOV = 2 * Math.atan( Math.tan( this.vFOV * 0.5 ) * this._camera.aspect  / this._camera.zoom);
        // Perspective scaling factor
        this.horizontalScalingFactor = (2 * Math.tan(this.hFOV * 0.5));
        this.perspectiveScalingFactor = this.container.offsetWidth / this.horizontalScalingFactor;
    }



}
