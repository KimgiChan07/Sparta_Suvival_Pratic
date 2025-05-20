using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ItemData itemData;
    public int quantityPerHit = 1;
    public int capacy;

    public void Gather(Vector3 _hitPoint, Vector3 _hitNormal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacy <= 0)
            {
                break;
            }

            capacy -= 1;
            Instantiate(itemData.dropPrefab, _hitPoint + Vector3.up, Quaternion.LookRotation(_hitNormal, Vector3.up));
            
        }
    }
}
