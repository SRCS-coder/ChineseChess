using GameLogic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public const int ZForPiece = -5;
    public const int ZForTargetPosition = -6;

    public const float YForNorth = 4.0685f;
    public const float YForSouth = -4.1908f;
    public const float XForEast = 3.6754f;
    public const float XForWest = -3.6658f;

    [SerializeField]
    private GameObject blackGeneralPrefab;
    [SerializeField]
    private GameObject blackAdvisorPrefab;
    [SerializeField]
    private GameObject blackElephantPrefab;
    [SerializeField]
    private GameObject blackChariotPrefab;
    [SerializeField]
    private GameObject blackHorsePrefab;
    [SerializeField]
    private GameObject blackCannonPrefab;
    [SerializeField]
    private GameObject blackSoldierPrefab;

    [SerializeField]
    private GameObject redGeneralPrefab;
    [SerializeField]
    private GameObject redAdvisorPrefab;
    [SerializeField]
    private GameObject redElephantPrefab;
    [SerializeField]
    private GameObject redChariotPrefab;
    [SerializeField]
    private GameObject redHorsePrefab;
    [SerializeField]
    private GameObject redCannonPrefab;
    [SerializeField]
    private GameObject redSoldierPrefab;

    [SerializeField]
    private GameObject currentColorBlack;
    [SerializeField]
    private GameObject currentColorRed;

    [SerializeField]
    private GameObject targetPositionPrefab;

    [SerializeField]
    private GameObject gameOverCanvas;
    [SerializeField]
    private Text gameOverText; 

    private GameState gameState;
    private readonly List<Move> availableMoves = new();

    private GameObject selectedPieceObject;

    private Dictionary<PieceType, GameObject> blackPiecePrefabs;
    private Dictionary<PieceType, GameObject> redPiecePrefabs;

    private readonly List<GameObject> pieceObjects = new();
    private readonly List<GameObject> targetPositionObjects = new();

    private void Start()
    {
        blackPiecePrefabs = new Dictionary<PieceType, GameObject>()
        {
            { PieceType.General, blackGeneralPrefab },
            { PieceType.Advisor, blackAdvisorPrefab },
            { PieceType.Elephant, blackElephantPrefab },
            { PieceType.Chariot, blackChariotPrefab },
            { PieceType.Horse, blackHorsePrefab },
            { PieceType.Cannon, blackCannonPrefab },
            { PieceType.Soldier, blackSoldierPrefab }
        };

        redPiecePrefabs = new Dictionary<PieceType, GameObject>()
        {
            { PieceType.General, redGeneralPrefab },
            { PieceType.Advisor, redAdvisorPrefab },
            { PieceType.Elephant, redElephantPrefab },
            { PieceType.Chariot, redChariotPrefab },
            { PieceType.Horse, redHorsePrefab },
            { PieceType.Cannon, redCannonPrefab },
            { PieceType.Soldier, redSoldierPrefab }
        };

        NewGame();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d)
            {
                OnClickObject(hit2d.transform.gameObject);
            }
            else
            {
                OnClickNothing();
            }
        }
    }

    public void NewGame()
    {
        gameState = new GameState();
        UpdatePieces();
    }

    public void Cancel()
    {
        gameState.CancelMove();
        UpdatePieces();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void OnClickObject(GameObject gameObject)
    {
        if (selectedPieceObject == null)
        {
            if (gameObject.TryGetComponent(out PieceBehaviour pieceBehaviour))
            {
                Piece piece = pieceBehaviour.Piece;
                Position position = gameState.Board.GetPiecePosition(piece);
                availableMoves.AddRange(gameState.LegalMovesForPiece(position).ToList());

                if (availableMoves.Count > 0)
                {
                    selectedPieceObject = gameObject;
                    UpdateTargetPositions();
                }

                return;
            }
        }
        else
        {
            if (gameObject.TryGetComponent(out TargetPositionBehavior targetPositionBehavior))
            {
                Position targetPosition = targetPositionBehavior.Position;
                Move move = availableMoves.First(move => move.ToPosition == targetPosition);
                gameState.MakeMove(move);
                UpdatePieces();

                return;
            }
        }

        OnClickNothing();
    }

    private void OnClickNothing()
    {
        selectedPieceObject = null;
        availableMoves.Clear();
        UpdateTargetPositions();
    }

    private void UpdatePieces()
    {
        foreach (GameObject piece in pieceObjects)
        {
            Destroy(piece);
        }

        pieceObjects.Clear();

        foreach (Piece piece in gameState.Board.AllPieces())
        {
            GameObject piecePrefab = piece.Color switch
            {
                PieceColor.Black => blackPiecePrefabs[piece.Type],
                PieceColor.Red => redPiecePrefabs[piece.Type],
                _ => null
            };

            GameObject pieceObject = CreatePiece(piecePrefab, piece);

            if (piece.Type == PieceType.General && piece.Color == gameState.CurrentColor)
            {
                if (gameState.Board.IsInCheck(gameState.CurrentColor))
                {
                    pieceObject.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }

            pieceObjects.Add(pieceObject);
        }

        selectedPieceObject = null;
        availableMoves.Clear();

        UpdateCurrentColor();
        UpdateTargetPositions();
        UpdateGameResult();
    }

    void UpdateTargetPositions()
    {
        foreach (GameObject targetPosition in targetPositionObjects)
        {
            Destroy(targetPosition);
        }

        targetPositionObjects.Clear();

        foreach (Position position in availableMoves.Select(move => move.ToPosition))
        {
            targetPositionObjects.Add(CreateTargetPosition(position));
        }
    }

    private GameObject CreatePiece(GameObject piecePrefab, Piece piece)
    {
        GameObject pieceObject = Instantiate(piecePrefab,
            ConvertPosition(gameState.Board.GetPiecePosition(piece), ZForPiece),
            Quaternion.identity);

        pieceObject.GetComponent<PieceBehaviour>().Piece = piece;
        return pieceObject;
    }

    private GameObject CreateTargetPosition(Position position)
    {
        GameObject targetPositionObject = Instantiate(targetPositionPrefab,
            ConvertPosition(position, ZForTargetPosition),
            Quaternion.identity);

        targetPositionObject.GetComponent<TargetPositionBehavior>().Position = position;
        return targetPositionObject;
    }

    private Vector3 ConvertPosition(Position position, int z)
    {
        float distanceForRow = (YForNorth - YForSouth) / (Board.RowCount - 1);
        float distanceForColumn = (XForEast - XForWest) / (Board.ColumnCount - 1);

        return new Vector3(XForWest + position.Column * distanceForRow,
            YForNorth - position.Row * distanceForColumn, z);
    }

    private void UpdateCurrentColor()
    {
        currentColorBlack.SetActive(gameState.CurrentColor == PieceColor.Black);
        currentColorRed.SetActive(gameState.CurrentColor == PieceColor.Red);
    }

    private void UpdateGameResult()
    {
        Result result = gameState.Result;

        if (result != null)
        {
            string resultText = "游戏结束，" + result.Winner switch
            {
                PieceColor.Red => "红方获胜",
                PieceColor.Black => "黑方获胜",
                _ => "双方平局"
            };

            gameOverText.text = resultText;
            gameOverCanvas.SetActive(true);
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }
    }
}
