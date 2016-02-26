using Sagan.Framework;
using Sagan.Terrain;
using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Planet {


    public class Planet : SaganMesh {

        private Sagan.Framework.Camera _camera;
        private Sagan.Framework.SaganCube _planetCube;

        private List<Sagan.Terrain.Terrain> _faces = new List<Sagan.Terrain.Terrain>();

        private int _terrainSize;

        public int radius {
            get;
            private set;
        }

        public int levels {
            get;
            private set;
        }

        public Planet(Sagan.Framework.Camera camera, int radius, int levels=2) : base(name: "SaganPlanet") {

            this._camera = camera;
            this.radius = radius;

            this.levels = levels;

            this._terrainSize = 2 * this.radius;

            // Add a cube to center our local origin around the cube faces we will be adding
            this._planetCube = new SaganCube("PlanetCube");
            this.AddChild(_planetCube);
            this._planetCube.transform.localScale = new Vector3(this._terrainSize, this._terrainSize, this._terrainSize);
            this._planetCube.visible = false;

            // Add our terrain faces to the cube
            // If the faces are splayed out, this is how they would look:
            //   4
            // 0 1 2 3
            //   5
            this.AddFace("Face 0", new Vector3(270, 0, 0), new Vector3(0, 0, -this.radius));
            this.AddFace("Face 1", new Vector3(270, 270, 0), new Vector3(this.radius, 0, 0));
            this.AddFace("Face 2", new Vector3(270, 180, 0), new Vector3(0, 0, this.radius));
            this.AddFace("Face 3", new Vector3(270, 90, 0), new Vector3(-this.radius, 0, 0));
            this.AddFace("Face 4", new Vector3(0, 270, 0), new Vector3(0, this.radius, 0));
            this.AddFace("Face 5", new Vector3(0, 90, 180), new Vector3(0, -this.radius, 0));

        }

        public void Update() {
            foreach (var face in this._faces) {
                face.Update();
            }
        }
        
        void AddFace(string name, Vector3 rotate, Vector3 translate) {
            var face = new Sagan.Terrain.Terrain(this._terrainSize, this.levels, this._camera);
            face.gameObject.name = name;

            this._planetCube.AddChild(face);
            face.Precalculate();
            face.Spherify(this.radius);
            face.Create();

            face.transform.Translate(translate);
            face.transform.Rotate(rotate);
            this._faces.Add(face);
        }
    }

}