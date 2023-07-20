using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class PlayerColorPallete : NetworkBehaviour
{
    public static PlayerColorPallete Instance;

    [SerializeField] private List<Color> m_AllColors;

    private List<Color> AvailableColors;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
     
        Instance = this;

        AvailableColors = new List<Color>();
        m_AllColors.CopyTo(AvailableColors);
    }

    public Color TakeRandomColor()
    {
        int index = Random.Range(0, AvailableColors.Count);
        Color color = m_AllColors[index];

        AvailableColors.RemoveAt(index);

        return color;
    }

    public void PutColor(Color color)
    {
        if (m_AllColors.Contains(color) == true)
        {
            if (AvailableColors.Contains(color) == false)
            {
                AvailableColors.Add(color);
            }
        }
    }


}
