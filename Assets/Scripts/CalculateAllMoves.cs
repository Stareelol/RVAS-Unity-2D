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
    public int numberOfChecks = 0;

    public bool whiteChecked = false;
    public bool blackChecked = false;

    public List<string> checkingPiece = new List<string>();

    private string first = "";
    private string second = "";

    public List<string> allMoves = new List<string>();
    public List<string> slidingMoves = new List<string>();

    int matrixX;
    int matrixY;

    public void Calculate(string player)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        File.WriteAllText(Application.persistentDataPath + "/moveList.txt", string.Empty);
        File.WriteAllText(Application.persistentDataPath + "/moveListReadable.txt", string.Empty);
        File.WriteAllText(Application.persistentDataPath + "/slidingMoves.txt", string.Empty);
        moveCount = 0;
        whiteChecked = false;
        blackChecked = false;
        checkingPiece.Clear();
        slidingMoves.Clear();

        for (int i = 7; i >= 0; i--) 
            for (int j = 0; j < 8; j++)
            {
                if (gs.GetPosition(j, i) != null)
                {
                    xBoard = gs.GetPosition(j, i).GetComponent<PieceController>().xBoard;
                    yBoard = gs.GetPosition(j, i).GetComponent<PieceController>().yBoard;

                    if (player == gs.GetPosition(j, i).name.Substring(0,5)) SpawnPossibleMoves(gs.GetPosition(j, i).name);
                }
            }
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/moveList.txt")) while (reader.ReadLine() != null) moveCount++;
    }


    public void AddMovesToList()
    {
        allMoves.Clear();
        string lines;
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/moveList.txt"))
            while ((lines = reader.ReadLine()) != null)
            {
                allMoves.Add(lines);
            }
    }

    public bool NoCheck(int x, int y)
    {
        int slidingCount = Count();

        if (slidingMoves.Count == 0) return true;
        if (slidingCount > 1) return true;

        foreach (string move in slidingMoves)
        {
            int compX = int.Parse(move.Substring(0, 1));
            int compY = int.Parse(move.Substring(1, 1));
            if (compX == x && compY == y) return false;
        }

        return true;
    }

    public int Count()
    {
        GameScript gs = this.GetComponent<GameScript>();
        int slidingCount = 0;

        foreach (string move in slidingMoves)
        {
            int compX = int.Parse(move.Substring(0, 1));
            int compY = int.Parse(move.Substring(1, 1));
            if (gs.GetPosition(compX, compY) != null && (gs.GetPosition(compX, compY).name != "white_king" && gs.GetPosition(compX, compY).name != "black_king")) slidingCount++;
        }

        return slidingCount;
    }

    public bool CheckMoveInList(int x, int y) { 
    
        foreach (string move in allMoves)
        {
            int compX = int.Parse(move.Substring(0, 1));
            int compY = int.Parse(move.Substring(1, 2));
            if (compX == x && compY == y) return true;
        }

        return false;
    }

    public bool CheckSlidingMoveInList(int x, int y)
    {
        foreach (string move in slidingMoves)
        {
            int compX = int.Parse(move.Substring(0, 1));
            int compY = int.Parse(move.Substring(1, 1));
            if (compX == x && compY == y) return true;
        }

        return false;
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

    public void WriteToFileSliding(string character)
    {
        string path = Application.persistentDataPath + "/slidingMoves.txt";
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

        numberOfChecks = checkingPiece.Count;

        //string[] ep = null;

        switch (piece)
        {
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0, piece);
                LineMovePlate(0, 1, piece);
                LineMovePlate(-1, 0, piece);
                LineMovePlate(0, -1, piece);
                //SlidingMovePlateCalculate
                SlidingMovePlateCalculate(1, 0, piece);
                SlidingMovePlateCalculate(0, 1, piece);
                SlidingMovePlateCalculate(-1, 0, piece);
                SlidingMovePlateCalculate(0, -1, piece);
                break;
            case "black_knight":
            case "white_knight":
                KnightMovePlate(piece);
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1, piece);
                LineMovePlate(1, -1, piece);
                LineMovePlate(-1, 1, piece);
                LineMovePlate(-1, -1, piece);
                //SlidingMovePlateCalculate
                SlidingMovePlateCalculate(1, 1, piece);
                SlidingMovePlateCalculate(1, -1, piece);
                SlidingMovePlateCalculate(-1, 1, piece);
                SlidingMovePlateCalculate(-1, -1, piece);
                break;
            case "black_king":
            case "white_king":
                KingMovePlate(piece);
                break;
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0, piece);
                LineMovePlate(0, 1, piece);
                LineMovePlate(1, 1, piece);
                LineMovePlate(-1, 0, piece);
                LineMovePlate(0, -1, piece);
                LineMovePlate(-1, -1, piece);
                LineMovePlate(-1, 1, piece);
                LineMovePlate(1, -1, piece);
                //SlidingMovePlateCalculate
                SlidingMovePlateCalculate(1, 0, piece);
                SlidingMovePlateCalculate(0, 1, piece);
                SlidingMovePlateCalculate(1, 1, piece);
                SlidingMovePlateCalculate(-1, 0, piece);
                SlidingMovePlateCalculate(0, -1, piece);
                SlidingMovePlateCalculate(-1, -1, piece);
                SlidingMovePlateCalculate(-1, 1, piece);
                SlidingMovePlateCalculate(1, -1, piece);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1, piece);
                if (yBoard == 6) PawnMovePlate(xBoard, yBoard - 2, piece); // possible only if the pawn is in the starting pos
                PawnAttackMovePlate(xBoard - 1, yBoard - 1, piece);
                PawnAttackMovePlate(xBoard + 1, yBoard - 1, piece);
                //CheckEnPassant();
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1, piece);
                if (yBoard == 1) PawnMovePlate(xBoard, yBoard + 2, piece); // possible only if the pawn is in the starting pos
                PawnAttackMovePlate(xBoard - 1, yBoard + 1, piece);
                PawnAttackMovePlate(xBoard + 1, yBoard + 1, piece);
                //CheckEnPassant();
                break;
        }
    }

    public void CheckEnPassant()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        string[] ep = null;
        if (gs.EnPassantSquare != " " && string.Equals(gs.EnPassantSquare, "-") == false) ep = gs.EnPassantSquare.Split(' ');
        if (ep != null)
        {
            Debug.Log(gs.GetPosition(matrixX, matrixY));
            if ((matrixX - int.Parse(ep[0])) == 0 && Mathf.Abs(matrixY - int.Parse(ep[1])) == 1)
            {
                //if (CheckEndCase(matrixX, matrixY, int.Parse(ep[0]), int.Parse(ep[1])) != true)
                //{
                //    Destroy(gs.GetPosition(int.Parse(ep[0]), int.Parse(ep[1])));
                //    gs.SetPositionEmpty(int.Parse(ep[0]), int.Parse(ep[1]));
                //}
            }
        }

    }
    public void KnightMovePlate(string piece)
    {
        PointMovePlate(xBoard + 2, yBoard + 1, piece);
        PointMovePlate(xBoard + 1, yBoard + 2, piece);
        PointMovePlate(xBoard - 1, yBoard - 2, piece);
        PointMovePlate(xBoard - 2, yBoard - 1, piece);
        PointMovePlate(xBoard + 1, yBoard - 2, piece);
        PointMovePlate(xBoard - 1, yBoard + 2, piece);
        PointMovePlate(xBoard + 2, yBoard - 1, piece);
        PointMovePlate(xBoard - 2, yBoard + 1, piece);
    }

    public void KingMovePlate(string piece)
    {

        GameScript sc = controller.GetComponent<GameScript>();

        PointMovePlate(xBoard, yBoard + 1, piece);
        PointMovePlate(xBoard, yBoard - 1, piece);
        PointMovePlate(xBoard - 1, yBoard - 1, piece);
        PointMovePlate(xBoard - 1, yBoard, piece);
        PointMovePlate(xBoard - 1, yBoard + 1, piece);
        PointMovePlate(xBoard + 1, yBoard - 1, piece);
        PointMovePlate(xBoard + 1, yBoard, piece);
        PointMovePlate(xBoard + 1, yBoard + 1, piece);

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleKingAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard + 1, yBoard) && sc.PositionOnBoard(xBoard + 2, yBoard) && sc.PositionOnBoard(xBoard + 3, yBoard))
                if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
                {
                    PointMovePlate(xBoard + 2, yBoard, piece);
                };
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleKingAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard + 1, yBoard) && sc.PositionOnBoard(xBoard + 2, yBoard) && sc.PositionOnBoard(xBoard + 3, yBoard))
                if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
                {
                    PointMovePlate(xBoard + 2, yBoard, piece);
                };
        }

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleQueenAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard - 1, yBoard) && sc.PositionOnBoard(xBoard - 2, yBoard) && sc.PositionOnBoard(xBoard - 3, yBoard) && sc.PositionOnBoard(xBoard - 4, yBoard))
                if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
                {
                    PointMovePlate(xBoard - 2, yBoard, piece);
                }
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleQueenAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard - 1, yBoard) && sc.PositionOnBoard(xBoard - 2, yBoard) && sc.PositionOnBoard(xBoard - 3, yBoard) && sc.PositionOnBoard(xBoard - 4, yBoard))
                if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
                {
                    PointMovePlate(xBoard - 2, yBoard, piece);
                }
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement, string piece)
    {

        GameScript sc = controller.GetComponent<GameScript>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        List<string> tempList = new List<string>();
        string[] compare = null;

        while (sc.PositionOnBoard(x, y))
        {
            GameObject pieceOnBoard = sc.GetPosition(x, y);

            if (pieceOnBoard != null)
            {
                compare = pieceOnBoard.name.Split("_");
                second = compare[0];
                if (second != first)
                {
                    if (sc.GetPosition(x, y).name == "white_king" || sc.GetPosition(x, y).name == "black_king")
                    {
                        if (pieceOnBoard.name == "white_king") whiteChecked = true;
                        else if ((pieceOnBoard.name == "black_king")) blackChecked = true;
                        WriteToFileReadable(piece + ": " + Convert(x, y));
                        WriteToFile(x + " " + y);                     
                        x += xIncrement;
                        y += yIncrement;
                    }
                    else
                    {
                        WriteToFileReadable(piece + ": " + Convert(x, y));
                        WriteToFile(x + " " + y);                     
                        break;
                    }
                }
                else break;
            }
            if(sc.PositionOnBoard(x, y)){
                if (pieceOnBoard == null)
                {                 
                    WriteToFileReadable(piece + ": " + Convert(x, y));
                    WriteToFile(x + " " + y);
                    x += xIncrement;
                    y += yIncrement; 
                }
            }
        }
    }

    public void SlidingMovePlateCalculate(int xIncrement, int yIncrement, string piece) {

        GameScript sc = controller.GetComponent<GameScript>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        List<string> tempList = new List<string>();

        string[] compare = null;

        while (sc.PositionOnBoard(x, y))
        {
            GameObject pieceOnBoard = sc.GetPosition(x, y);

            if (pieceOnBoard != null)
            {
                compare = pieceOnBoard.name.Split("_");
                second = compare[0];
                if (second != first)
                {
                    if (second[0] == 'w' && pieceOnBoard.name == "white_king" || second[0] == 'b' && pieceOnBoard.name == "black_king")
                    {
                        WriteToFileSliding(piece + ": " + Convert(x, y));
                        tempList.Add(x + "" + y);
                        slidingMoves.AddRange(tempList);
                        checkingPiece.Add(xBoard + "" + yBoard);
                        break;
                    }
                    else
                    {
                        tempList.Add(x + "" + y);
                        WriteToFileSliding(piece + ": " + Convert(x, y));
                        x += xIncrement;
                        y += yIncrement;
                    }
                }
                else break;
            }
            else
            {
                tempList.Add(x + "" + y);
                WriteToFileSliding(piece + ": " + Convert(x, y));
                x += xIncrement;
                y += yIncrement;
            }
        }
    }

    public void PointMovePlate(int x, int y, string piece)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        string player = sc.GetCurrentPlayer();

        string[] compare = null;

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                WriteToFileReadable(piece + ": " + Convert(x, y));
                WriteToFile(x + " " + y);
   
            }
            else if (cp != null)
            {
                if (sc.GetPosition(x, y) != null) compare = sc.GetPosition(x, y).name.Split("_");
                if (sc.GetPosition(x, y) != null) second = compare[0];
                if (second != first)
                {
                    if (sc.GetPosition(x, y).name == "white_king" || sc.GetPosition(x, y).name == "black_king") 
                    {
                        if (sc.GetPosition(x, y).name == "white_king") whiteChecked = true;
                        else if ((sc.GetPosition(x, y).name == "black_king")) blackChecked = true;
                        checkingPiece.Add(xBoard + "" + yBoard);
                    }
                    WriteToFileReadable(piece + ": " + Convert(x, y));
                    WriteToFile(x + " " + y);
                }
            }
        }
    }

    public void PawnMovePlate(int x, int y, string piece)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                WriteToFileReadable(piece + ": " + Convert(x, y));
                WriteToFile(x + " " + y);
            }
        }
    }

    public void PawnAttackMovePlate(int x, int y, string piece)
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
                if (sc.GetPosition(x, y).name == "white_king" || sc.GetPosition(x, y).name == "black_king")
                {
                    if (sc.GetPosition(x, y).name == "white_king" || sc.GetPosition(x, y).name == "black_king")
                    {
                        if (sc.GetPosition(x, y).name == "white_king") whiteChecked = true;
                        else if ((sc.GetPosition(x, y).name == "black_king")) blackChecked = true;
                        checkingPiece.Add(xBoard + "" + yBoard);
                    }
                }
                WriteToFileReadable(piece + ": " + Convert(x, y));
                WriteToFile(x + " " + y);
            }
        }
    }
}