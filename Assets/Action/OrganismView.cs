using UnityEngine;

public class OrganismView : MonoBehaviour
{
    public OrganismData Data
    {
        get;
        private set;
    }

    public void Initialize(
        OrganismData data)
    {
        Data = data;

        UpdateView();
    }

    private void Update()
    {
        if (Data == null)
        {
            return;
        }

        UpdateView();
    }

    private void UpdateView()
    {
        transform.position =
            new Vector3(
                Data.position.x / 100f,
                Data.position.y / 100f,
                0f
            );
    }
}