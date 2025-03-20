using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puzzleController : MonoBehaviour
{
    public GameObject[,] puzzlePieces = new GameObject[3, 3]; // Matriz de piezas
    private Vector2Int emptySlot = new Vector2Int(2, 2); // Posición vacía en la matriz (fila 2, columna 2)

    public GameObject pieza00, pieza01, pieza02;
    public GameObject pieza10, pieza11, pieza12;
    public GameObject pieza20, pieza21;

    private Vector3 basePosition = new Vector3(118.548f, 22.84164f, 89.13959f);
    private float offsetX = 0.24f;
    private float offsetY = -0.24f;  // Antes era Z, ahora Y

    void Start()
    {
        // Asignar las piezas a la matriz
        puzzlePieces[0, 0] = pieza00;
        puzzlePieces[0, 1] = pieza01;
        puzzlePieces[0, 2] = pieza02;
        puzzlePieces[1, 0] = pieza10;
        puzzlePieces[1, 1] = pieza11;
        puzzlePieces[1, 2] = pieza12;
        puzzlePieces[2, 0] = pieza20;
        puzzlePieces[2, 1] = pieza21;
        puzzlePieces[2, 2] = null; // Espacio vacío

        SetInitialPositions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MovePiece(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            MovePiece(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MovePiece(Vector2Int.right);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MovePiece(Vector2Int.left);
    }

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

    void MovePiece(Vector2Int direction)
    {
        Vector2Int targetPos = emptySlot + direction;

        if (targetPos.x >= 0 && targetPos.x < 3 && targetPos.y >= 0 && targetPos.y < 3)
        {
            GameObject movingPiece = puzzlePieces[targetPos.x, targetPos.y];

            if (movingPiece != null)
            {
                // Mueve la pieza en Unity en X e Y en lugar de X y Z
                movingPiece.transform.position = GetWorldPosition(emptySlot.x, emptySlot.y);

                // Intercambia en la matriz
                puzzlePieces[emptySlot.x, emptySlot.y] = movingPiece;
                puzzlePieces[targetPos.x, targetPos.y] = null;
                emptySlot = targetPos;
            }
        }
    }

    Vector3 GetWorldPosition(int x, int y)
    {
        return basePosition + new Vector3(x * offsetX, y * offsetY, -0.24f);  // Se mueve en X e Y
    }
}
