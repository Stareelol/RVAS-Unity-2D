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
        GameScript gs = controller.GetComponent<GameScript>();

        if (attack)
        {
            GameObject defender = gs.GetPosition(matrixX, matrixY);

            Destroy(defender);

            capture.Play(0);

            gs.SetPositionEmpty((int)createdByPiece.GetComponent<PieceController>().xBoard, (int)createdByPiece.GetComponent<PieceController>().yBoard);

            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;

            createdByPiece.GetComponent<PieceController>().SetCoords();

            gs.SetPosition(createdByPiece);

            gs.NextTurn();

            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
        }

        else
        {
            move.Play(0);

            CheckEnPassant();

            if (createdByPiece.GetComponent<PieceController>().name == "white_pawn" || createdByPiece.GetComponent<PieceController>().name == "black_pawn") SetEnPassantSquare();
            else gs.GetComponent<GameScript>().EnPassantSquare = " ";

            gs.SetPositionEmpty(createdByPiece.GetComponent<PieceController>().xBoard, createdByPiece.GetComponent<PieceController>().yBoard);
            createdByPiece.GetComponent<PieceController>().xBoard = matrixX;
            createdByPiece.GetComponent<PieceController>().yBoard = matrixY;
            createdByPiece.GetComponent<PieceController>().SetCoords();
            gs.SetPosition(createdByPiece);

            CheckPromotion();
            CheckCastlingShort(createdByPiece.GetComponent<PieceController>().name);
            CheckCastlingLong(createdByPiece.GetComponent<PieceController>().name);

            gs.NextTurn();
            createdByPiece.GetComponent<PieceController>().DestroyMovePlates();
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
            if ((matrixX - int.Parse(ep[0])) == 0 && Mathf.Abs(matrixY - int.Parse(ep[1])) == 1)
            {
                Destroy(gs.GetPosition(int.Parse(ep[0]), int.Parse(ep[1])));
                gs.SetPositionEmpty(int.Parse(ep[0]), int.Parse(ep[1]));
            }
        }

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
