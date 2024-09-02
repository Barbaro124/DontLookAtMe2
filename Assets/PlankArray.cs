using UnityEngine;

public class PlankArray : MonoBehaviour
{
    public GameObject plankPrefab; // Assign your plank prefab in the inspector
    public int rows = 1;           // Number of rows of planks
    public int columns = 1;        // Number of columns of planks
    public float spacing = 0.1f;   // Spacing between planks

    private GameObject[,] plankArray;

    public void GeneratePlanks()
    {
        // Clear existing planks if any
        ClearPlanks();

        // Initialize the 2D array
        plankArray = new GameObject[rows, columns];

        // Get the size of the plank prefab to calculate spacing
        Vector3 plankSize = plankPrefab.GetComponent<Renderer>().bounds.size;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position for each plank based on row, column, and spacing
                float randomZOffset = Random.Range(-0.15f, 0.15f);  // Adjust the range for desired randomness
                Vector3 position = new Vector3(
                    col * (plankSize.x + spacing),   // Horizontal position
                    row * (plankSize.y + spacing),   // Vertical stacking on the y-axis
                    randomZOffset                    // Random offset for natural stacking
                );

                // Instantiate plank
                GameObject plank = Instantiate(plankPrefab, transform.position + position, Quaternion.identity, transform);

                // Randomly rotate 180 degrees on the y-axis
                float randomYRotation = Random.value > 0.5f ? 180f : 0f;
                plank.transform.Rotate(0, randomYRotation, 0);

                // Apply a random tint or shade to the plank material
                Renderer plankRenderer = plank.GetComponent<Renderer>();
                if (plankRenderer != null)
                {
                    // Clone the material to avoid modifying the original prefab material
                    Material plankMaterial = new Material(plankRenderer.sharedMaterial);

                    // Generate a random color tint
                    float randomTint = Random.Range(0.5f, 1.5f); // Adjust this range for desired color variation
                    Color originalColor = plankMaterial.color; // Get the original color of the material
                    Color tintedColor = originalColor * randomTint; // Apply tint
                    plankMaterial.color = tintedColor; // Set the new tinted color

                    // Assign the new material to the plank
                    plankRenderer.material = plankMaterial;
                }

                // Store reference in array
                plankArray[row, col] = plank;
            }
        }
    }





    public void ClearPlanks()
    {
        if (plankArray != null)
        {
            foreach (GameObject plank in plankArray)
            {
                if (plank != null)
                {
                    DestroyImmediate(plank);
                }
            }
        }
    }
}
