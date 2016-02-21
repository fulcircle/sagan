using UnityEngine;

namespace Sagan.Framework {

    public class SaganObject : ISaganObject {

        public GameObject gameObject {
            get;
            private set;
        }

        public MeshRenderer renderer {
            get {
                return gameObject.GetComponent<MeshRenderer>();
            }
        }

        public MeshFilter meshFilter {
            get {
                return gameObject.GetComponent<MeshFilter>();
            }
        }

        public Transform transform {
            get {
                return gameObject.transform;
            }
        }

        public Mesh mesh {
            get {
                return meshFilter.mesh;
            }

            set {
                meshFilter.mesh = value;
            }
        }

        /// <summary>
        /// Returns the Axis-Aligned Bounding Box in world space
        /// </summary>
        public Bounds boundingBox {
            get {
                return renderer.bounds;
            }
        }

        public bool active {
            get {
                return gameObject.activeSelf;
            }

            set {
                gameObject.SetActive(value);
            }
        }

        public Material material {
            get {
                return renderer.material;
            }
        }

        public Material[] materials {
            get {
                return renderer.materials;
            }
        }

        private bool _wireframe = false;

        public bool wireframe {
            get {
                return _wireframe;
            }

            set {
                if (value) {
                    renderer.material = Resources.Load("Materials/Wireframe.mat", typeof(Material)) as Material;
                } else {
                    renderer.material = null;
                }
            }
        }

        public SaganObject() {
            gameObject = new GameObject();

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
        }

    }
}