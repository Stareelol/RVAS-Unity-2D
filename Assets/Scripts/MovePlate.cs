using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject createdByPiece;

    GameObject board;

    int matrixX;
    int matrixY;

    public bool attack = false;

    AudioSource move;
    AudioSource capture;

    public void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        move = controller.GetComponent<AudioSource>();
        board = GameObject.FindGameObjectWithTag("Board");
        capture = board.GetComponent<AudioSource>();


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
            GameObject defender = controller.GetComponent<GameScript>().GetPosition(matrixX, matrixY);

            Destroy(defender);

            capture.Play(0);

            controller.GetComponent<GameScript>().SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

            createdByPiece.GetComponent<PieceController>().SetCoords();

            controller.GetComponent<GameScript>().SetPosition(createdByPiece);

            controller.GetComponent<GameScript>().NextTurn();

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
        }

        else
        {
            move.Play(0);

            controller.GetComponent<GameScript>().SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

            createdByPiece.GetComponent<PieceController>().SetCoords();

            controller.GetComponent<GameScript>().SetPosition(createdByPiece);

            CheckPromotion();

            //controller.GetComponent<GameScript>().NextTurn();

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
        }

    }

    public void CheckPromotion()
    {
        if (createdByPiece.name == "white_pawn" && createdByPiece.GetComponent<PieceController>().yBoard == 7)
        {
            controller.GetComponent<GameScript>().Create("white_queen", createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);
            Destroy(createdByPiece);
        }

        if (createdByPiece.name == "black_pawn" && createdByPiece.GetComponent<PieceController>().yBoard == 0)
        {
            controller.GetComponent<GameScript>().Create("black_queen", createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);
            Destroy(createdByPiece);
        }
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
