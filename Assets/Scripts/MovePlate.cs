using UnityEngine;
using System.IO;
using System.Collections;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject createdByPiece;

    GameObject board;

    GameObject canvas;
    GameObject clock;

    int matrixX;
    int matrixY;

    public int promotedPieceX;
    public int promotedPieceY;

    public bool attack = false;

    AudioSource move;
    AudioSource capture;

    public void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        move = controller.GetComponent<AudioSource>();
        board = GameObject.FindGameObjectWithTag("Board");
        capture = board.GetComponent<AudioSource>();
        clock = GameObject.FindGameObjectWithTag("Clock");


        if (attack)
        {
            this.GetComponent<SpriteRenderer>().material.color = new Color(220, 0, 0) {
                a = 0.9f
            };
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        CalculateAllDMoves cadm = controller.GetComponent<CalculateAllDMoves>();

        if (gs.startClockWhite && gs.startClockBlack) clock.GetComponent<TimerScript>().AddSecondsToClock(gs.GetCurrentPlayer());

        if (gs.GetCurrentPlayer() == "white" && gs.startClockWhite == false) gs.startClockWhite = true;
        if (gs.GetCurrentPlayer() == "black" && gs.startClockWhite && gs.startClockBlack == false) gs.startClockBlack = true;


        if (attack)
        {
            GameObject defender = gs.GetPosition(matrixX, matrixY);

            Destroy(defender);

            capture.Play(0);

            gs.SetPositionEmpty(createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);

            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

            createdByPiece.GetComponent<PieceController>().SetCoords();

            gs.SetPosition(createdByPiece);

            CheckPromotion(gs.GetCurrentPlayer());

            cam.Calculate(gs.GetCurrentPlayer());

            gs.NextTurn();
            RemoveCastlingIfChecked(gs.GetCurrentPlayer());
            cadm.Calculate(gs.GetCurrentPlayer());
            cadm.IsCheckmate(gs.GetCurrentPlayer());

            if (cam.blackChecked == false && gs.GetCurrentPlayer() == "white") if (GameObject.FindWithTag("black_check") != null) Destroy(GameObject.FindWithTag("black_check"));
            if (cam.whiteChecked == false && gs.GetCurrentPlayer() == "black") if (GameObject.FindWithTag("white_check") != null) Destroy(GameObject.FindWithTag("white_check"));

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
            createdByPiece.GetComponent<PieceController>().DestroyClick();
        }

        else
        {
            move.Play(0);

            gs.SetPositionEmpty(createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);
            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;
            createdByPiece.GetComponent<PieceController>().SetCoords();
            gs.SetPosition(createdByPiece);

            CheckPromotion(gs.GetCurrentPlayer());
            CheckCastlingShort(createdByPiece.GetComponent<PieceController>().name);
            CheckCastlingLong(createdByPiece.GetComponent<PieceController>().name);

            cam.Calculate(gs.GetCurrentPlayer());

            gs.NextTurn();

            cadm.Calculate(gs.GetCurrentPlayer());
            cadm.IsCheckmate(gs.GetCurrentPlayer());
            RemoveCastlingIfChecked(gs.GetCurrentPlayer());

            if (cam.blackChecked == false && gs.GetCurrentPlayer() == "white") if (GameObject.FindWithTag("black_check") != null) Destroy(GameObject.FindWithTag("black_check"));
            if (cam.whiteChecked == false && gs.GetCurrentPlayer() == "black") if (GameObject.FindWithTag("white_check") != null) Destroy(GameObject.FindWithTag("white_check"));

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
            createdByPiece.GetComponent<PieceController>().DestroyClick();
        }

    }

    public void RemoveCastlingIfChecked(string player)
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        GameScript gs = controller.GetComponent<GameScript>();

        GameObject black_king = GameObject.Find("black_king");
        GameObject white_king = GameObject.Find("white_king");


        if (cam.blackChecked == true && player == "black")
        {
            controller.GetComponent<GameScript>().BlackCastleKingAllowed = false;
            controller.GetComponent<GameScript>().BlackCastleQueenAllowed = false;
            GameObject spawn = Instantiate(gs.checkBG, new Vector3(black_king.transform.position.x, black_king.transform.position.y, -1), Quaternion.identity);
            spawn.tag = "black_check";
            
        }
        if (cam.whiteChecked == true && player == "white")
        {
            controller.GetComponent<GameScript>().WhiteCastleKingAllowed = false;
            controller.GetComponent<GameScript>().WhiteCastleQueenAllowed = false;
            GameObject spawn = Instantiate(gs.checkBG, new Vector3(white_king.transform.position.x, white_king.transform.position.y, -1), Quaternion.identity);
            spawn.tag = "white_check";
        }
    }

    public void CheckPromotion(string player) 
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameObject canvas_object = GameObject.FindGameObjectWithTag("Canvas");
        CanvasScript cs = canvas_object.GetComponent<CanvasScript>();

        if (player == "white" && createdByPiece.name == "white_pawn" && createdByPiece.GetComponent<PieceController>().yBoard == 7) cs.ShowPromoteScreen(player, createdByPiece);
        if (player == "black" && createdByPiece.name == "black_pawn" && createdByPiece.GetComponent<PieceController>().yBoard == 0) cs.ShowPromoteScreen(player, createdByPiece);
    }

    public void CheckCastlingShort(string piece) 
    {
        GameScript gs = controller.GetComponent<GameScript>();
        switch (piece)
        {
            case "white_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 6 && createdByPiece.GetComponent<PieceController>().yBoard == 0 && controller.GetComponent<GameScript>().GetPosition(7, 0) != null && controller.GetComponent<GameScript>().GetPosition(7, 0).name == "white_rook" && gs.WhiteCastleKingAllowed == true) CastleWhiteShort(); ; break;
            case "black_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 6 && createdByPiece.GetComponent<PieceController>().yBoard == 7 && controller.GetComponent<GameScript>().GetPosition(7, 7) != null && controller.GetComponent<GameScript>().GetPosition(7, 7).name == "black_rook" && gs.BlackCastleKingAllowed == true) CastleBlackShort(); ; break;
        }      
    }

    public void CheckCastlingLong(string piece)
    {
        GameScript gs = controller.GetComponent<GameScript>();
        switch (piece)
        {
            case "white_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 2 && createdByPiece.GetComponent<PieceController>().yBoard == 0 && controller.GetComponent<GameScript>().GetPosition(0, 0) != null && controller.GetComponent<GameScript>().GetPosition(0, 0).name == "white_rook" && gs.WhiteCastleQueenAllowed == true) CastleWhiteLong(); ; break;
            case "black_king": if (createdByPiece.GetComponent<PieceController>().xBoard == 2 && createdByPiece.GetComponent<PieceController>().yBoard == 7 && controller.GetComponent<GameScript>().GetPosition(0, 7) != null && controller.GetComponent<GameScript>().GetPosition(0, 7).name == "black_rook" && gs.WhiteCastleQueenAllowed == true) CastleBlackLong(); ; break;
        }
    }

    public void CastleWhiteShort()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(7, 0));
        controller.GetComponent<GameScript>().SetPositionEmpty(7,0);
        GameObject created = controller.GetComponent<GameScript>().Create("white_rook", 5,0);
        controller.GetComponent<GameScript>().SetPosition(created);
        controller.GetComponent<GameScript>().WhiteCastleKingAllowed = false;
        controller.GetComponent<GameScript>().WhiteCastleQueenAllowed = false;
    }

    public void CastleBlackShort()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(7, 7));
        controller.GetComponent<GameScript>().SetPositionEmpty(7, 7);
        GameObject created = controller.GetComponent<GameScript>().Create("black_rook", 5, 7);
        controller.GetComponent<GameScript>().SetPosition(created);
        controller.GetComponent<GameScript>().BlackCastleKingAllowed = false;
        controller.GetComponent<GameScript>().BlackCastleQueenAllowed = false;
    }

    public void CastleWhiteLong()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(0, 0));
        controller.GetComponent<GameScript>().SetPositionEmpty(0, 0);
        GameObject createdRook = controller.GetComponent<GameScript>().Create("white_rook", 3, 0);
        controller.GetComponent<GameScript>().SetPosition(createdRook);
        controller.GetComponent<GameScript>().WhiteCastleKingAllowed = false;
        controller.GetComponent<GameScript>().WhiteCastleQueenAllowed = false;
    }

    public void CastleBlackLong()
    {
        Destroy(controller.GetComponent<GameScript>().GetPosition(0, 7));
        controller.GetComponent<GameScript>().SetPositionEmpty(0, 7);
        GameObject createdRook = controller.GetComponent<GameScript>().Create("black_rook", 3, 7);
        controller.GetComponent<GameScript>().SetPosition(createdRook);
        controller.GetComponent<GameScript>().BlackCastleKingAllowed = false;
        controller.GetComponent<GameScript>().BlackCastleQueenAllowed = false;
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
