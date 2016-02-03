import { dat } from '../vendor/dat.gui.min.js'
export class Controls {
    constructor() {
        this.gui = new dat.GUI();
    }

    addControl(obj, prop) {
        return this.gui.add(obj, prop);
    }
}