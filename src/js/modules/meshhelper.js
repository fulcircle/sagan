export class MeshHelper {

    constructor(engine) {
        this.engine = engine;
    }
    //
    //// TODO: Replace this logic with a wireframe shader on the mesh,
    //// so we don't need to remove and add a new wireframe
    wireframe({mesh, color=0xffffff}) {
        if (mesh._wireframe) {
            this.engine.remove(mesh._wireframe);
        }
        let wire = mesh.wireframe(color);
        this.engine.add(wire);
    }
}
