using Mirror;
using UnityEngine;

public class VehicleColor : NetworkBehaviour
{
    [SerializeField] 
    private SpriteRenderer m_spriteRenderer;

    [SyncVar(hook = nameof(SetColor))]
    private Color synccolor;

    public override void OnStartClient()
    {
        base.OnStartClient();

        synccolor = Random.ColorHSV();
    }

    private void SetColor(Color oldColor, Color newColor)
    {
        m_spriteRenderer.color = newColor;
    }
}
