using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacmac
{
    public abstract class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField]
        protected float _speedBase = 0.4f;
        protected Vector2 _dest;

        abstract protected Character GetCharacter();

        virtual protected void FixedUpdate()
        {
            if(HeDed())
            {
                DeadUpdate();
            }
            else
            {
                MoveUpdate();
            }
        }

       private void MoveUpdate()
        {
            if ((Vector2) transform.position != _dest)
            {
                /* input if moving */
                Vector2 p = Vector2.MoveTowards(transform.position, _dest, GetCharacter().GetSpeed());
                GetComponent<Rigidbody2D>().MovePosition(p);
            }
            else
            {
                _dest = ChooseDest();
            }
            Vector2Int dir = new Vector2Int((int)(_dest.x - transform.position.x), (int)(_dest.y - transform.position.y));
            GetComponent<Animator>().SetInteger("DirX", dir.x);
            GetComponent<Animator>().SetInteger("DirY", dir.y);
        }

        abstract protected void DeadUpdate();

        public bool HeDed()
        {
            return GetCharacter().HeDed();
        }
        abstract protected Vector2 ChooseDest();
        virtual public void ResetPosition(Vector3 pos3D, Configuration conf)
        {
            transform.position = pos3D;
            _dest = (Vector2) pos3D;
            Show();
        }
        virtual public void ResetPosition()
        {
            Hide();
        }
        private void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }
        private void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }
        protected bool IsDirectionValid(Vector2 dir)
        {
            var pos = (Vector2) transform.position;
            RaycastHit2D ray = Physics2D.Linecast(pos + dir, pos);
            bool hit = (ray.collider != GetComponent<Collider2D>());
            bool isValid = hit ? ray.collider.isTrigger : true;
            return isValid;
        }
    }

}

