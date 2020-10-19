using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool isWhite;
    public bool isKing;

    public bool isForcedToMove(Piece[,] board, int x, int y)
    {
        if (isWhite || isKing)
        {
            //Top Left
            if (x >= 2 && y <= 5)
            {
                Piece p = board[x - 1, y + 1];
                //If there is a piece, and it's not the color as ours
                if (p != null && p.isWhite !=isWhite)
                {
                    //Check if its possible to land after the jump
                    if (board[x - 2, y + 2] == null)
                    { 
                        return true;
                    }
                        
                }
            }
            //Top Right

            if (x <= 5 && y <= 5)
            {
                Piece p = board[x + 1, y + 1];
                //If there is a piece, and it's not the color as ours
                if (p != null && p.isWhite != isWhite)
                {
                    //Check if its possible to land after the jump
                    if (board[x + 2, y + 2] == null)
                    {
                        return true;
                    }

                }
            }
        }

        if(!isWhite || isKing)
        {
            //Bottom Left
            if (x >= 2 && y >= 2)
            {
                Piece p = board[x - 1, y - 1];
                //If there is a piece, and it's not the color as ours
                if (p != null && p.isWhite != isWhite)
                {
                    //Check if its possible to land after the jump
                    if (board[x - 2, y - 2] == null)
                    {
                        return true;
                    }

                }
            }
            //BottomRight

            if (x <= 5 && y >= 2)
            {
                Piece p = board[x + 1, y - 1 ];
                //If there is a piece, and it's not the color as ours
                if (p != null && p.isWhite != isWhite)
                {
                    //Check if its possible to land after the jump
                    if (board[x + 2, y - 2] == null)
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }
    public bool validMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        //If you're moving on top of another piece
        if (board[x2, y2] != null)
        {
            return false;
        }

        int deltaMoveX = (int)Mathf.Abs(x1 - x2);
        int deltaMoveY = y2 - y1;

        // If piece is white or if the piece is a king
        if (isWhite || isKing)
        {
            //If you're moving 1 diaganol
            if (deltaMoveX == 1)
            {
                if (deltaMoveY == 1)
                    return true;

            }
            //If you're moving for a kill
            else if (deltaMoveX == 2)
            {
                if (deltaMoveY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; 
                    if(p != null && p.isWhite != isWhite)
                    {
                        return true;

                    }
                }
          
            }
        }

        if (!isWhite || isKing)
        {
            //If you're moving 1 diaganol
            if (deltaMoveX == 1)
            {
                if (deltaMoveY == -1)
                    return true;

            }
            //If you're moving for a kill
            else if (deltaMoveX == 2)
            {
                if (deltaMoveY == -2)
                {   //Checking which piece you killed
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2]; 
                    if (p != null && p.isWhite != isWhite)
                    {
                        return true;

                    }
                }

            }
        }

        return false;
    }
}
