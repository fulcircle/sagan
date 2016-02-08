export class HeightMap {

    constructor(width, height) {
        this.map = 
    }

    getHeight(x, y) {
        // Get the ratio of the heightmap dimensions to the mesh dimensions
        let xratio = this.heightMap.length * ( 1 / (this.heightMapWidth) );
        let yratio = this.heightMap[0].length * (1 / (this.heightMapHeight) );

        // Get the height coordinates on the heightmap that correspond to the x,y vertex on the mesh
        // Note: this won't necessarily be an integer, which is why we perform a bilinear interpolation next
        let heightmapx = x*xratio;
        let heightmapy = y*yratio;

        // Interpolate the height value for x,y on the heightmap using bilinear interpolation
        // See: http://supercomputingblog.com/graphics/coding-bilinear-interpolation/

        let x1 = Math.floor(heightmapx);
        let x2 = Math.ceil(heightmapx);
        let y1 = Math.floor(heightmapy);
        let y2 = Math.ceil(heightmapy);

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
            r1 = ((x2 - heightmapx)/(x2 - x1))*q11 + ((heightmapx - x1)/(x2 - x1))*q21;
            r2 = ((x2 - heightmapx)/(x2 - x1))*q12 + ((heightmapx - x1)/(x2 - x1))*q22;
        }

        if (y2 == y1) {
            height = r1;
        } else {
            height = ((y2 - heightmapy)/(y2 - y1))*r1 + ((heightmapy - y1)/(y2 - y1))*r2;
        }


        return height;
    }
    }
}
