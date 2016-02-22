using Sagan.Framework;
using Sagan.Terrain;
using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Planet {


    public class Planet : SaganMesh {

        private List<Sagan.Terrain.Terrain> _faces = new List<Sagan.Terrain.Terrain>();
        private Sagan.Framework.Camera _camera;
        private Sagan.Framework.SaganCube _planetCube;

        private int _terrainSize;

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
            this._terrainSize = Mathf.RoundToInt(Mathf.Sqrt(2.0f*radius*radius));

            // Add a cube to center our local origin around the cube faces we will be adding
            this._planetCube = new SaganCube();
            this.AddChild(_planetCube);
            this._planetCube.transform.localScale = new Vector3(this._terrainSize, this._terrainSize, this._terrainSize);
            this._planetCube.visible = false;

            // Add our terrain faces to the cube

            // Face 0
            this.AddFace("Face 0", 
                        new Vector3(-this._terrainSize * 0.5f, -this._terrainSize * 0.5f, -this._terrainSize * 0.5f),
                        new Vector3(-90, 0, 0));

//            // Face 1
//            this.AddFace("Face 1",
//                        new Vector3(-this._terrainSize * 0.5f, this._terrainSize * 0.5f, this._terrainSize * 0.5f),
//                        new Vector3(90, 0, 0));
//
//            // Face 2
//            this.AddFace("Face 2",
//                    new Vector3(-this._terrainSize * 0.5f, -this._terrainSize * 0.5f, -this._terrainSize * 0.5f),
//                    new Vector3(0, 0, 90));
//
//            // Face 3
//            this.AddFace("Face 3",
//                    new Vector3(this._terrainSize * 0.5f, this._terrainSize * 0.5f, -this._terrainSize * 0.5f),
//                    new Vector3(0, 0, -90));
//
//            // Face 4
//            this.AddFace("Face 4",
//                    new Vector3(-this._terrainSize * 0.5f, this._terrainSize * 0.5f, this._terrainSize * 0.5f),
//                    new Vector3(0, 90, 0));
//
//            // Face 5
//            this.AddFace("Face 5",
//                    new Vector3(this._terrainSize * 0.5f, -this._terrainSize * 0.5f, -this._terrainSize * 0.5f),
//                    new Vector3(0, 0, 180));

        }
        
        void AddFace(string name, Vector3 position, Vector3 rotation) {
            var face = new Sagan.Terrain.Terrain(this._terrainSize, this.levels, this._camera);
            face.gameObject.name = name;
            this.AddChild(face);
            face.PrecalculateQuads();
            face.Spherify(this.radius);
            face.CreateQuads();
            face.transform.localEulerAngles = rotation;
            face.transform.localPosition = position;
            this._faces.Add(face);
        }
    }

}