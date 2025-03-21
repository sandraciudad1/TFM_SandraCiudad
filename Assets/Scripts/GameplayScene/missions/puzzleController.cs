using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleController : MonoBehaviour
{
    public bool canMove = false;

    GameObject[,] puzzlePieces = new GameObject[3, 3];
    float[] xPositions = new float[8];
    float[] yPositions = new float[8];
    Vector2Int emptySlot = new Vector2Int(2, 2);

    public GameObject pieza00, pieza01, pieza02;
    public GameObject pieza10, pieza11, pieza12;
    public GameObject pieza20, pieza21;

    Vector3 basePosition = new Vector3(118.548f, 22.84164f, 89.138f);
    float offsetX = 0.24f;
    float offsetY = -0.24f;  

    [SerializeField] GameObject playerTrigger;
    mission9Controller mission9;
    bool finish = false;

    // Initializes mission and puzzle setup.
    void Start()
    {
        mission9 = playerTrigger.GetComponent<mission9Controller>();

        initializePieces();
        initializePositions();
        SetInitialPositions();
    }

    // Fills the puzzlePieces array with corresponding pieces.
    void initializePieces()
    {
        puzzlePieces[0, 0] = pieza00;
        puzzlePieces[0, 1] = pieza01;
        puzzlePieces[0, 2] = pieza02;
        puzzlePieces[1, 0] = pieza10;
        puzzlePieces[1, 1] = pieza11;
        puzzlePieces[1, 2] = pieza12;
        puzzlePieces[2, 0] = pieza20;
        puzzlePieces[2, 1] = pieza21;
        puzzlePieces[2, 2] = null;
    }

    // Sets the x and y positions for puzzle pieces.
    void initializePositions()
    {
        xPositions[0] = 118.548f;
        xPositions[1] = 118.788f;
        xPositions[2] = 119.028f;
        xPositions[3] = 118.548f;
        xPositions[4] = 118.788f;
        xPositions[5] = 119.028f;
        xPositions[6] = 118.788f;
        xPositions[7] = 118.548f;

        yPositions[0] = 22.84164f;
        yPositions[1] = 22.84164f;
        yPositions[2] = 22.84164f;
        yPositions[3] = 22.60164f;
        yPositions[4] = 22.60164f;
        yPositions[5] = 22.60164f;
        yPositions[6] = 22.36164f;
        yPositions[7] = 22.36164f;
    }

    // Checks if the puzzle is solved and starts the end sequence.
    void Update()
    {
        if (canMove)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MovePiece(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MovePiece(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovePiece(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MovePiece(Vector2Int.left);
            }
        }
        
        if (isSolution() && !finish)
        {
            canMove = false;
            StartCoroutine(waiitUntilEnd());
        }
    }

    // Waits for a short time before marking the game as finished
    IEnumerator waiitUntilEnd()
    {
        yield return new WaitForSeconds(2.5f);
        mission9.finishGame = true;
        finish = true;
    }

    // Checks if all puzzle pieces are in their correct positions
    bool isSolution()
    {
        if (checkPiece(pieza00, 0) && checkPiece(pieza01, 1) && checkPiece(pieza02, 2) && checkPiece(pieza10, 3) && 
            checkPiece(pieza11, 4) && checkPiece(pieza12, 5) && checkPiece(pieza20, 6) && checkPiece(pieza21, 7))
        {
            return true;
        } 
        else
        {
            return false;
        }
    }

    // Checks if a puzzle piece is at the correct position
    bool checkPiece(GameObject piece, int index)
    {
        if (Mathf.Approximately(piece.transform.position.x, xPositions[index]) &&
            Mathf.Approximately(piece.transform.position.y, yPositions[index]))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Sets the initial world positions for all puzzle pieces
    void SetInitialPositions()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (puzzlePieces[i, j] != null)
                {
                    puzzlePieces[i, j].transform.position = GetWorldPosition(i, j);
                }
            }
        }
    }

    // Moves a piece in the given direction if possible
    void MovePiece(Vector2Int direction)
    {
        Vector2Int targetPos = emptySlot + direction;

        if (targetPos.x >= 0 && targetPos.x < 3 && targetPos.y >= 0 && targetPos.y < 3)
        {
            GameObject movingPiece = puzzlePieces[targetPos.x, targetPos.y];

            if (movingPiece != null)
            {
                movingPiece.transform.position = GetWorldPosition(emptySlot.x, emptySlot.y);

                puzzlePieces[emptySlot.x, emptySlot.y] = movingPiece;
                puzzlePieces[targetPos.x, targetPos.y] = null;
                emptySlot = targetPos;
            }
        }
    }

    // Returns the world position for a given grid coordinate
    Vector3 GetWorldPosition(int x, int y)
    {
        return basePosition + new Vector3(x * offsetX, y * offsetY, -0.024f);  
    }
}
