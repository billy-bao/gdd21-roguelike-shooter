using UnityEngine;
using System.Collections;

public class SecretLevelManager : LevelManager
{
    private bool inited = false;
    public Item bonusItem;
    public Transform bonusItemSpawn;

    void Start()
    {
        if (inited) return;
        if(flags is null) //single level testing
        {
            Item it = Instantiate(bonusItem, bonusItemSpawn.position, Quaternion.identity);
            it.id = 1;
        }
    }
    public override void Initialize(LevelFlags flags, Player player, int dir)
    {
        inited = true;
        base.Initialize(flags, player, dir);
        if(flags.customFlags is null || !(bool)flags.customFlags)
        {
            Item it = Instantiate(bonusItem, bonusItemSpawn.position, Quaternion.identity);
            it.id = 1;
        }
    }

    public override void OnItemPickup(Item i)
    {
        if (flags != null && i.id == 0)
        {
            flags.droppedItem = null;
        }
        else if (flags != null && i.id == 1)
        {
            flags.customFlags = true;
        }
    }
}
