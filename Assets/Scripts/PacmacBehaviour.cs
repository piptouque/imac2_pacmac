using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmacBehaviour : MonoBehaviour
{
    public float _speed = 0.4f;
    private int _score = 0;
    private int _pelletEatenCount = 0;

    private Vector2 _dest = Vector2.zero;


    public int GetScore()
    {
        return _score;
    }

    public int GetPalletEatenCount()
    {
        return _pelletEatenCount;
    }

    public void Reset(Vector3 pos3D)
    {
        _pelletEatenCount = 0;
        transform.position = pos3D;
        _dest = transform.position;
    }


    void Start()
    {
        _dest = transform.position;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Oui");
        if (other.gameObject.CompareTag("Pellet"))
        {
            EatPellet(other.gameObject);
        }
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
        RaycastHit2D ray = Physics2D.Linecast(pos + dir, pos);
        bool hit = (ray.collider != GetComponent<Collider2D>());
        return hit ? ray.collider.isTrigger : true;
    }
    private void EatPellet(GameObject pellet)
    {
        int added = pellet.GetComponent<PelletBehaviour>().GetEaten();
        _score = _score + added;
        ++_pelletEatenCount;
    }
}
