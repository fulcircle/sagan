import "babel-polyfill";
import { TerrainMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'
import { Controls } from './modules/controls.js'
import { randomNumber } from './modules/util.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);

document.body.appendChild(engine.domElement);

let meshes = [];

let random = randomNumber(0, 0.3);

let mesh_width = 16;
let mesh_height = 16;

let terrain_size = {
    x: 128,
    y: 128
};

let current_size = {
    x: 0,
    y: 0
};

// Set initial position to draw at the upper left from (0,0) so terrain is centered on the origin
let initial_position = new THREE.Vector3(-(terrain_size.x*0.5), (terrain_size.y*0.5)-mesh_height, 0);
let current_position = new THREE.Vector3(initial_position.x, initial_position.y, initial_position.z);

while (current_size.y < terrain_size.y) {
    let mesh = new TerrainMesh({width: mesh_width, height: mesh_height, LOD: 1});

    mesh.setHeightMap((x, y) => {
        return Math.sin(0.1*x) + Math.sin(0.1*y) + random.next().value;
    });

    mesh.generateTerrain();
    mesh.position.copy(current_position);
    mesh.wireframe = true;
    mesh.LOD = 3;

    engine.add(mesh);

    meshes.push(mesh);

    current_size.x += mesh_width;

    if (current_size.x >= terrain_size.x) {
        current_position.setX(initial_position.x);
        current_position.setY(current_position.y - mesh_height);
        current_size.x = 0;
        current_size.y += mesh_height;
    } else {
        current_position.setX(current_position.x + mesh_width);
    }
}

//let controls = new Controls();
//controls.addControl(mesh, 'LOD').min(1).max(4)
//    .onChange((newLOD) => {
//        meshHelper.wireframe({mesh});
//    });

engine.render();

window.engine = engine;
