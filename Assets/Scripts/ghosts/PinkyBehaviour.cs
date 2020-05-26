using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public class PinkyBehaviour : GhostBehaviour
    {
        public PinkyBehaviour()
        {
            _dirPriorities[0] = - Vector2.up;
            _dirPriorities[1] = Vector2.up;
            _dirPriorities[2] = Vector2.right;
            _dirPriorities[3] = - Vector2.right;

            _prevDir = _dirPriorities[0];
        }
    }
}

