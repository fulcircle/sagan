using UnityEngine;

namespace Sagan.Framework {

    public class Entity : IEntity {

        public GameObject gameObject {
            get;
            private set;
        }

        public Transform transform {
            get {
                return gameObject.transform;
            }
        }

        public Mesh mesh {
            get {
                return gameObject.GetComponent<MeshFilter>().mesh;
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

        /// <summary>
        /// Returns the Axis-Aligned Bounding Box in world space
        /// </summary>
        public Bounds boundingBox {
            get {
                return gameObject.GetComponent<Renderer>().bounds;
            }
        }

        public Entity() {
            gameObject = new GameObject();
        }
    }
}