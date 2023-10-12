using System.Linq;
using UnityEngine;

public class BlockGenerationSystem : MonoBehaviour, ISystem
{
    public GameObject block_L;
    
    public void OnFrameUpdate()
    {
        
    }

    public void OnSystemUpdate()
    {
        var allBlocks = FindObjectsOfType<BlockComponent>().ToList();
        var allBlocksGrounded = true;
        foreach (var block in allBlocks)
        {
            var gravityComponent = block.GetComponent<GravityComponent>();
            if (gravityComponent != null && gravityComponent.isGrounded == false)
                allBlocksGrounded = false;
        }

        if (allBlocksGrounded)
        {
            Instantiate(block_L);
            //...
        }
    }
}