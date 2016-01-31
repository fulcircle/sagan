import { TriangleStripMesh } from './modules/mesh.js'
import { Renderer } from './modules/renderer.js'

let mesh = new TriangleStripMesh();
mesh.addStrip([
    [-1.0, -1.0,  1.0],
    [ 1.0, -1.0,  1.0],
    [ 1.0,  1.0,  1.0],

    [ 1.0,  1.0,  1.0],
    [-1.0,  1.0,  1.0],
    [-1.0, -1.0,  1.0]
]);

let renderer = new Renderer(document.body);
window.renderer = renderer;

document.body.appendChild(renderer.domElement);

renderer.addWireframe(mesh, 0x00ff00);
renderer.focus(mesh);

renderer.render();

