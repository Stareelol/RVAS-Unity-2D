using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject createdByPiece;

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
            GameObject gm = controller.GetComponent<GameScript>().GetPosition(matrixX, matrixY);
            Destroy(gm);
        }

        controller.GetComponent<GameScript>().SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

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
        createdByPiece = obj;
    }

    public GameObject GetReference() {
        return createdByPiece;
    }

}
