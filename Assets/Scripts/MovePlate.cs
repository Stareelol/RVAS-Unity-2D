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
            CheckCastlingShort(createdByPiece.GetComponent<PieceController>().name);
            CheckCastlingLong(createdByPiece.GetComponent<PieceController>().name);

            controller.GetComponent<GameScript>().NextTurn();

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
        }

    }

    public void CheckPromotion() // PROMOTION DOES NOT WORK BECAUSE THERE IS NO SET POS
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
    public void CheckCastlingShort(string piece) 
    {
        switch (piece)
        {
            case "white_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 6 && createdByPiece.GetComponent<PieceController>().yBoard == 0 && controller.GetComponent<GameScript>().GetPosition(7, 0).name == "white_rook") CastleWhiteShort(); ; break;
            case "black_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 6 && createdByPiece.GetComponent<PieceController>().yBoard == 7 && controller.GetComponent<GameScript>().GetPosition(7, 7).name == "black_rook") CastleBlackShort(); ; break;
        }      
    }

    public void CheckCastlingLong(string piece)
    {
        switch (piece)
        {
            case "white_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 2 && createdByPiece.GetComponent<PieceController>().yBoard == 0 && controller.GetComponent<GameScript>().GetPosition(0, 0).name == "white_rook") CastleWhiteLong(); ; break;
            case "black_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 2 && createdByPiece.GetComponent<PieceController>().yBoard == 7 && controller.GetComponent<GameScript>().GetPosition(0, 7).name == "black_rook") CastleBlackLong(); ; break;
        }
    }

    public void CastleWhiteShort()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(7, 0));
        controller.GetComponent<GameScript>().SetPositionEmpty(7,0);
        GameObject created = controller.GetComponent<GameScript>().Create("white_rook", 5,0);
        controller.GetComponent<GameScript>().SetPosition(created);
    }

    public void CastleBlackShort()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(7, 7));
        controller.GetComponent<GameScript>().SetPositionEmpty(7, 7);
        GameObject created = controller.GetComponent<GameScript>().Create("black_rook", 5, 7);
        controller.GetComponent<GameScript>().SetPosition(created);
    }

    public void CastleWhiteLong()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(0, 0));
        controller.GetComponent<GameScript>().SetPositionEmpty(0, 0);
        GameObject createdRook = controller.GetComponent<GameScript>().Create("white_rook", 3, 0);
        controller.GetComponent<GameScript>().SetPosition(createdRook);
    }

    public void CastleBlackLong()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(0, 7));
        controller.GetComponent<GameScript>().SetPositionEmpty(0, 7);
        GameObject createdRook = controller.GetComponent<GameScript>().Create("black_rook", 3, 7);
        controller.GetComponent<GameScript>().SetPosition(createdRook);
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
