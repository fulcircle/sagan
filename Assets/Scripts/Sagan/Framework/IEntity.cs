using UnityEngine;

namespace Sagan.Framework {

    public interface IEntity {

        GameObject gameObject {
            get;
        }

        Transform transform {
            get;
        }

        Mesh mesh {
            get;
        }

        bool active {
            get; set;
        }

    }
}