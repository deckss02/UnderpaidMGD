using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public bool isPickedUp;
    private Vector2 vel;
    public float smoothTime;
    [SerializeField] GameObject Door;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedUp)
        {
            Vector3 offset = new Vector3(-0.2f,1.7f,0);
            transform.position = Vector2.SmoothDamp(transform.position, Player.transform.position + offset, ref vel, smoothTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag =="Player" && !isPickedUp)
        {
            isPickedUp = true;
            Door.GetComponent<Door>().keyPickedUp = true;
            Door.GetComponent<BoxCollider2D>().enabled = false;
        }
   }
}
