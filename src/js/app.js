import "babel-polyfill";
import { Controls } from './modules/Controls.js'
import { Engine } from './modules/Engine.js'
import { Planet } from './modules/Planet.js'
import THREE from './vendor/three.min.js';

let engine = new Engine(document.body);
document.body.appendChild(engine.domElement);

//let controls = new Controls();
//controls.addControl(quad, 'LOD').min(1).max(4).step(1);

let radius = 128;
let planet = new Planet(engine, radius);

var axisHelper = new THREE.AxisHelper( radius*2 );
engine.add( axisHelper );

engine.camera.position = new THREE.Vector3(planet.centroid.x, planet.centroid.y, 20);
engine.camera.focus(planet.centroid);

engine.render();

window.engine = engine;
window.planet = planet;
