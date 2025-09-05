using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    void Combine()
    {
        // Get all mesh filters in children
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].sharedMesh == null)
            {
                i++;
                continue;
            }

            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false); // disable original
            i++;
        }

        // Create new mesh filter
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = new Mesh();
        mf.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32; // supports >65k verts
        mf.mesh.CombineMeshes(combine);

        // Add mesh renderer (copy from first child if exists)
        MeshRenderer parentRenderer = gameObject.GetComponent<MeshRenderer>();
        if (parentRenderer == null && meshFilters.Length > 0)
        {
            MeshRenderer childRenderer = meshFilters[0].GetComponent<MeshRenderer>();
            if (childRenderer != null)
            {
                MeshRenderer newRenderer = gameObject.AddComponent<MeshRenderer>();
                newRenderer.sharedMaterial = childRenderer.sharedMaterial;
            }
        }

        Debug.Log("Meshes combined into: " + mf.mesh.vertexCount + " verts");
    }
}