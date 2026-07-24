using UnityEngine;
using UnityEngine.UI;

public class AdaptiveCalendarGrid : MonoBehaviour
{
    private GridLayoutGroup grid;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        UpdateCellSize();
    }

    private void UpdateCellSize()
    {
        float width = GetComponent<RectTransform>().rect.width;

        float availableWidth = width - grid.padding.left - grid.padding.right - grid.spacing.x * 6;

        float cellWidth = availableWidth / 7f;

        grid.cellSize = new Vector2(cellWidth, grid.cellSize.y);
    }
}