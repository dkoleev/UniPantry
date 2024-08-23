# UniPantry
Unity wrapper for the Pantry API - https://getpantry.cloud

## Getting started
Install via UPM package with git reference or asset package(UniPantry.*.*.*.unitypackage) available in [UniPantry/releases](https://github.com/dkoleev/UniPantry/releases).

UPM Package
---
### Install via git URL
Use [UPM](https://docs.unity3d.com/Manual/upm-ui.html) to install the package via the following git URL: 

```
https://github.com/dkoleev/UniPantry.git
```

![](https://gyazo.com/8c8fc97345fc64f53d62814cce571974.gif)

Usage
---
Create a new client
---
```c#
using Yogi.UniPantry.Runtime;

var pantry = new Pantry();
```

Get Pantry
---
Given a PantryID, return the details of the pantry, including a list of baskets currently stored inside it.

```c#
using Yogi.UniPantry.Runtime;

private void GetPantry() {
    StartCoroutine(
        pantry.GetPantry(pantryId,
            info => {
                if (info is not null) {
                    Debug.Log($"name: {info.name} description: {info.description} percentFull: {info.percentFull} errors: {info.errors}");
                    foreach (var infoBasket in info.baskets) {
                        Debug.Log(infoBasket.name);
                    }
                }
            })
    );
}
```

Update Pantry Details
---

```c#
using Yogi.UniPantry.Runtime;

private void UpdatePantryInfo() {
    StartCoroutine(
        pantry.UpdatePantryInfo(pantryId, "new name", "new description",
            info => {
                if (info is not null) {
                    Debug.Log($"name: {info.name} description: {info.description} percentFull: {info.percentFull} errors: {info.errors}");
                    foreach (var infoBasket in info.baskets) {
                        Debug.Log(infoBasket.name);
                    }
                }
            })
    );
}
```

Get Basket
---
Given a basket name, return the full contents of the basket.

```c#
using Yogi.UniPantry.Runtime;

private void GetBasket() {
    StartCoroutine(_pantry.GetBasket(pantryId, basketId, Debug.Log));
}
```

Create Basket
---
Given a basket name as provided in the url, this will either create a new basket inside your pantry, or replace an existing one.

```c#
using Yogi.UniPantry.Runtime;

private void CreateBasket() {
    var content = JsonUtility.ToJson(SOME_SERIALIZABLE_CLASS);
    StartCoroutine(_pantry.CreateBasket(pantryId, basketId, content, Debug.Log));
}
```

Update Basket
---
Given a basket name, this will update the existing contents and return the contents of the newly updated basket. This operation performs a deep merge and will overwrite the values of any existing keys, or append values to nested objects or arrays.

```c#
using Yogi.UniPantry.Runtime;

private void UpdateBasket() {
    var content = JsonUtility.ToJson(SOME_SERIALIZABLE_CLASS);
    StartCoroutine(_pantry.UpdateBasket(pantryId, basketId, content, Debug.Log));
}
```

Delete Basket
---
Delete the entire basket. Warning, this action cannot be undone.

```c#
using Yogi.UniPantry.Runtime;

private void DeleteBasket() {
    StartCoroutine(_pantry.DeleteBasket(pantryId, basketId, Debug.Log));
}
```






