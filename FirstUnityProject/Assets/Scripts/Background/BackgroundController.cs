using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform player;
    public float tileSize = 19.2f; // �� ����� ����/���� ũ�� (����Ƽ ��������)

    private Vector2Int currentTile = Vector2Int.zero;

    void Update()
    {
        // �÷��̾� ��ġ �������� ���� Ÿ�� ��ǥ ���
        Vector2Int newTile = new Vector2Int(
            Mathf.FloorToInt(player.position.x / tileSize),
            Mathf.FloorToInt(player.position.y / tileSize)
        );

        if (newTile != currentTile)
        {
            currentTile = newTile;
            RepositionTiles();
        }
    }

    void RepositionTiles()
    {
        // 4�� Ÿ�� ���ġ
        for (int y = -1; y <= 0; y++)
        {
            for (int x = -1; x <= 0; x++)
            {
                string tileName = $"Background_{(y == -1 ? "B" : "T")}{(x == -1 ? "L" : "R")}";
                Transform tile = transform.Find(tileName);
                if (tile)
                {
                    tile.position = new Vector3(
                        (currentTile.x + x + 1) * tileSize,
                        (currentTile.y + y + 1) * tileSize,
                        0
                    );
                }
            }
        }
    }
}
