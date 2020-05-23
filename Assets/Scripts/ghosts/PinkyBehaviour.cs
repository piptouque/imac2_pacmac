using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public class PinkyBehaviour : GhostBehaviour
    {
        override protected Vector2 ChooseDest()
        {
            Vector2 dest = (Vector2) transform.position;
            if (IsDirectionValid(Vector2.up))
            {
                
            }
            else if (IsDirectionValid(Vector2.down))
            {

            }
            else if (IsDirectionValid(Vector2.down))
            {

            }
            else if (IsDirectionValid(Vector2.down))
            {

            }
            return dest;
        }
    }

}

