using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BaseRecipesCounter<T> : BaseCounter where T: BaseRecipeSO
{
    [SerializeField] protected T[] recipeSOArray;

    protected virtual bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return !!GetRecipeSOWithInput(inputKitchenObjectSO);
    }

    protected virtual KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return GetRecipeSOWithInput(inputKitchenObjectSO)?.output ?? null;
    }

    protected virtual T GetRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        return recipeSOArray.FirstOrDefault(item => item.input == inputKitchenObjectSO);
    }
}
