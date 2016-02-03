import "babel-polyfill";
import { TerrainMesh } from './modules/mesh.js'
import { Engine } from './modules/engine.js'
import { MeshHelper } from './modules/meshhelper.js'
import { Controls } from './modules/controls.js'
import { randomNumber } from './modules/util.js'

let engine = new Engine(document.body);

document.body.appendChild(engine.domElement);

let mesh = new TerrainMesh({width: 16, height: 16, LOD: 1});
let meshHelper = new MeshHelper(engine);

let random = randomNumber(0, 0.3);

mesh.setHeightMap((x, y) => {
    return Math.sin(0.1*x) + Math.sin(0.1*y) + random.next().value;
});
mesh.generateTerrain();

meshHelper.wireframe({mesh});

let controller = {
    set LOD(lod) {
        mesh.LOD = lod;
        meshHelper.wireframe({mesh})
    }
}
let controls = new Controls();
controls.addControl(mesh, 'LOD').min(1).max(4)
    .onChange((newLOD) => {
        meshHelper.wireframe({mesh});
    });

engine.focus(mesh);
engine.render();

window.engine = engine;
