using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IceCubeGrid
{
    public int xCount;
    public int zCount;
    public int yCount;
    public float yBase;
    public float yStep;
    public float xStep;
    public float zStep;

}

public class IceCubeController : MonoBehaviour
{
    [SerializeField] GameObject iceRockPrefab;
    [SerializeField] Transform iceRockContainer;
    [SerializeField] float instantiateTimeout = 0.5f;
    [SerializeField] IceCubeGrid grid;

    private void Start()
    {
        BuildGrid();
    }

    [ContextMenu("Build Grid")]
    private void BuildGrid()
    {
        iceRockContainer.Clear();

        StartCoroutine(InstantiateRocks());

    }

    private IEnumerator InstantiateRocks()
    {
        var timer = new WaitForSeconds(instantiateTimeout);

        for (int y = 0; y < grid.yCount; y++)
        {
            for (int x = 0; x < grid.xCount; x++)
            {
                for (int z = 0; z < grid.zCount; z++)
                {
                    //if (y != grid.yCount - 1 &&
                    //    x != 0 && x != grid.xCount - 1 &&
                    //    z != 0 && z != grid.zCount - 1)
                    //{
                    //    continue;
                    //}

                    if ((y == 0 || y == 1) &&
                        x == Mathf.RoundToInt(grid.xCount / 2f) &&
                        (z == Mathf.RoundToInt(grid.zCount / 2f) ||
                        z == Mathf.RoundToInt(grid.zCount / 2f) - 1))
                        continue;

                    float yPos = grid.yBase + y * grid.yStep;
                    float xPos = x * grid.xStep - (grid.xCount - 1) * grid.xStep / 2f;
                    float zPos = z * grid.zStep - (grid.zCount - 1) * grid.zStep / 2f;

                    var pos = iceRockContainer.position + new Vector3(xPos, yPos, zPos);

                    var rock = Instantiate(iceRockPrefab, pos, Quaternion.identity, iceRockContainer).transform;

                    if (y != 0)
                    {
                        rock.GetComponentInChildren<StencilShadow>(true)?.DisableShadow();
                    }

                    yield return timer;
                }
            }
        }
    }
}
