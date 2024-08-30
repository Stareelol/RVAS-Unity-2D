using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    public Sprite QueenWhitePromote, QueenBlackPromote, BishopWhitePromote, BishopBlackPromote, RookWhitePromote, RookBlackPromote, KnightWhitePromote, KnightBlackPromote;

    public bool QueenPress, RookPress, KnightPress, BishopPress;

    public GameObject pieceToPromote = null;

    GameObject controller;

    CalculateAllDMoves cadm;
    GameScript gs;
    CalculateAllMoves cam;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().enabled = false;
        controller = GameObject.FindGameObjectWithTag("GameController");
        cadm = controller.GetComponent<CalculateAllDMoves>();
        cam = controller.GetComponent<CalculateAllMoves>();
        gs = controller.GetComponent<GameScript>();
    }

    public void ShowPromoteScreen(string player, GameObject piece)
    {
        Button QueenPromote, RookPromote, BishopPromote, KnightPromote;

        pieceToPromote = piece;
        this.GetComponent<Canvas>().enabled = true;

        QueenPromote = GameObject.Find("QueenPromote").GetComponent<Button>();
        RookPromote = GameObject.Find("RookPromote").GetComponent<Button>();
        BishopPromote = GameObject.Find("BishopPromote").GetComponent<Button>();
        KnightPromote = GameObject.Find("KnightPromote").GetComponent<Button>();

        switch (player)
        {
            case "white":
                {
                    QueenPromote.image.sprite = QueenWhitePromote;
                    BishopPromote.image.sprite = BishopWhitePromote;
                    RookPromote.image.sprite = RookWhitePromote;
                    KnightPromote.image.sprite = KnightWhitePromote;
                }
                break;
            case "black":
                {
                    QueenPromote.image.sprite = QueenBlackPromote;
                    BishopPromote.image.sprite = BishopBlackPromote;
                    RookPromote.image.sprite = RookBlackPromote;
                    KnightPromote.image.sprite = KnightBlackPromote;
                }
                break;
        }
    }

    public void OnQueenPromoteClick()
    {
        this.GetComponent<Canvas>().enabled = false;

        if (pieceToPromote.name == "white_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("white_queen", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("white");
        }

        if (pieceToPromote.name == "black_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("black_queen", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("black");
        }
    }

    public void OnRookPromoteClick()
    {
        this.GetComponent<Canvas>().enabled = false;

        if (pieceToPromote.name == "white_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("white_rook", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("white");
        }

        if (pieceToPromote.name == "black_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("black_rook", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("black");
        }
    }

    public void OnBishopPromoteClick()
    {
        this.GetComponent<Canvas>().enabled = false;

        if (pieceToPromote.name == "white_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("white_bishop", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("white");
        }

        if (pieceToPromote.name == "black_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("black_bishop", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("black");
        }
    }

    public void OnKnightPromoteClick()
    {
        this.GetComponent<Canvas>().enabled = false;

        if (pieceToPromote.name == "white_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("white_knight", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("white");
        }

        if (pieceToPromote.name == "black_pawn")
        {
            Destroy(pieceToPromote);
            controller.GetComponent<GameScript>().SetPositionEmpty(pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            GameObject promotedPiece = controller.GetComponent<GameScript>().Create("black_knight", pieceToPromote.GetComponent<PieceController>().xBoard, pieceToPromote.GetComponent<PieceController>().yBoard);
            controller.GetComponent<GameScript>().SetPosition(promotedPiece);
            CalculateAgain("black");
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

    public void CalculateAgain(string player)
    {
        if (player == "white")
        {
            cam.Calculate("white");
            cadm.Calculate("black");
            cadm.IsCheckmate("black");
            RemoveCastlingIfChecked("black");
        }
        if (player == "black")
        {
            cam.Calculate("black");
            cadm.Calculate("white");
            cadm.IsCheckmate("white");
            RemoveCastlingIfChecked("white");
        }
    }
}
