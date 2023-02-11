using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CuttingCounter : BaseRecipesCounter<CuttingRecipeSO>, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject() && (player.GetKitchenObject().GetKitchenObjectSO()).isCuttable)
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;

                CuttingRecipeSO cuttingRecipeSO = GetRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                this.GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAternate(Player player)
    {
        if (HasKitchenObject() && GetKitchenObject().GetKitchenObjectSO().isCuttable)
        {
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);

            CuttingRecipeSO cuttingRecipeSO = GetRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }
}
