using UnityEngine;
using System.IO;
using System.Collections;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject createdByPiece;

    GameObject board;

    GameObject canvas;

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


        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        CalculateAllMoves cam = controller.GetComponent<CalculateAllMoves>();
        CalculateAllDMoves cadm = controller.GetComponent<CalculateAllDMoves>();

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
            cam.AddMovesToList();

            if (cam.blackChecked == false && gs.GetCurrentPlayer() == "black") if (GameObject.FindWithTag("black_check") != null) Destroy(GameObject.FindWithTag("black_check"));
            if (cam.whiteChecked == false && gs.GetCurrentPlayer() == "white") if (GameObject.FindWithTag("white_check") != null) Destroy(GameObject.FindWithTag("white_check"));

            gs.NextTurn();
            RemoveCastlingIfChecked(gs.GetCurrentPlayer());
            cadm.Calculate(gs.GetCurrentPlayer());
            cadm.IsCheckmate(gs.GetCurrentPlayer());
            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
        }

        else
        {
            move.Play(0);

            //if (createdByPiece.GetComponent<PieceController>().name == "white_pawn" || createdByPiece.GetComponent<PieceController>().name == "black_pawn")
            //{
            //    SetEnPassantSquare();
            //    CheckEnPassant();
            //}
            //else gs.GetComponent<GameScript>().EnPassantSquare = " ";

            gs.SetPositionEmpty(createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);
            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;
            createdByPiece.GetComponent<PieceController>().SetCoords();
            gs.SetPosition(createdByPiece);

            //CheckPromotion(gs.GetCurrentPlayer());

            //CheckCastlingShort(createdByPiece.GetComponent<PieceController>().name);
            //CheckCastlingLong(createdByPiece.GetComponent<PieceController>().name);

            //cam.Calculate(gs.GetCurrentPlayer());
            //cam.AddMovesToList();

            //if (cam.blackChecked == false && gs.GetCurrentPlayer() == "black") if (GameObject.FindWithTag("black_check") != null) Destroy(GameObject.FindWithTag("black_check"));
            //if (cam.whiteChecked == false && gs.GetCurrentPlayer() == "white") if (GameObject.FindWithTag("white_check") != null) Destroy(GameObject.FindWithTag("white_check"));

            //gs.NextTurn();

            //cadm.Calculate(gs.GetCurrentPlayer());
            //cadm.IsCheckmate(gs.GetCurrentPlayer());
            //RemoveCastlingIfChecked(gs.GetCurrentPlayer());

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates(); 
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
                if (CheckEndCase(matrixX, matrixY, int.Parse(ep[0]), int.Parse(ep[1])) != true)
                {
                    Destroy(gs.GetPosition(int.Parse(ep[0]), int.Parse(ep[1])));
                    gs.SetPositionEmpty(int.Parse(ep[0]), int.Parse(ep[1]));
                }
            }
        }

    }

    public bool CheckEndCase(int playX, int playY, int compX, int compY) {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
        //Debug.Log(playX + ", " + playY + ", " + compX + ", " + compY);
        Debug.Log(gs.GetPosition(playX, playY));
        Debug.Log(gs.GetPosition(compX, compY));
        if (gs.PositionOnBoard(playX, playY) && gs.PositionOnBoard(compX, compY) && gs.GetPosition(playX, playY) != null && gs.GetPosition(compX, compY)!= null)
        {
            Debug.Log("here!");
            if (gs.GetPosition(playX, playY).name == "black_pawn" && gs.GetPosition(compX, compY).name == "white_pawn")
            {
                Debug.Log("hereB!");
                if ((playY - compY) == 1) return true;
            }
            if (gs.GetPosition(playX, playY).name == "white_pawn" && gs.GetPosition(compX, compY).name == "black_pawn")
            {
                Debug.Log("hereW!");
                if ((playY - compY) == -1) return true;
            }
        }
        return false;
    }
    public void SetEnPassantSquare()
    {

        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();

        int previousX = createdByPiece.GetComponent<PieceController>().xBoard;
        int previousY = createdByPiece.GetComponent<PieceController>().yBoard;

        if (Mathf.Abs((matrixY - previousY)) == 2 && matrixX == previousX)
        {
            gs.EnPassantSquare = matrixX.ToString() + " " + matrixY.ToString();
        }

        else gs.GetComponent<GameScript>().EnPassantSquare = " ";
    }

    public void CheckPromotion(string player) 
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        GameScript gs = controller.GetComponent<GameScript>();
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
