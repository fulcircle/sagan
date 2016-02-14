import THREE from '../vendor/three.min.js';
import { TerrainGenerator } from './TerrainGenerator.js';
import { HeightMapFuncs } from './TerrainGenerator.js';

export class PlanetGenerator {

    constructor(engine, radius) {
        let generator = new TerrainGenerator(engine, radius, radius, HeightMapFuncs.SinRandom.func);
        this.quadGroup = generator.generate();

        //var faceAngle = new THREE.Euler( 0, THREE.Math.degToRad(90), 0, 'XYZ' );
        //this.quadGroup.group.rotateX(90)

    }
}