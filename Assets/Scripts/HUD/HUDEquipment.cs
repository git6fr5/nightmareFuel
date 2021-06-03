using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDEquipment : MonoBehaviour
{
    public HUDEquipmentSlot[] hudEquipmentSlots;

    public void SetEquipment(List<Equipable> equipment)
    {
        for (int i = 0; i < hudEquipmentSlots.Length; i++)
        {
            if (i < equipment.Count)
            {
                hudEquipmentSlots[i].image.sprite = equipment[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                hudEquipmentSlots[i].image.sprite = null;
            }
        }
    }
}
