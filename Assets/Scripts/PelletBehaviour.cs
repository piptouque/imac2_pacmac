using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletBehaviour : MonoBehaviour
{

    public int _score = 10;
    private static int PELLET_COUNT = 0;

    void Start()
    {
        ++PELLET_COUNT;
    }

    public static int GetPelletCount()
    {
        return PELLET_COUNT;
    }

    public static bool AreAllPelletsEaten()
    {
        return PELLET_COUNT == 0;
    }

    public int GetEaten()
    {
        Destroy(this.gameObject);
        return _score;
    }
}
