using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PieceController : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public int xBoard = -1;
    public int yBoard = -1;

    private string player;

    public Sprite black_king, black_knight, black_pawn, black_bishop, black_queen, black_rook;
    public Sprite white_king, white_knight, white_pawn, white_bishop, white_queen, white_rook;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            // black pieces
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop;player = "black"; break;
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;

            // white pieces
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1f;
        y *= 1f;

        x *= 1.235f;
        x += -7.3f;

        switch (y)
        {
            case 0: y = -4.4f; break; // -4.4
            case 1: y = -3.15f; break; // -3.15
            case 2: y = -1.9f; break; // -1.9
            case 3: y = -0.65f; break; // -0.65
            case 4: y = 0.6f; break; // -0.65
            case 5: y = 1.8f; break; // 1.8
            case 6: y = 3.1f; break; // 3.1
            case 7: y = 4.3f; break; // 4.3
        }

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public void GenerateLegalMoves()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        //List<GameObject> gameObjects = new List<GameObject>();

        for (int i = 0; i < 7; i++)
            for (int j = 0; j < 7; j++)
            {
                if (gs.GetPosition(i,j) != null)
                {
                    //
                }
            }
    }

    private void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if( controller.GetComponent<GameScript>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates();
        }
        
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

        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        string[] ep = null;

        switch (this.name)
        {
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(- 1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_knight":
            case "white_knight":
                KnightMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                KingMovePlate();
                break;
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                if (yBoard == 6) PawnMovePlate(xBoard, yBoard - 2); // possible only if the pawn is in the starting pos
                PawnAttackMovePlate(xBoard - 1, yBoard - 1);
                PawnAttackMovePlate(xBoard + 1, yBoard - 1);
                ep = null;
                if (gs.EnPassantSquare != " " && string.Equals(gs.EnPassantSquare, "-") == false) ep = gs.EnPassantSquare.Split(' ');
                if (ep != null)
                {
                    int epX = int.Parse(ep[0]);
                    int epY = int.Parse(ep[1]);
                    if (epY == yBoard)
                    {
                        if ((epX - xBoard) == -1) PawnMovePlate(xBoard - 1, yBoard - 1);
                        else if ((epX - xBoard) == 1) PawnMovePlate(xBoard + 1, yBoard - 1);
                    }
                }
                break;
            case "white_pawn":
                PawnMovePlate(xBoard,  yBoard + 1);
                if (yBoard == 1) PawnMovePlate(xBoard,  yBoard + 2); // possible only if the pawn is in the starting pos
                PawnAttackMovePlate(xBoard - 1, yBoard + 1);
                PawnAttackMovePlate(xBoard + 1, yBoard + 1);
                ep = null;
                if (gs.EnPassantSquare != " " && string.Equals(gs.EnPassantSquare, "-") == false) ep = gs.EnPassantSquare.Split(' ');
                if (ep != null)
                {
                    int epX = int.Parse(ep[0]);
                    int epY = int.Parse(ep[1]);
                    if (epY == yBoard)
                    {
                        if ((epX - xBoard) == -1) PawnMovePlate(xBoard - 1, yBoard + 1);
                        else if ((epX - xBoard) == 1) PawnMovePlate(xBoard + 1, yBoard + 1);
                    }
                }
                break;
        }
    }

    public void LineMovePlate (int xIncrement, int yIncrement)
    {
        GameScript sc = controller.GetComponent<GameScript>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x,y) && sc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<PieceController>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void KnightMovePlate() 
    {
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
    }

    public void KingMovePlate()
    {

        GameScript sc = controller.GetComponent<GameScript>();

        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleKingAllowed == true)
        {
            if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
            {
                PointMovePlate(xBoard + 2, yBoard);
            };
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleKingAllowed == true)
        {
            if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
            {
                PointMovePlate(xBoard + 2, yBoard);
            };
        }

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleQueenAllowed == true)
        {
            if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
            {
                PointMovePlate(xBoard - 2, yBoard);
            }
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleQueenAllowed == true)
        {
            if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
            {
                PointMovePlate(xBoard - 2, yBoard);
            }
        }
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
        }
    }

    public void PawnAttackMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {

        float x = matrixX;
        float y = matrixY;

        x *= 1f;
        y *= 1f;

        x *= 1.235f;
        x += -7.3f;

        switch (y)
        {
            case 0: y = -4.4f; break; // -4.4
            case 1: y = -3.15f; break; // -3.15
            case 2: y = -1.9f; break; // -1.9
            case 3: y = -0.65f; break; // -0.65
            case 4: y = 0.6f; break; // -0.65
            case 5: y = 1.8f; break; // 1.8
            case 6: y = 3.1f; break; // 3.1
            case 7: y = 4.3f; break; // 4.3
        }

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {

        float x = matrixX;
        float y = matrixY;

        x *= 1f;
        y *= 1f;

        x *= 1.235f;
        x += -7.3f;

        switch (y)
        {
            case 0: y = -4.4f; break; // -4.4
            case 1: y = -3.15f; break; // -3.15
            case 2: y = -1.9f; break; // -1.9
            case 3: y = -0.65f; break; // -0.65
            case 4: y = 0.6f; break; // -0.65
            case 5: y = 1.8f; break; // 1.8
            case 6: y = 3.1f; break; // 3.1
            case 7: y = 4.3f; break; // 4.3
        }

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        mp.GetComponent<Renderer>().material.color = new Color(255, 0, 0);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
