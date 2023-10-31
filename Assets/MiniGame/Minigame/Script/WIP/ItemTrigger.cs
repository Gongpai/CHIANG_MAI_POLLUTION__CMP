using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Stone"))
            {
                ProcessTriggerWithStone();
            }
            else if (gameObject.CompareTag("Wood"))
            {
                ProcessTriggerWithWood();
            }

            //Get the Inventory component from the player
            var inventory = other.GetComponent<Inventory>();
            //Add the collected item’s tag name to the inventory
            inventory.AddItem(gameObject.tag, 1);

            //Destroy itself
            Destroy(gameObject);
        }
    }

    protected virtual void ProcessTriggerWithStone()
    {

    }

    protected virtual void ProcessTriggerWithWood()
    {

    }
}