using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBehaviour : MonoBehaviour
{

    public int _score = 10;

    public int GetEaten()
    {
        Destroy(this.gameObject);
        return _score;
    }

    public void Reset()
    {
        Destroy(this.gameObject);
    }
}
