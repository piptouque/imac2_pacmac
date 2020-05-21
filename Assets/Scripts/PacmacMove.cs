using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmacMove : MonoBehaviour
{
    public float _speed = 0.4f;

    private Vector2 _dest = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        _dest = transform.position;
    }

    void FixedUpdate()
    {
        if ((Vector2) transform.position != _dest)
        {
            /* input if moving */
            Vector2 p = Vector2.MoveTowards(transform.position, _dest, _speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        else
        {
            if(Input.GetKey(KeyCode.UpArrow) && IsDirectionValid(Vector2.up))
            {
                _dest = (Vector2) transform.position + Vector2.up;
            }
            else if(Input.GetKey(KeyCode.RightArrow) && IsDirectionValid(Vector2.right))
            {
                _dest = (Vector2) transform.position + Vector2.right;
            }
            else if(Input.GetKey(KeyCode.DownArrow) && IsDirectionValid(- Vector2.up))
            {
                _dest = (Vector2) transform.position - Vector2.up;
            }
            else if(Input.GetKey(KeyCode.LeftArrow) && IsDirectionValid(- Vector2.right))
            {
                _dest = (Vector2) transform.position - Vector2.right;
            }
        }
        Vector2 dir = _dest - (Vector2) transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private bool IsDirectionValid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GetComponent<Collider2D>());
    }
}
