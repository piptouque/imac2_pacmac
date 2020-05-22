using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        public float _speedBase = 0.4f;
        protected Vector2 _dest = Vector2.zero;

        void Start()
        {
            _dest = transform.position;
        }
        void FixedUpdate()
        {
            
            if ((Vector2) transform.position != _dest)
            {
                /* input if moving */
                Vector2 p = Vector2.MoveTowards(transform.position, _dest, GetSpeed());
                GetComponent<Rigidbody2D>().MovePosition(p);
            }
            else
            {
                ChooseDest();
            }
            Vector2Int dir = new Vector2Int((int)(_dest.x - transform.position.x), (int)(_dest.y - transform.position.y));
            GetComponent<Animator>().SetInteger("DirX", dir.x);
            GetComponent<Animator>().SetInteger("DirY", dir.y);
        }
        abstract protected void OnTriggerEnter2D(Collider2D other);

        abstract protected void ChooseDest();
        abstract public void Reset(Vector3 pos3D);
        abstract protected float GetSpeed();

        public float GetSpeedBase()
        {
            return _speedBase;
        }
        protected bool IsDirectionValid(Vector2 dir)
        {
            Vector2 pos = transform.position;
            RaycastHit2D ray = Physics2D.Linecast(pos + dir, pos);
            bool hit = (ray.collider != GetComponent<Collider2D>());
            return hit ? ray.collider.isTrigger : true;
        }
    }

}

