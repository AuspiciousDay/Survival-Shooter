using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public GameManager gameManager;
    public UIManager uIManager;
    public static readonly string VerticalAixs = "Vertical";
    public static readonly string HorizontalAixs = "Horizontal";
    public static readonly string FireAixs = "Fire1";
    public static readonly string ESCAixs = "Cancel";

    public float Vertical { get; private set; }
    public float Horizontal { get; private set; }
    public bool Fire { get; private set; }
    public bool ESC { get; private set; }

    private void OnDisable()
    {
        Vertical = 0f;
        Horizontal = 0f;
        Fire = false;
    }

    public void Update()
    {
        Vertical = Input.GetAxis(VerticalAixs);
        Horizontal = Input.GetAxis(HorizontalAixs);
        Fire = Input.GetButton(FireAixs);
        ESC = Input.GetButtonDown(ESCAixs);
        if (ESC)
        {
            if (uIManager.pausedUI.activeSelf)
            {
                gameManager.Paused(!ESC);
            }
            else
            {
                gameManager.Paused(ESC);
            }
            
        }        
    }
}
