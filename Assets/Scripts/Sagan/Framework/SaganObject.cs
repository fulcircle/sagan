using System.Collections.Generic;
using UnityEngine;

namespace Sagan.Framework {

    public class SaganObject {

        public GameObject gameObject {
            get;
            protected set;
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

        protected List<SaganObject> _children = new List<SaganObject>();

        public SaganObject(GameObject gameObject = null, string name = "SaganObject") {
            if (gameObject == null) {
                this.gameObject = new GameObject(name);
            } else {
                this.gameObject = gameObject;
            }
        }

        public void AddChild(SaganObject child) {
            child.transform.parent = this.transform;
            this._children.Add(child);
        }

    }
}