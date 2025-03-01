using Core;
using Core.Prank;
using UnityEngine;

namespace CrazyPawn
{
    [CreateAssetMenu(menuName = "CrazyPawn/Settings", fileName = "CrazyPawnSettings")]
    public class CrazyPawnSettings : ScriptableObject
    {
        [SerializeField] public float InitialZoneRadius = 10f;
        [SerializeField] public int InitialPawnCount = 7;

        [SerializeField] public Material DeleteMaterial;
        [SerializeField] public Material ActiveConnectorMaterial;

        [SerializeField] public int CheckerboardSize = 18;
        [SerializeField] public Color BlackCellColor = Color.yellow;
        [SerializeField] public Color WhiteCellColor = Color.green;

        [SerializeField] public float DragSensitivity = 0.01f;
        [SerializeField] public ConnectionView ConnectionViewPrefab;
        [SerializeField] public LineRenderer ConnectionLinePrefab;

        [Header("Чтобы улыбнуло )")]
        [SerializeField] public bool IsPrank = true;
        [SerializeField] public PrankLine PrankLinePrefab;
        [SerializeField] public PrankFigure PrankFigurePrefab;
    }
}