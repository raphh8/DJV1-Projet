using UnityEngine;

public class UIStressLvl : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;
    [SerializeField] private PlayerCharacter player;

    private void Update()
    {
        fillTransform.sizeDelta = new Vector2(480f * player.StressPercent, fillTransform.sizeDelta.y);
    }
}
