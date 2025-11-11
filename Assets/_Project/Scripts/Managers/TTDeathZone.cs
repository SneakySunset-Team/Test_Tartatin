using System;
using UnityEngine;

public class TTDeathZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ennemy")
        {
            TTRunManager.Instance.GameOver();
        }
    }
}
