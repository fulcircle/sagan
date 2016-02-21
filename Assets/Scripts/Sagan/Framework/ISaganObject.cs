using UnityEngine;

namespace Sagan.Framework {

    public interface ISaganObject {

        GameObject gameObject {
            get;
        }

        MeshRenderer renderer {
            get;
        }

        MeshFilter meshFilter {
            get;
        }

        Transform transform {
            get;
        }

        Mesh mesh {
            get;
        }

        Bounds boundingBox {
            get;
        }

        bool active {
            set;
            get;
        }

        Material material {
            get;
        }

        Material[] materials {
            get;
        }

    }
}