using UnityEngine;

namespace Sagan.Framework {

    public class SaganMesh : SaganObject {

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

        public bool visible {
            get {
                return renderer.enabled;
            }

            set {
                renderer.enabled = value;
            }
        }


        public SaganMesh(GameObject gameObject = null, string name = "SaganMesh") : base(gameObject, name) {
            if (this.gameObject.GetComponent<MeshFilter>() == null) {
                this.gameObject.AddComponent<MeshFilter>();
            }

            if (this.gameObject.GetComponentInChildren<MeshRenderer>() == null) {
                this.gameObject.AddComponent<MeshRenderer>();
            }
        }

    }
}