using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public float xBoard = -1f;
    public float yBoard = -1f;

    private string player;

    public Sprite black_king, black_knight, black_pawn;
    public Sprite white_king, white_knight, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name) 
        {
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1.4f;
        y *= 1.2f;

        x += -7.2f;
        y += -4.2f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();
        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates() {
        switch (this.name)
        {
            case "black_knight":
            case "white_knight":
                KnightMovePlate();
                break;
            case "black_king":
            case "white_king":
                KingMovePlate();
                break;
            case "black_pawn":
                PawnMovePlate((int) xBoard, (int) yBoard - 2);
                PawnMovePlate((int)xBoard + 1, (int)yBoard - 2);
                PawnMovePlate((int)xBoard - 1, (int)yBoard - 2);
                break;
            case "white_pawn":
                PawnMovePlate((int)xBoard, (int) yBoard + 1);
                PawnMovePlate((int)xBoard + 1, (int)yBoard + 1);
                PawnMovePlate((int)xBoard - 1, (int)yBoard + 1);
                break;
        }
    }

    public void KnightMovePlate() 
    {
        PointMovePlate((int)xBoard + 2, (int)yBoard + 1);
        PointMovePlate((int)xBoard + 1, (int)yBoard + 2);
        PointMovePlate((int)xBoard - 1, (int)yBoard - 3);
        PointMovePlate((int)xBoard - 2, (int)yBoard - 2);
        PointMovePlate((int)xBoard + 1, (int)yBoard - 3);
        PointMovePlate((int)xBoard - 1, (int)yBoard + 2);
        PointMovePlate((int)xBoard + 2, (int)yBoard - 2);
        PointMovePlate((int)xBoard - 2, (int)yBoard + 1);
    }

    public void KingMovePlate()
    {
        PointMovePlate((int)xBoard, (int)yBoard + 1);
        PointMovePlate((int)xBoard, (int)yBoard - 2);
        PointMovePlate((int)xBoard - 1, (int)yBoard - 2);
        PointMovePlate((int)xBoard - 1, (int)yBoard);
        PointMovePlate((int)xBoard - 1, (int)yBoard + 1);
        PointMovePlate((int)xBoard + 1, (int)yBoard - 2);
        PointMovePlate((int)xBoard + 1, (int)yBoard);
        PointMovePlate((int)xBoard + 1, (int)yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        GameScript sc  = controller.GetComponent<GameScript>();
        if (sc.PositionOnBoard(x, y)) {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null) 
            {
                MovePlateSpawn(x, y);    
            } else if (cp.GetComponent<PieceController>().player != player) 
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x+1,y) && sc.GetPosition(x+1,y) != null && sc.GetPosition(x+1,y).GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {

        float x = matrixX;
        float y = matrixY;

        x *= 1.4f;
        x += -7.2f;

        switch (y)
        {
            case 0: x = -4.2f; break;
            case 1: y = -2.8f; break;
            case 2: y = -1.4f; break;
            case 3: y = 0f; break;
            case 4: y = 1.4f; break;
            case 5: y = 2.8f; break;
            case 6: y = 4.2f; break;
        }

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        NewBehaviourScript mpScript = mp.GetComponent<NewBehaviourScript>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 1f;
        y *= 1f;

        switch (x)
        {
            case 0f: x = -7.2f; break;
            case 1f: x = -5.8f; break;
            case 2f: x = -4.4f; break;
            case 3f: x = -2.95f; break;
            case 4f: x = -1.55f; break;
            case 5f: x = -0.15f; break;
            case 6f: x = 1.25f; break;
        }

        if (y == 0) y += -4.25f;
        if (y == 7) y += -2.85f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        NewBehaviourScript mpScript = mp.GetComponent<NewBehaviourScript>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
