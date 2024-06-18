using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public GameObject SP1;
    public GameObject SP2;
    public GameObject SP3;
    public GameObject SP4;
    public GameObject SP5;
    public GameObject SP6;
    public GameObject SP7;
    public GameObject SP8;
    public GameObject SP9;

    public Color gizmoColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawWireSphere(SP1.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP2.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP3.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP4.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP5.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP6.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP7.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP8.transform.position, 0.3f);
        Gizmos.DrawWireSphere(SP9.transform.position, 0.3f);
    }
}
