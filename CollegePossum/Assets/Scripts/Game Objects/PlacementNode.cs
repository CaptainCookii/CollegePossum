using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;

public class PlacementNode : MonoBehaviour
{
    public bool isOccupied = false;

    public void PlacePiece(GameObject piecePrefab)
    {
        if (isOccupied)
            return;

        GameObject newPiece = Instantiate(piecePrefab, transform.position, Quaternion.identity);
        PinBumper script = (PinBumper)newPiece.GetComponent("PinBumper");
        script.placementNode = this;
        isOccupied = true;
    }

    void OnEnable()
    {
        PlacementRegistry.Register(this);
    }

    void OnDisable()
    {
        PlacementRegistry.Unregister(this);
    }
}
