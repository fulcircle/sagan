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

        public Entity() {
            gameObject = new GameObject();
        }
    }
}