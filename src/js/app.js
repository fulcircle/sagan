import "babel-polyfill";
import { Controls } from './modules/controls.js'
import { Engine } from './modules/engine.js'
import { PlanetGenerator } from './modules/PlanetGenerator.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);
document.body.appendChild(engine.domElement);

//let controls = new Controls();
//controls.addControl(quad, 'LOD').min(1).max(4).step(1);

let TERRAIN_HEIGHT = 128;
let TERRAIN_WIDTH = 128;

//let generator = new TerrainGenerator(engine, TERRAIN_HEIGHT, TERRAIN_WIDTH, HeightMapFuncs.SinRandom.func);
//generator.generate();

window.generator = new PlanetGenerator(engine, TERRAIN_WIDTH);

engine.camera.position = new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 20);
engine.camera.focus(new THREE.Vector3(TERRAIN_WIDTH/2, TERRAIN_HEIGHT/2, 0));

engine.render();

window.engine = engine;
