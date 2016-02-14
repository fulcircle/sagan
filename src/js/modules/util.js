import THREE from '../vendor/three.min.js'
import { Mesh } from '../modules/Mesh.js'

export function* randomNumber(min, max) {
    while (true) {
        yield (Math.random() * (max - min) + min);
    }
}

export function initArray(length) {
    var arr = new Array(length || 0),
        i = length;

    if (arguments.length > 1) {
        var args = Array.prototype.slice.call(arguments, 1);
        while(i--) arr[length-1 - i] = initArray.apply(this, args);
    }

    return arr;
}

export function getCentroid(object) {
    let box = getBoundingBox(object);

    let centroid = new THREE.Vector3();
    centroid.addVectors(box.min, box.max);
    centroid.multiplyScalar(0.5);
    return centroid;
}

export function getBoundingBox(object) {

    let box = new THREE.Box3();

    if (object instanceof THREE.Object3D) {
        box.setFromObject(object);
    } else if (object instanceof THREE.Box3) {
        box = object;
    } else {
        throw Error('Unrecognized type passed into getBoundingBox()');
    }

    return box;

}

