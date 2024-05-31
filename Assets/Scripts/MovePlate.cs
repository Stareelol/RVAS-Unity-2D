using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject controller;

    GameObject createdByPiece = null;

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            // Change to red
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            // Ovde ide funkcionalnost za napadanje
        }

        controller.GetComponent<GameScript>().SetPositionEmpty((int) createdByPiece.GetComponent<PieceController>().xBoard, (int) createdByPiece.GetComponent<PieceController>().yBoard);

        createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
        createdByPiece.GetComponent<PieceController>().yBoard = matrixY;
        createdByPiece.GetComponent<PieceController>().SetCoords();

        controller.GetComponent<GameScript>().SetPosition(createdByPiece);

        createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        controller = obj;
    }

    public GameObject GetReference() {
        return createdByPiece;
    }

}
