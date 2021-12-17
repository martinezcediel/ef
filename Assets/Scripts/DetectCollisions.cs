using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{

    private void OnCollisionEnter(Collision otherCollider)
    {
        /*if (otherTrigger.gameObject.CompareTag("Obstacle"))
        {
            Destroy(otherTrigger.gameObject);
            Destroy(gameObject);
        }*/
        if (otherCollider.gameObject.CompareTag("Obstacle"))
        {
            Destroy(otherCollider.gameObject);
            Destroy(gameObject);
        }
    }
}
