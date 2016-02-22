using UnityEngine;
using Sagan.Framework;
using Sagan.Terrain;

namespace Sagan.Planet {


    public class Planet : SaganMesh {

        private Sagan.Terrain.Terrain[] _faces = new Sagan.Terrain.Terrain[6];
        private Sagan.Framework.Camera _camera;

        public float radius {
            get;
            private set;
        }

        public int levels {
            get;
            private set;
        }

        public Planet(Sagan.Framework.Camera camera, float radius, int levels=2) : base(name: "SaganPlanet") {

            this._camera = camera;
            this.radius = radius;

            this.levels = levels;


            // Going to spherize the terrain cube, so we calculate size of cube face to get the radius we want
            var terrain_size = Mathf.RoundToInt(Mathf.Sqrt(2.0f*radius*radius));

            // Add a cube to center our local origin around the cube faces we will be adding
            var cube = new SaganCube();
            this.AddChild(cube);
            cube.transform.localScale = new Vector3(terrain_size, terrain_size, terrain_size);
            cube.visible = false;

            // Add our terrain faces to the cube

            // Face 0
            var face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
            face.gameObject.name = "Face 0";
            this.AddChild(face);
            var rot = new Vector3(-90, 0, 0);
            face.transform.localEulerAngles = rot;
            face.transform.localPosition = new Vector3(-terrain_size * 0.5f, -terrain_size * 0.5f, -terrain_size * 0.5f);
            this._faces[0] = face;

//            // Face 1
//            face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
//            face.gameObject.name = "Face 1";
//            this.AddChild(face);
//            rot = new Vector3(90, 0, 0);
//            face.transform.localEulerAngles = rot;
//            face.transform.localPosition = new Vector3(-terrain_size * 0.5f, terrain_size * 0.5f, terrain_size * 0.5f);
//            this._faces[1] = face;
//
//            // Face 2
//            face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
//            face.gameObject.name = "Face 2";
//            this.AddChild(face);
//            rot = new Vector3(0, 0, 90);
//            face.transform.localEulerAngles = rot;
//            face.transform.localPosition = new Vector3(-terrain_size * 0.5f, -terrain_size * 0.5f, -terrain_size * 0.5f);
//            this._faces[2] = face;
//
//            // Face 3
//            face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
//            face.gameObject.name = "Face 3";
//            this.AddChild(face);
//            rot = new Vector3(0, 0, -90);
//            face.transform.localEulerAngles = rot;
//            face.transform.localPosition = new Vector3(terrain_size * 0.5f, terrain_size * 0.5f, -terrain_size * 0.5f);
//            this._faces[3] = face;
//
//            // Face 4
//            face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
//            face.gameObject.name = "Face 4";
//            this.AddChild(face);
//            rot = new Vector3(0, 90, 0);
//            face.transform.localEulerAngles = rot;
//            face.transform.localPosition = new Vector3(-terrain_size * 0.5f, terrain_size * 0.5f, terrain_size * 0.5f);
//            this._faces[4] = face;
//
//            // Face 5
//            face = new Sagan.Terrain.Terrain(terrain_size, this.levels, this._camera);
//            face.gameObject.name = "Face 5";
//            this.AddChild(face);
//            rot = new Vector3(0, 0, 180);
//            face.transform.localEulerAngles = rot;
//            face.transform.localPosition = new Vector3(terrain_size * 0.5f, -terrain_size * 0.5f, -terrain_size * 0.5f);
//            this._faces[5] = face;

        }

        public void Update() {
            foreach (var face in this._faces) {
                face.Update();
            }
        }
    }

}