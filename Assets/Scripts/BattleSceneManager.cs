using UnityEngine;

public class BattleSceneManager : MonoBehaviour
{
    [Header("Лейбли гравців")]
    public RectTransform labelP1;
    public RectTransform labelP2;
    public Vector2 labelOffset = new Vector2(0, 50f);

    [Header("Префаби персонажів")]
    public GameObject warriorPrefab;
    public GameObject magicianPrefab;

    [Header("Spawn точки")]
    public Transform spawnPointPlayer1;
    public Transform spawnPointPlayer2;

    [Header("Canvas")]
    public RectTransform mainCanvas;

    private Transform player1Transform, player2Transform;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        GameManager gm = GameManager.Instance;
        if (gm == null) return;

        GameObject p1 = Instantiate(
            gm.selectedCharacter1 == 0 ? warriorPrefab : magicianPrefab,
            spawnPointPlayer1.position,
            Quaternion.identity
        );
        GameObject p2 = Instantiate(
            gm.selectedCharacter2 == 0 ? warriorPrefab : magicianPrefab,
            spawnPointPlayer2.position,
            Quaternion.identity
        );

        player1Transform = p1.transform;
        player2Transform = p2.transform;

        Transform body1 = p1.transform.Find("Body");
        if (body1 != null) body1.gameObject.tag = "BodyP1";
        Transform body2 = p2.transform.Find("Body");
        if (body2 != null) body2.gameObject.tag = "BodyP2";

        Transform hitbox1 = p1.transform.Find("Hitbox");
        if (hitbox1 != null) hitbox1.gameObject.tag = "HitboxP1";
        Transform hitbox2 = p2.transform.Find("Hitbox");
        if (hitbox2 != null) hitbox2.gameObject.tag = "HitboxP2";

        Transform shortAtk1 = p1.transform.Find("ShortAttack");
        if (shortAtk1 != null) shortAtk1.gameObject.tag = "ShortAttackP1";
        Transform shortAtk2 = p2.transform.Find("ShortAttack");
        if (shortAtk2 != null) shortAtk2.gameObject.tag = "ShortAttackP2";

        Transform longAtk1 = p1.transform.Find("LongAttack");
        if (longAtk1 != null) longAtk1.gameObject.tag = "LongAttackP1";
        Transform longAtk2 = p2.transform.Find("LongAttack");
        if (longAtk2 != null) longAtk2.gameObject.tag = "LongAttackP2";

        Player player1 = p1.GetComponent<Player>();
        Player player2 = p2.GetComponent<Player>();

        if (player1 != null && player2 != null)
        {
            player1.leftKey = KeyCode.A;
            player1.rightKey = KeyCode.D;
            player1.upKey = KeyCode.W;
            player1.downKey = KeyCode.S;
            player1.longAtkKey = KeyCode.G;
            player1.shortAtkKey = KeyCode.F;
            player1.shieldKey = KeyCode.H;

            player2.leftKey = KeyCode.Keypad4;
            player2.rightKey = KeyCode.Keypad6;
            player2.upKey = KeyCode.Keypad8;
            player2.downKey = KeyCode.Keypad2;
            player2.longAtkKey = KeyCode.Keypad9;
            player2.shortAtkKey = KeyCode.Keypad7;
            player2.shieldKey = KeyCode.Keypad5;

            player1.otherPlayer = player2.transform;
            player2.otherPlayer = player1.transform;
        }
    }

    void LateUpdate()
    {
        UpdateLabelPosition(labelP1, player1Transform);
        UpdateLabelPosition(labelP2, player2Transform);
    }

    private void UpdateLabelPosition(RectTransform label, Transform target)
    {
        if (label == null || target == null || mainCanvas == null) return;

        Vector2 screenPos = mainCamera.WorldToScreenPoint(target.position);

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas,
            screenPos + labelOffset,
            mainCamera,
            out localPos
        );

        label.anchoredPosition = localPos;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(target.position);
        label.gameObject.SetActive(
            viewportPos.x > 0 && viewportPos.x < 1 &&
            viewportPos.y > 0 && viewportPos.y < 1
        );
    }
}
