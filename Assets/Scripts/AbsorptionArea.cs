using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class AbsorptionArea : MonoBehaviour, IAbsorbable
{
    public AbsorptionMaterialChange materials;

    [OnValueChanged("OnBoundariesChanged")]
    [ShowIf("isEditingBoundaries")]
    public Vector3[] boundaries;
    public bool areaHasWater = true;

    public float transitionSpeed = 0.5f;

    [HideInInspector]
    public bool isEditingBoundaries;

    [SerializeField]
    private int lastBoundariesLength = 0;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [ShowInInspector]
    [SerializeField]
    [ReadOnly]
    private MeshRenderer[] natureMeshRenderers;

    private static readonly System.Random random = new System.Random();

    [Button]
    private void ToggleDriedState()
    {
        if (areaHasWater)
        {
            OnAbsorption();
        }
        else
        {
            OnRestore();
        }
    }

    private void Start()
    {
        UpdateMaterialBasedOnWater();
    }

    public bool OnAbsorption()
    {
        if (!areaHasWater)
        {
            return false;
        }

        areaHasWater = false;
        UpdateMaterialBasedOnWater();
        return true;
    }

    public bool OnRestore()
    {
        if (areaHasWater)
        {
            return false;
        }

        areaHasWater = true;
        UpdateMaterialBasedOnWater();
        return true;
    }

    private void UpdateMaterialBasedOnWater()
    {
        float driedValue = areaHasWater ? 1.0f : 0.0f;
        DOTween.To(() => driedValue, x => driedValue = x, (areaHasWater ? 0.0f : 1.0f), transitionSpeed).OnUpdate(() =>
        {
            foreach (var nature in natureMeshRenderers)
            {
                nature.materials[0].SetFloat("_DRIED", driedValue);
            }
        });
    }

#if UNITY_EDITOR
    private Vector3 GetRandomPointAround(Vector3 center, float radius)
    {
        float angle = (float)(2.0 * System.Math.PI * random.NextDouble());

        float randX = center.x + radius * Mathf.Cos(angle);
        float randZ = center.z + radius * Mathf.Sin(angle);

        return new Vector3(randX, center.y, randZ);
    }

    private void OnBoundariesChanged()
    {
        if (boundaries.Length > lastBoundariesLength)
        {
            float radius = Random.Range(0.1f, Mathf.Min(transform.localScale.x, transform.localScale.z) / 2.0f);
            boundaries[boundaries.Length - 1] = GetRandomPointAround(transform.position, radius);
        }
        lastBoundariesLength = boundaries.Length;
    }

    private Mesh CreateMesh(Vector2[] vertices2d, int[] indices)
    {
        Vector3[] vertices = new Vector3[vertices2d.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(transform.position.x - vertices2d[i].x, transform.position.y, transform.position.z - vertices2d[i].y);
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateNormals();

        string filePath = EditorUtility.SaveFilePanelInProject("Save Procedural Mesh", "Procedural Mesh", "asset", "");
        if (filePath != "")
        {
            AssetDatabase.CreateAsset(mesh, filePath);
        }

        return mesh;
    }

    [Button(ButtonSizes.Medium), GUIColor(0.58f, 0.58f, 0.58f)]
    [HideIf("isEditingBoundaries")]
    public void EditBoundaries()
    {
        isEditingBoundaries = true;
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Medium), GUIColor(0.14f, 0.89f, 0.14f)]
    [ShowIf("isEditingBoundaries")]
    public void BakeBoundaries()
    {
        isEditingBoundaries = false;

        int[] sourceIndices;

        Vector2[] sourceVertices = new Vector2[boundaries.Length];
        for (int i = 0; i < boundaries.Length; i++)
            sourceVertices[i] = new Vector2(boundaries[i].x, boundaries[i].z);

        if (sourceVertices.Length > 0)
        {
            Triangulator2D.Triangulator.Triangulate(sourceVertices, Triangulator2D.WindingOrder.Clockwise, out sourceVertices, out sourceIndices);
        }
        else
        {
            sourceIndices = new int[sourceVertices.Length / 3];
        }
        
        Mesh mesh = CreateMesh(sourceVertices, sourceIndices);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(ButtonSizes.Medium, Name = "Cancel"), GUIColor(0.89f, 0.14f, 0.14f)]
    [ShowIf("isEditingBoundaries")]
    public void CancelBakeBoundaries()
    {
        isEditingBoundaries = false;
    }

    [Button(ButtonSizes.Medium), GUIColor(0.89f, 0.14f, 0.14f)]
    public void Populate()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        natureMeshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
    }
#endif
}
