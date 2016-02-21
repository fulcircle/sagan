using UnityEngine;

namespace Sagan.Framework {

    public class SaganObject {

        public GameObject gameObject {
            get;
            private set;
        }


        public Transform transform {
            get {
                return gameObject.transform;
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

        public SaganObject() {
            gameObject = new GameObject();
        }

    }
}