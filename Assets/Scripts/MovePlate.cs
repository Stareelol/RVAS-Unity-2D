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
            // handle attacks
            GameObject defender = controller.GetComponent<GameScript>().GetPosition(matrixX, matrixY);

            defender.GetComponent<PieceController>().health -= createdByPiece.GetComponent<PieceController>().attack;
            createdByPiece.GetComponent<PieceController>().health -= defender.GetComponent<PieceController>().attack;

            if (defender.GetComponent<PieceController>().health <= 0)
            {
                Destroy(defender);

                capture.Play(0);

                if (createdByPiece.GetComponent<PieceController>().health <= 0) Destroy(createdByPiece);

                controller.GetComponent<GameScript>().SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

                createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
                createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

                createdByPiece.GetComponent<PieceController>().SetCoords();

                controller.GetComponent<GameScript>().SetPosition(createdByPiece);

                controller.GetComponent<GameScript>().NextTurn();

                createdByPiece.GetComponent<PieceController>().DestroyMovePlates();

                return;
            }
            else if (createdByPiece.GetComponent<PieceController>().health <= 0)
            {
                Destroy(createdByPiece);
                capture.Play(0);
                controller.GetComponent<GameScript>().NextTurn();
                createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
            }
            else if (createdByPiece.GetComponent<PieceController>().health <= 0 && defender.GetComponent<PieceController>().health <= 0)
            {
                Destroy(defender);
                Destroy(createdByPiece);
                capture.Play(0);
                controller.GetComponent<GameScript>().NextTurn();
                createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
            }
            else
            {
                controller.GetComponent<GameScript>().NextTurn();
                createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
                move.Play(0);
            }
        }
        else {

            move.Play(0);

            controller.GetComponent<GameScript>().SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

            createdByPiece.GetComponent<PieceController>().SetCoords();

            controller.GetComponent<GameScript>().SetPosition(createdByPiece);

            controller.GetComponent<GameScript>().NextTurn();

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
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
