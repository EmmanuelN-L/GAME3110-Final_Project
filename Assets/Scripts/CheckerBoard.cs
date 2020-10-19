using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckerBoard : MonoBehaviour
{   //2D Array for chekerboard pieces, checkerboard is 8x8 
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    public Vector3 boardOffset = new Vector3(-4.0f, 0.0f, -4.0f);
    public Vector3 pieceOffset = new Vector3(0.5f, 0.0f, 0.5f);

    private bool isWhite = true;
    private bool isWhiteTurn;
    private bool hasKilled;

    private Piece selectedPiece;
    private List<Piece> forcedPieces;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;


    private void Start()
    {
        isWhiteTurn = true;
        forcedPieces = new List<Piece>();
        GenerateBoard();
        
    }

    private void Update()
    {

        // Debug.Log(mouseOver);
        if ((isWhite) ? isWhiteTurn : !isWhiteTurn)
        {
            UpdateMouseOver();
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if (selectedPiece != null)
                updatePieceDrag(selectedPiece);

            if (Input.GetMouseButtonDown(0))
            {
                selectPiece(x, y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
            }

        }
        
    }

    private void UpdateMouseOver()
    {
        //If it's my turn

        if(!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x - boardOffset.x);
            mouseOver.y = (int)(hit.point.z - boardOffset.z);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    private void updatePieceDrag(Piece p)
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("Board")))
        {
            p.transform.position = hit.point + Vector3.up;
        }
      
    }

    private void selectPiece(int x, int y)
    {
        //Out of bounds
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
        return;

        Piece p = pieces[x, y];
        if (p !=null && p.isWhite == isWhite)
        {
            if (forcedPieces.Count == 0)
            {
                 selectedPiece = p;
                 startDrag = mouseOver;
                 Debug.Log(selectedPiece.name);
            }
            else
            {
                //Look for the piece under our forced pieces list
                if (forcedPieces.Find(fp => fp == p) == null)
                    return;

                selectedPiece = p;
                startDrag = mouseOver;
            }
            

        }
    }

    private void TryMove(int x1, int y1, int x2, int y2)
    {
        forcedPieces = scanForPossibleMove();
        Debug.Log(forcedPieces.Count);

        //Multiplayer support
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        selectedPiece = pieces[x1, y1];

        MovePiece(selectedPiece, x2, y2);
        //If piece is out of bounds reset to the start drag position
        if(x2 <0 || x2 >= 8 || y2 < 0 || y2 >= 8)
        {
            if (selectedPiece != null)
                MovePiece(selectedPiece, x1, y1);

            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }

        if(selectedPiece != null)
        {
            //If piece hasnt move
            if(endDrag == startDrag)
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            //Check if it's a vald move
            if(selectedPiece.validMove(pieces, x1, y1, x2, y2))
            {
                //Kill Check if its a jump
                if(Mathf.Abs(x2-x1) == 2)
                {
                    Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(p.gameObject);
                        hasKilled = true;
                        scanForPossibleMove();
                    }
                }
                //Were we  supposed to kill anything?
                if(forcedPieces.Count !=0 && !hasKilled)
                {
                    MovePiece(selectedPiece, x1, y1);
                    startDrag = Vector2.zero;
                    selectedPiece = null;
                    Debug.Log("Broke");
                    return;
                }

                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x2, y2);
                scanForPossibleMove();
                endTurn();
            }
            else
            {
                MovePiece(selectedPiece, x1, y1);
                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

        }

    }

    private void endTurn()
    {   
        
        int x = (int)endDrag.x;
        int y = (int)endDrag.y;

        //Creating king
        if (selectedPiece != null)
        {   //For White Piece King
            if (selectedPiece.isWhite && !selectedPiece.isKing && y == 7)
            {
                selectedPiece.isKing = true;
                selectedPiece.transform.Rotate(Vector3.right * 180);
            } //For Black Piece King
            else if (!selectedPiece.isWhite && !selectedPiece.isKing && y == 0)
            {
                selectedPiece.isKing = true;
                selectedPiece.transform.Rotate(Vector3.right * 180);
            }
        }

        selectedPiece = null;
        startDrag = Vector2.zero;

        if(scanForPossibleMove(selectedPiece,x,y).Count != 0 && hasKilled)
        {
            return;
        }

        isWhiteTurn = !isWhiteTurn;
        isWhite = !isWhite;
        hasKilled = false;
        checkVictory();
    }


    private void checkVictory()
    {
        var ps = FindObjectsOfType<Piece>();
        bool hasWhite = false, hasBlack = false;
        for (int i = 0; i < ps.Length; i++)
        {
            if (ps[i].isWhite)
            {
                hasWhite = true;
            }
            else
            {
                hasBlack = true;
            }
        }
        if(!hasWhite)
        {
            Victory(true);
        }
        if(!hasBlack)
        {
            Victory(true);
        }
    }

    private void Victory(bool isWhite)
    {
        if (isWhite)
            Debug.Log("White team has won");
        else
            Debug.Log("Black team has won");
    }

    private List<Piece> scanForPossibleMove(Piece p, int x, int y)
    {
        forcedPieces = new List<Piece>();

        if(pieces[x,y].isForcedToMove(pieces, x, y))
        {
            forcedPieces.Add(pieces[x, y]);
        }

        return forcedPieces;
    }
    private List<Piece> scanForPossibleMove()
    {
        forcedPieces = new List<Piece>();

        //Check all the pieces
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if(pieces[i,j] != null && pieces[i,j].isWhite == isWhiteTurn)
                    if(pieces[i,j].isForcedToMove(pieces, i, j))
                        forcedPieces.Add(pieces[i, j]);
             
        return forcedPieces;
    }

    private void GenerateBoard()
    {
        // Generate White Pieces
        for (int y = 0; y < 3; y++)
        {
            //To check if it's in a odd row so that we can shift the pieces in the correct spot
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x+=2)
            {
                //Generate the piece 
                GeneratePiece((oddRow)?x : x+1, y);
            }
        }
   
        // Generate Black Pieces
        for (int y = 7; y > 4; y--)
        {
            //To check if it's in a odd row so that we can shift the pieces in the correct spot
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                //Generate the piece 
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }
    }

    

    private void GeneratePiece(int x, int y)
    {
        //generating the pieces 
        bool isWhite = (y > 3) ? false : true;
        GameObject go = Instantiate((isWhite) ? whitePiecePrefab : blackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    //Positioning the pieces on the board
    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = (Vector3.right * x) + (Vector3.forward * y) + boardOffset + pieceOffset;
    }
}
