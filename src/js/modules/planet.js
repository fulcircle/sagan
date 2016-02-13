import THREE from './vendor/three.min.js';

export class Planet {

    constructor(radius) {
        let generator = new TerrainGenerator(engine, radius, radius, HeightMapFuncs.SinRandom.func);
        generator.generate();

        var faceAngle = new THREE.Euler( 0, THREE.Math.degToRad(90), 0, 'XYZ' );
    }
}