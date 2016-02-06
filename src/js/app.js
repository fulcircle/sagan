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

//let controls = new Controls();
//controls.addControl(mesh, 'LOD').min(1).max(4)
//    .onChange((newLOD) => {
//        meshHelper.wireframe({mesh});
//    });

engine.render();

window.engine = engine;
