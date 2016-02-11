import "babel-polyfill";
import { QuadMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'
import { Controls } from './modules/controls.js'
import { HeightMap } from './modules/heightmap.js'
import { randomNumber } from './modules/util.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);
document.body.appendChild(engine.domElement);

let random = randomNumber(0, 2);
let heightMapFunc = function(x, y) {
    return 3*Math.sin(0.1*x) + 3*Math.sin(0.1*y) + random.next().value;
};

let TERRAIN_HEIGHT = 64;
let TERRAIN_WIDTH = 64;

// Add +1 to width and height of heightmap so bilinear interpolation of quad can interpolate extra data point beyond edge of quad
let heightMap = new HeightMap(TERRAIN_WIDTH + 1, TERRAIN_HEIGHT + 1, heightMapFunc);

let quad = new QuadMesh({
    height: TERRAIN_HEIGHT,
    width: TERRAIN_WIDTH,
    LOD: 1,
    heightMap: heightMap,
    error: 8
});

quad.wireframe = true;
engine.add(quad);
quad.position = new THREE.Vector3();

//let controls = new Controls();
//controls.addControl(quad, 'LOD').min(1).max(4).step(1);

generateQuadTree(quad);

// TODO: Convert into breadth-first generation of tree
function generateQuadTree(parent_quad) {
    if (parent_quad.LOD > 4) {
        return;
    }
    let currX = parent_quad.position.x;
    let currY = parent_quad.position.y;
    let currZ = parent_quad.position.z;

    let xstride = parent_quad.width * 0.5;
    let ystride = parent_quad.height * 0.5;

    let LOD = parent_quad.LOD + 1;

    for (var i = 0; i < 4; i++) {
        let quad = new QuadMesh({
            width: parent_quad.width * 0.5,
            height: parent_quad.height * 0.5,
            LOD: LOD,
            heightMap: parent_quad.heightMap,
            error: parent_quad.error * 0.5
        });

        quad.wireframe = true;

        //quad.position = new THREE.Vector3(currX, currY, currZ);

        parent_quad.children.push(quad);
        engine.add(quad);
        quad.position = new THREE.Vector3(currX, currY, currZ);

        currX = currX + xstride;
        if ((currX - parent_quad.position.x) >= parent_quad.width) {
            currX = parent_quad.position.x;
            currY = currY + ystride;
        }
        generateQuadTree(quad);
    }
}

engine.addQuadTree(quad);

engine.camera.position = new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 20);
engine.camera.focus(new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 0));

engine.render();

window.engine = engine;
