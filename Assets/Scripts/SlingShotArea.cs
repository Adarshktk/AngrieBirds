using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask slingShotLayerMask;
    public bool IsWithinSlingshotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.mousePosition);
        if (Physics2D.OverlapPoint(worldPosition, slingShotLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
