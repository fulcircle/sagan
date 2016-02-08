import "babel-polyfill";
import { QuadMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'
import { Controls } from './modules/controls.js'
import { randomNumber } from './modules/util.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);

document.body.appendChild(engine.domElement);

let random = randomNumber(0, 0.3);

let quad = new QuadMesh({width: 32, height: 32, LOD: 1});

quad.setHeightMap((x, y) => {
    return Math.sin(0.1 * x) + Math.sin(0.1 * y) + random.next().value;
});

quad.generateTerrain();
quad.coordinates = new THREE.Vector3(-16, 16, 0);
generateQuadTree(quad);

function generateQuadTree(parent_quad) {
    if (parent_quad.LOD > 4) {
        return;
    }
    let currX = parent_quad.coordinates.x;
    let currY = parent_quad.coordinates.y;
    let currZ = parent_quad.coordinates.z;

    let xstride = parent_quad.width * 0.5;
    let ystride = parent_quad.height * 0.5;

    let LOD = parent_quad.LOD + 1;

    for (var i = 0; i < 4; i++) {
        let quad = new QuadMesh({
            width: parent_quad.width * 0.5,
            height: parent_quad.height * 0.5,
            LOD: LOD
        });
        quad.setHeightMap((x, y) => {
            return Math.sin(0.1 * x) + Math.sin(0.1 * y) + random.next().value;
        });

        quad.wireframe = true;
        quad.generateTerrain();

        quad.coordinates = new THREE.Vector3(currX, currY, currZ);

        parent_quad.children.push(quad);

        engine.add(quad);

        currX = currX + xstride;
        if ((currX - parent_quad.coordinates.x) >= parent_quad.width) {
            currX = parent_quad.coordinates.x;
            currY = currY - ystride;
        }
        generateQuadTree(quad);
    }
}

//let controls = new Controls();
//controls.addControl(mesh, 'LOD').min(1).max(4)
//    .onChange((newLOD) => {
//        meshHelper.wireframe({mesh});
//    });

engine.render();

window.engine = engine;
