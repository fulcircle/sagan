using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sagan.Terrain;
using Camera = Sagan.Framework.Camera;

public interface ILodStrategy {
    List<Quad> quads { get; }

    void Precalculate();
    void Spherify();
    void Create();
    void Update(Camera cam);
}
