import "babel-polyfill";
import { TerrainMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'

let engine = new Engine(document.body);
window.engine = engine;

document.body.appendChild(engine.domElement);

var mesh = new TerrainMesh({width: 16, height: 16, LOD: 1});
mesh.randomHeightMap();
mesh.generateTerrain();
engine.addWireframe(mesh, 0x00ff00);
engine.focus(mesh);

engine.render();

setTimeout(remove, 1000);

function remove() {
    //let newMesh = new TerrainMesh({width: 16, height: 16, LOD: 2});
    //newMesh.randomHeightMap();
    //newMesh.generateTerrain();
    engine.removeMesh(mesh);
    //engine.addWireframe(newMesh, 0x00ff00);
}
