using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CalculateAllMoves : MonoBehaviour
{
    private GameObject controller;

    private int xBoard = -1;
    private int yBoard = -1;

    public int moveCount = 0;

    private string first = "";
    private string second = "";

    public void Calculate(string player)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        File.WriteAllText(Application.persistentDataPath + "/moveList.txt", string.Empty);
        File.WriteAllText(Application.persistentDataPath + "/moveListReadable.txt", string.Empty);
        moveCount = 0;

        for (int i = 7; i >= 0; i--) 
            for (int j = 0; j < 8; j++)
            {
                if (gs.GetPosition(j, i) != null)
                {
                    xBoard = gs.GetPosition(j, i).GetComponent<PieceController>().xBoard;
                    yBoard = gs.GetPosition(j, i).GetComponent<PieceController>().yBoard;

                    if (player == "white" && gs.GetPosition(j, i).name.Substring(0,5) == "black") SpawnPossibleMoves(gs.GetPosition(j, i).name);
                    if (player == "black" && gs.GetPosition(j, i).name.Substring(0,5) == "white") SpawnPossibleMoves(gs.GetPosition(j, i).name);
                }
            }
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/moveList.txt")) while (reader.ReadLine() != null) moveCount++;
    }

    public void WriteToFile(string character)
    {
        string path = Application.persistentDataPath + "/moveList.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(character);
        writer.Close();

    }

    public void WriteToFileReadable(string character)
    {
        string path = Application.persistentDataPath + "/moveListReadable.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(character);
        writer.Close();
    }

    public string Convert(int x, int y)
    {

        string converted = "";

        switch (x)
        {
            case 0: converted += "a";break;
            case 1: converted += "b"; break;
            case 2: converted += "c"; break;
            case 3: converted += "d"; break;
            case 4: converted += "e"; break;
            case 5: converted += "f"; break;
            case 6: converted += "g"; break;
            case 7: converted += "h"; break;
        }

        switch (y)
        {
            case 0: converted += "1"; break;
            case 1: converted += "2"; break;
            case 2: converted += "3"; break;
            case 3: converted += "4"; break;
            case 4: converted += "5"; break;
            case 5: converted += "6"; break;
            case 6: converted += "7"; break;
            case 7: converted += "8"; break;
        }

        return converted;
    }

    public void SpawnPossibleMoves(string piece)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        string[] compare = null;
        compare = piece.Split("_");
        first = compare[0];


        string[] ep = null;

        switch (piece)
        {
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
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
                PawnMovePlate(xBoard, yBoard + 1);
                if (yBoard == 1) PawnMovePlate(xBoard, yBoard + 2); // possible only if the pawn is in the starting pos
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

    public void LineMovePlate(int xIncrement, int yIncrement)
    {

        GameScript sc = controller.GetComponent<GameScript>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        string[] compare = null;

        if (sc.PositionOnBoard(x, y)) 
            if (sc.GetPosition(x, y) != null) if (sc.GetPosition(x, y).name == "black_rook") Debug.Log(x + ", " + y);

        if (sc.PositionOnBoard(x,y))
        {
            if (sc.GetPosition(x, y) != null) compare = sc.GetPosition(x, y).name.Split("_");
            if (sc.GetPosition(x, y) != null) second = compare[0];
        }

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            WriteToFileReadable(Convert(x,y));
            WriteToFile(x + ", " + y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && first != second)
        {
            WriteToFileReadable(Convert(x,y));
            WriteToFile(x + ", " + y);
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
        GameScript sc = controller.GetComponent<GameScript>();
        string player = sc.GetCurrentPlayer();

        string[] compare = null;

        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) != null) compare = sc.GetPosition(x, y).name.Split("_");
            if (sc.GetPosition(x, y) != null) second = compare[0];
        }

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                WriteToFileReadable(Convert(x,y));
                WriteToFile(x + ", " + y);
   
            }
            else if (first!=second)
            {
                WriteToFileReadable(Convert(x,y));
                WriteToFile(x + ", " + y);
   
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                WriteToFileReadable(Convert(x,y));
                WriteToFile(x + ", " + y);
   
            }
        }
    }

    public void PawnAttackMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        string player = sc.GetCurrentPlayer();

        string[] compare = null;

        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) != null) compare = sc.GetPosition(x, y).name.Split("_");
            if (sc.GetPosition(x, y) != null) second = compare[0];
        }
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && first != second)
            {
                WriteToFileReadable(Convert(x,y));
                WriteToFile(x + ", " + y);
   
            }
        }
    }
}