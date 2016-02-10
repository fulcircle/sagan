import { initArray } from './util.js'

export class HeightMap {

    constructor(width, height, func) {
        this.width = width;
        this.height = height;
        this._initMap(func);
    }


    _initMap(func) {
        this.heightMap = initArray(this.width, this.height);

        for (var a = 0; a < this.heightMap.length; a++) {
            for (var b = 0; b < this.heightMap[a].length; b++) {
                this.heightMap[a][b] = func(a, b);
            }
        }
    }

    // TODO: Think of optimizations
    getHeight(x, y) {

        // Interpolate the height value for x,y on the heightmap using bilinear interpolation
        // See: http://supercomputingblog.com/graphics/coding-bilinear-interpolation/

        let x1 = Math.floor(x);
        let x2 = Math.ceil(x);
        let y1 = Math.floor(y);
        let y2 = Math.ceil(y);

        if (x1 >= this.width || x2 >= this.width
            || y1 >= this.height || y2 >= this.height ) {
            console.log(x1, y1, x2, y2);
            throw new Error("Trying to get an (x,y) value that's off the heightMap");
        }
        // The four surrounding pixels to this pixel on the heightmap
        let q11 = this.heightMap[x1][y1];
        let q12 = this.heightMap[x1][y2];
        let q21 = this.heightMap[x2][y1];
        let q22 = this.heightMap[x2][y2];

        // The bilinear interpolation
        let r1, r2, height = null;
        if (x2 == x1) {
            r1 = q11;
            r2 = q12;
        } else {
            r1 = ((x2 - x)/(x2 - x1))*q11 + ((x - x1)/(x2 - x1))*q21;
            r2 = ((x2 - x)/(x2 - x1))*q12 + ((x - x1)/(x2 - x1))*q22;
        }

        if (y2 == y1) {
            height = r1;
        } else {
            height = ((y2 - y)/(y2 - y1))*r1 + ((y - y1)/(y2 - y1))*r2;
        }

        return height;
    }
}
