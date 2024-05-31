using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public GameObject SP1;
    public GameObject SP2;
    public GameObject SP3;

    public Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(SP1.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP2.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP3.transform.position, 0.3f);
    }
}
