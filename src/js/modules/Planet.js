import THREE from '../vendor/three.min.js';
import { getBoundingBox, getCentroid } from './Util.js';
import { Terrain, HeightMapFuncs } from './Terrain.js';

export class Planet {

    constructor(engine, radius) {
        this.cube = new THREE.Group();
        engine.add(this.cube);

        // Going to spherize the terrain cube, so we make each face 90 degree extent from center of cube
        let terrain_size = Math.round(Math.sqrt(2*radius*radius));
        // Round to nearest multiple of 2 since we're using quads
        terrain_size = Math.round(terrain_size / 2) * 2;

        console.log('Sphere radius: ' + radius);
        console.log('Cube face size: ' + terrain_size);

        let sides = [
            //{
            //    axis: 'x',
            //    degrees: '90'
            //},
            //{
            //    axis: 'y',
            //    degrees: '-90'
            //},
            //{
            //    axis: 'z',
            //    degrees: '0'
            //},
            {
                axis: 'x',
                degrees: '90',
                translation: {
                    x: 0,
                    y: terrain_size,
                    z: 0
                }
            },
            //{
            //   translation: {
            //       x: 0,
            //       y: 0,
            //       z: terrain_size
            //   }
            //},
            //{
            //    axis: 'y',
            //    degrees: '-90',
            //    translation: {
            //        x: terrain_size,
            //        y: 0,
            //        z: 0
            //    }
            //}

        ];


        for (var i = 0; i < sides.length; i++) {
            let terrain = new Terrain(terrain_size, HeightMapFuncs.SinRandom.func);
            //if (sides[i].translation) {
            //    terrain.mesh.translateZ(sides[i].translation.z);
            //    terrain.mesh.translateY(sides[i].translation.y);
            //    terrain.mesh.translateX(sides[i].translation.x);
            //}
            //if (sides[i].axis) {
            //    terrain.mesh.rotation[sides[i].axis] = THREE.Math.degToRad(sides[i].degrees);
            //}

            // Add this terrain as one of our cube faces
            this.cube.add(terrain.mesh);

            //for (let q of terrain.quads) {
            //    q.spherify(radius);
            //}

            engine.renderFuncs.push(() => {
                terrain.draw(engine.camera.position, engine.camera.perspectiveScalingFactor);
            })

        }

    }

    get centroid() {
        return getCentroid(this.cube);
    }

}