using System.Runtime.CompilerServices;
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
                this.RecalculateBounds();
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

        /// <summary>
        /// Returns the Axis-Aligned Bounding Box in local space
        /// </summary>
        public Bounds localBoundingBox {
            get {
                return mesh.bounds;
            }
        }

        public Material material {
            get {
                return renderer.material;
            }

            set {
                renderer.material = value;
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

        public bool boundingBoxVisible {
            get {
                if (this._boundingBox != null) {
                    return this._boundingBox.visible;
                }
                else {
                    return false;
                }
            }

            set {
                if (value) {
                    this.AddBoundingBox();
                }

                this._boundingBox.visible = value;
            }

        }


        private SaganBox _boundingBox;

        public SaganMesh(GameObject gameObject = null, string name = "SaganMesh") : base(gameObject, name) {
            if (this.gameObject.GetComponent<MeshFilter>() == null) {
                this.gameObject.AddComponent<MeshFilter>();
            }

            if (this.gameObject.GetComponentInChildren<MeshRenderer>() == null) {
                this.gameObject.AddComponent<MeshRenderer>();
            }
        }

        public void RecalculateBounds() {
            meshFilter.mesh.RecalculateBounds();
            meshFilter.mesh.RecalculateNormals();

            if (this._boundingBox != null) {
                this._boundingBox.transform.localPosition = this.localBoundingBox.center;
                this._boundingBox.transform.localScale = this.localBoundingBox.max;
            }

        }

        protected void AddBoundingBox() {
            if (this._boundingBox != null) { return; }
            this._boundingBox = new SaganBox("BoundingBox");
            this.AddChild(this._boundingBox);
            this.RecalculateBounds();
        }
    }
}