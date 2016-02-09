import "babel-polyfill";
import { QuadMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'
import { Controls } from './modules/controls.js'
import { HeightMap } from './modules/heightmap.js'
import { randomNumber } from './modules/util.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);
document.body.appendChild(engine.domElement);

let random = randomNumber(0, 0.3);
let heightMapFunc = function(x, y) {
    return Math.sin(0.1 * x) + Math.sin(0.1 * y) + random.next().value;
};

let TERRAIN_HEIGHT = 32;
let TERRAIN_WIDTH = 32;

let quad = new QuadMesh({height: TERRAIN_HEIGHT, width: TERRAIN_WIDTH, LOD: 1});
let heightMap = new HeightMap(quad.width + 1, quad.height + 1, heightMapFunc);
quad.heightMap = heightMap;

quad.coordinates = new THREE.Vector3(0, quad.height, 0);
quad.generate();

generateQuadTree(quad);

// TODO: Convert into breadth-first generation of tree
function generateQuadTree(parent_quad) {
    if (parent_quad.LOD > 1) {
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
            LOD: LOD,
            heightMap: parent_quad.heightMap
        });

        quad.wireframe = true;

        quad.coordinates = new THREE.Vector3(currX, currY, currZ);
        quad.generate();


        parent_quad.children.push(quad);

        engine.add(quad);

        currX = currX + xstride;
        if ((currX - parent_quad.coordinates.x) >= parent_quad.width) {
            currX = parent_quad.coordinates.x;
            currY = currY + ystride;
        }
        generateQuadTree(quad);
    }
}

//let controls = new Controls();
//controls.addControl(mesh, 'LOD').min(1).max(4)
//    .onChange((newLOD) => {
//        meshHelper.wireframe({mesh});
//    });


engine.camera.position = new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 5);
engine.camera.focus(new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 0));
engine.render();

window.engine = engine;
