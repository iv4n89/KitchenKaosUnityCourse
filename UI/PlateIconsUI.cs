using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private Transform iconTemplate;

    void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in transform)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        plateKitchenObject.GetKitchenObjectSOList()
            .ForEach(element =>
            {
                Transform iconTransform = Instantiate(iconTemplate, transform);
                iconTransform.gameObject.SetActive(true);
                iconTransform.GetComponent<PlateIconSingleUI>().SetKitchenObjectSO(element);
            });
    }
}
