using System;
using UnityEngine;

public class PromotionUI : MonoBehaviour
{
    [SerializeField] private GameObject panelBody;
    public Action<PieceType> OnPick;

    private void Awake()
    {
        panelBody.SetActive(false);
    }
    public void Show()
    {
        panelBody.SetActive(true);
    }
    public void Hide()
    {
        panelBody.SetActive(false);
    }

    public void PickQueen() { OnPick?.Invoke(PieceType.Queen); }
    public void PickRook() { OnPick?.Invoke(PieceType.Rook); }
    public void PickBishop() { OnPick?.Invoke(PieceType.Bishop); }
    public void PickKnight() { OnPick?.Invoke(PieceType.Knight); }

}
