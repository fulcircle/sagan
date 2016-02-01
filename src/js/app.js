import "babel-polyfill";
import { TerrainMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'

let engine = new Engine(document.body);
window.engine = engine;

document.body.appendChild(engine.domElement);

let mesh = new TerrainMesh(16, 16);
mesh.randomTerrain();
engine.addWireframe(mesh, 0x00ff00);
engine.focus(mesh);

//engine.render();

