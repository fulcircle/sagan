import THREE from '../vendor/three.min.js';
import { TerrainGenerator } from './terraingenerator.js';
import { HeightMapFuncs } from './terraingenerator.js';

export class PlanetGenerator {

    constructor(engine, radius) {
        let generator = new TerrainGenerator(engine, radius, radius, HeightMapFuncs.SinRandom.func);
        let quadGroup = generator.generate();

        var faceAngle = new THREE.Euler( 0, THREE.Math.degToRad(90), 0, 'XYZ' );

    }
}