using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CalculateAllDMoves : MonoBehaviour
{
    private GameObject controller;

    private int xBoard = -1;
    private int yBoard = -1;

    public List<string> allDefensiveMoves = new List<string>();
    public List<string> kingDefensiveMoves = new List<string>();

    public string player;


    public void Calculate(string player)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        allDefensiveMoves.Clear();
        kingDefensiveMoves.Clear();

        for (int i = 7; i >= 0; i--)
            for (int j = 0; j < 8; j++)
            {
                if (gs.GetPosition(j, i) != null)
                {
                    xBoard = gs.GetPosition(j, i).GetComponent<PieceController>().xBoard;
                    yBoard = gs.GetPosition(j, i).GetComponent<PieceController>().yBoard;

                    

                    if (player == gs.GetPosition(j, i).name.Substring(0, 5)) SpawnDefensiveMoves(gs.GetPosition(j, i).name); 
                }
            }
    }

    public bool IsCheckmate(string player)
    {

        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();

        switch (player)
        {
            case "black": 
                { 
                   if (cam.blackChecked == true && allDefensiveMoves.Count == 0 && kingDefensiveMoves.Count == 0) Debug.Log("white checkmate");
                    if (cam.blackChecked == false && allDefensiveMoves.Count == 0 && kingDefensiveMoves.Count == 0) Debug.Log("stalemate"); 
                }; 
                break;
            case "white": { 
                    if (cam.whiteChecked == true && allDefensiveMoves.Count == 0 && kingDefensiveMoves.Count == 0) Debug.Log("black checkmate");
                    if (cam.whiteChecked == false && allDefensiveMoves.Count == 0 && kingDefensiveMoves.Count == 0) Debug.Log("stalemate");
                }; 
                break;
        }

        return false;
    }

    public void SpawnDefensiveMoves(string piece)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();

        switch (piece)
        {
            case "black_rook":
            case "white_rook":
                if (cam.numberOfChecks <= 1)
                {
                    LineMovePlate(1, 0);
                    LineMovePlate(0, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(0, -1);
                }
                break;
            case "black_knight":
            case "white_knight":
                if (cam.numberOfChecks <= 1) KnightMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                if (cam.numberOfChecks <= 1)
                {
                    LineMovePlate(1, 1);
                    LineMovePlate(1, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(-1, -1);
                }
                break;
            case "black_king":
            case "white_king":
                KingMovePlate();
                break;
            case "black_queen":
            case "white_queen":
                if (cam.numberOfChecks <= 1)
                {
                    LineMovePlate(1, 0);
                    LineMovePlate(0, 1);
                    LineMovePlate(1, 1);
                    LineMovePlate(-1, 0);
                    LineMovePlate(0, -1);
                    LineMovePlate(-1, -1);
                    LineMovePlate(-1, 1);
                    LineMovePlate(1, -1);
                }
                break;
            case "black_pawn":
                if (cam.numberOfChecks <= 1)
                {
                    PawnMovePlate(xBoard, yBoard - 1);
                    if (yBoard == 6) PawnMovePlate(xBoard, yBoard - 2); // possible only if the pawn is in the starting pos
                    PawnAttackMovePlate(xBoard - 1, yBoard - 1);
                    PawnAttackMovePlate(xBoard + 1, yBoard - 1);
                    //EnPassantMovePlate();
                }
                break;
            case "white_pawn":
                if (cam.numberOfChecks <= 1)
                {
                    PawnMovePlate(xBoard, yBoard + 1);
                    if (yBoard == 1) PawnMovePlate(xBoard, yBoard + 2); // possible only if the pawn is in the starting pos
                    PawnAttackMovePlate(xBoard - 1, yBoard + 1);
                    PawnAttackMovePlate(xBoard + 1, yBoard + 1);
                    //EnPassantMovePlate();
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
            {
                if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (cam.CheckSlidingMoveInList(x, y)) allDefensiveMoves.Add(x + " " + y);
            }
            else
            {
                if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
            }
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<PieceController>().player != player)
        {
            if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
            {
                if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (x == int.Parse(cam.checkingPiece[0].Substring(0, 1)) && y == int.Parse(cam.checkingPiece[0].Substring(1, 1))) allDefensiveMoves.Add(x + " " + y);
            }
            else
            {
                if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
            }
        }
    }

    public void PointMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        player = sc.GetCurrentPlayer();

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (cam.CheckSlidingMoveInList(x, y)) allDefensiveMoves.Add(x + " " + y);
                }
                else
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
                }

            }

            else if (cp != null && cp.GetComponent<PieceController>().player != player)
            {
                if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (x == int.Parse(cam.checkingPiece[0].Substring(0, 1)) && y == int.Parse(cam.checkingPiece[0].Substring(1, 1))) allDefensiveMoves.Add(x + " " + y);
                }
                else
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
                }
            }
        }
    }

    public void PointMovePlateKing(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();

        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                kingDefensiveMoves.Add(x + " " + y);
            }

            else if (cp.GetComponent<PieceController>().player != player)
            {
                if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
                {
                    if ((cam.CheckSlidingMoveInList(x, y))) if (x == int.Parse(cam.checkingPiece[0].Substring(0, 1)) && y == int.Parse(cam.checkingPiece[0].Substring(1, 1))) kingDefensiveMoves.Add(x + " " + y);
                }
                else
                {
                    kingDefensiveMoves.Add(x + " " + y);
                }
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (cam.CheckSlidingMoveInList(x, y)) allDefensiveMoves.Add(x + " " + y);
                }
                else
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
                }
            }
        }
    }

    public void PawnAttackMovePlate(int x, int y)
    {
        GameScript sc = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<PieceController>().player != player)
            {
                if ((sc.GetCurrentPlayer() == "black" && cam.blackChecked == true) || (sc.GetCurrentPlayer() == "white" && cam.whiteChecked == true))
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) if (x == int.Parse(cam.checkingPiece[0].Substring(0, 1)) && y == int.Parse(cam.checkingPiece[0].Substring(1, 1))) allDefensiveMoves.Add(x + " " + y);
                }
                else
                {
                    if (cam.canMoveWithoutCheck(xBoard, yBoard)) allDefensiveMoves.Add(x + " " + y);
                }
            }
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
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        player = sc.GetCurrentPlayer();

        if (cam.CheckMoveInList(xBoard, yBoard + 1) == false) PointMovePlateKing(xBoard, yBoard + 1);
        if (cam.CheckMoveInList(xBoard, yBoard - 1) == false) PointMovePlateKing(xBoard, yBoard - 1);
        if (cam.CheckMoveInList(xBoard - 1, yBoard - 1) == false) PointMovePlateKing(xBoard - 1, yBoard - 1);
        if (cam.CheckMoveInList(xBoard - 1, yBoard) == false) PointMovePlateKing(xBoard - 1, yBoard);
        if (cam.CheckMoveInList(xBoard - 1, yBoard + 1) == false) PointMovePlateKing(xBoard - 1, yBoard + 1);
        if (cam.CheckMoveInList(xBoard + 1, yBoard - 1) == false) PointMovePlateKing(xBoard + 1, yBoard - 1);
        if (cam.CheckMoveInList(xBoard + 1, yBoard) == false) PointMovePlateKing(xBoard + 1, yBoard);
        if (cam.CheckMoveInList(xBoard + 1, yBoard + 1) == false) PointMovePlateKing(xBoard + 1, yBoard + 1);

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleKingAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard + 1, yBoard) && sc.PositionOnBoard(xBoard + 2, yBoard) && sc.PositionOnBoard(xBoard + 3, yBoard))
                if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
                {
                    if (cam.CheckMoveInList(xBoard + 2, yBoard) == false) PointMovePlateKing(xBoard + 2, yBoard);
                };
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleKingAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard + 1, yBoard) && sc.PositionOnBoard(xBoard + 2, yBoard) && sc.PositionOnBoard(xBoard + 3, yBoard))
                if (sc.GetPosition(xBoard + 1, yBoard) == null && sc.GetPosition(xBoard + 2, yBoard) == null && sc.GetPosition(xBoard + 3, yBoard) != null && (sc.GetPosition(xBoard + 3, yBoard).name == "white_rook" || sc.GetPosition(xBoard + 3, yBoard).name == "black_rook"))
                {
                    if (cam.CheckMoveInList(xBoard + 2, yBoard) == false) PointMovePlateKing(xBoard + 2, yBoard);
                };
        }

        if (sc.GetCurrentPlayer() == "white" && sc.WhiteCastleQueenAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard - 1, yBoard) && sc.PositionOnBoard(xBoard - 2, yBoard) && sc.PositionOnBoard(xBoard - 3, yBoard) && sc.PositionOnBoard(xBoard - 4, yBoard))
                if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
                {
                    if (cam.CheckMoveInList(xBoard - 2, yBoard) == false) PointMovePlateKing(xBoard - 2, yBoard);
                }
        }

        if (sc.GetCurrentPlayer() == "black" && sc.BlackCastleQueenAllowed == true)
        {
            if (sc.PositionOnBoard(xBoard - 1, yBoard) && sc.PositionOnBoard(xBoard - 2, yBoard) && sc.PositionOnBoard(xBoard - 3, yBoard) && sc.PositionOnBoard(xBoard - 4, yBoard))
                if (sc.GetPosition(xBoard - 1, yBoard) == null && sc.GetPosition(xBoard - 2, yBoard) == null && sc.GetPosition(xBoard - 3, yBoard) == null && (sc.GetPosition(xBoard - 4, yBoard).name == "white_rook" || sc.GetPosition(xBoard - 4, yBoard).name == "black_rook"))
                {
                    if (cam.CheckMoveInList(xBoard - 2, yBoard) == false) PointMovePlateKing(xBoard - 2, yBoard);
                }
        }
    }


}
