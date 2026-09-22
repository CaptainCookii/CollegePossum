using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject playerPiecePrefab;
    public int piecesToPlacePerTurn = 3;
    private int piecesPlacedThisTurn = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPlacePiece();
        }
    }

    void TryPlacePiece()
    {
        if (TurnManager.Instance.currentTurn != TurnState.PlayerPlacing)
            return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit == null)
            return;

        PlacementNode node = hit.GetComponent<PlacementNode>();

        if (node != null && !node.isOccupied)
        {
            GameObject piecePrefab = TurnManager.Instance.GetCurrentPiecePrefab();
            node.PlacePiece(piecePrefab);

            piecesPlacedThisTurn++;

            if (piecesPlacedThisTurn >= piecesToPlacePerTurn)
            {
                piecesPlacedThisTurn = 0;
                TurnManager.Instance.BeginBallPhase();
            }
        }
    }
}

