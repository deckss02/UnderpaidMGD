using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoints : MonoBehaviour
{
    public GameObject FP1;
    public GameObject FP2;
    public GameObject FP3;
    public GameObject FP4;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(FP1.transform.position, 0.5f);
        Gizmos.DrawWireSphere(FP2.transform.position, 0.5f);
        Gizmos.DrawWireSphere(FP3.transform.position, 0.5f);
        Gizmos.DrawWireSphere(FP4.transform.position, 0.5f);
    }
}
