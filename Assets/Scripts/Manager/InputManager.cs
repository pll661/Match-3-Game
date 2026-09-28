using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]private GridManager Grid;
    public GameObject selectBoxPrefab;
    private GameObject selectBox;
    public Piece firstSelectedPiece { get; private set; }
    public Piece secondSelectedPiece { get; private set; }



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GameManager.Instance.gameState != GameState.Playing)
            {
                Debug.Log("½ûÖ¹µã»÷");
                return;
            }
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            Piece piece = Grid.GetPieceByWorldPosition(worldPos);
            if (piece == null) return;
            SelectedPiece(piece);
            //Debug.Log((piece.x, piece.y));
        }
    }
    public void SelectedPiece(Piece piece)
    {

        if (firstSelectedPiece == null)
        {
            firstSelectedPiece = piece;
            if (selectBox == null)
            {
                selectBox = Instantiate(selectBoxPrefab, transform);
            }
            selectBox.transform.position = firstSelectedPiece.transform.position;
            return;
        }
        if (piece == firstSelectedPiece)
            return;
        secondSelectedPiece = piece;
        if (!IsAdjacent(firstSelectedPiece, secondSelectedPiece))
        {
            firstSelectedPiece = secondSelectedPiece;
            secondSelectedPiece = null;
            selectBox.transform.position = firstSelectedPiece.transform.position;
            return;
        }
        //½»»»Î»ÖÃ
        GameManager.Instance.SetGameState(GameState.Resolving);
        Grid.SwapPiece(firstSelectedPiece, secondSelectedPiece);
        firstSelectedPiece = null;
        secondSelectedPiece = null;
        if (selectBox != null)
        {
            Destroy(selectBox);
            selectBox = null;
        }

    }
    public bool IsAdjacent(Piece p1, Piece p2)
    {
        int dx = Mathf.Abs(p1.x - p2.x);
        int dy = Mathf.Abs(p1.y - p2.y);
        return dx + dy == 1;
    }

}
