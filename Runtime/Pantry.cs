using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Yogi.UniPantry.Runtime {
    public class Pantry {
        /// <summary>
        /// Given a PantryID, return the details of the pantry, including a list of baskets currently stored inside it.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <param name="result">Callback with PantryInfo</param>
        public IEnumerator GetPantry(string pantryId, Action<PantryInfo> result) {
            using var request = UnityWebRequest.Get(GetUrl(pantryId));
            SetHeaders(request);
        
            yield return request.SendWebRequest();

            var success = IsRequestSuccess(request, $"Error while getting info");
            if (success) {
                var jsonData = JsonUtility.FromJson<PantryInfo>(request.downloadHandler.text);
                result?.Invoke(jsonData);
            } else {
                result?.Invoke(null);
            }
        }

        public IEnumerator UpdatePantryInfo(string pantryId, string newName, string newDescription, Action<PantryInfo> result) {
            var infoObject = new PantryInfoCommon {
                name = newName,
                description = newDescription
            };
            var json = JsonUtility.ToJson(infoObject);

            using var request = UnityWebRequest.Put(GetUrl(pantryId), json);
            SetHeaders(request);
        
            yield return request.SendWebRequest();
        
            var success = IsRequestSuccess(request, $"Error while updating pantry info");
            if (success) {
                var jsonData = JsonUtility.FromJson<PantryInfo>(request.downloadHandler.text);
                result?.Invoke(jsonData);
            } else {
                result?.Invoke(null); 
            }
        }

        /// <summary>
        /// Given a basket name, return the full contents of the basket as a string. It's up to you to deserialize it.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <param name="basketId">Basket ID</param>
        /// <param name="result">Callback with basket's data</param>
        /// <returns></returns>
        public IEnumerator GetBasket(string pantryId, string basketId, Action<string> result) {
            using var request = UnityWebRequest.Get(GetUrl(pantryId, basketId));
            SetHeaders(request);
            yield return request.SendWebRequest();
            var success = IsRequestSuccess(request, $"Error while getting basket {basketId} data");
            result?.Invoke(success ? request.downloadHandler.text : null);
        }

        /// <summary>
        /// Given a basket name, this will update the existing contents and return the contents of the newly updated basket.
        /// This operation performs a deep merge and will overwrite the values of any existing keys, or append values to nested objects or arrays.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <param name="basketId">Basket ID</param>
        /// <param name="content">JSON to be uploaded</param>
        /// <param name="result">Callback with result from the server</param>
        public IEnumerator UpdateBasket(string pantryId, string basketId, string content, Action<string> result = null) {
            using var request = UnityWebRequest.Put(GetUrl(pantryId, basketId), content);
            SetHeaders(request);
            yield return request.SendWebRequest();
            var success = IsRequestSuccess(request, $"Error while putting data to basket {basketId}");
            result?.Invoke(success ? request.downloadHandler.text : null);
        }

        /// <summary>
        /// Given a basket name as provided in the url, this will either create a new basket inside your pantry, or replace an existing one.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <param name="basketId">Basket ID</param>
        /// <param name="content">JSON to be uploaded</param>
        /// <param name="result">Callback with result from the server</param>
        /// <returns></returns>
        public IEnumerator CreateBasket(string pantryId, string basketId, string content, Action<string> result = null) {
            using var request = UnityWebRequest.Post(GetUrl(pantryId, basketId), content, "application/json");
            yield return request.SendWebRequest();
            var success = IsRequestSuccess(request, $"Error while create basket {basketId}");
            result?.Invoke(success ? request.downloadHandler.text : null);
        }

        /// <summary>
        /// Delete the entire basket. Warning, this action cannot be undone.
        /// </summary>
        /// <param name="pantryId">Your pantry ID</param>
        /// <param name="basketId">Basket ID</param>
        /// <param name="result">Result of action</param>
        /// <returns></returns>
        public IEnumerator DeleteBasket(string pantryId, string basketId, Action<string> result = null) {
            using var request = UnityWebRequest.Delete(GetUrl(pantryId, basketId));
            SetHeaders(request);
            yield return request.SendWebRequest();
            IsRequestSuccess(request, $"Error while delete basket {basketId}");
            result?.Invoke(request.result.ToString());
        }

        private void SetHeaders(UnityWebRequest request) {
            request.SetRequestHeader("Content-Type", "application/json");
        }

        private bool IsRequestSuccess(UnityWebRequest request, string errorMessage) {
            if (request.result == UnityWebRequest.Result.Success) {
                return true;
            }
        
            Debug.LogError($"[Pantry]: {errorMessage}: {request.result}");
            return false;
        }
    
        private string GetUrl(string pantryId) {
            return $"https://getpantry.cloud/apiv1/pantry/{pantryId}";
        }

        private string GetUrl(string pantryId, string basketId) {
            return $"https://getpantry.cloud/apiv1/pantry/{pantryId}/basket/{basketId}";
        }
    
        [Serializable]
        public class PantryInfoCommon {
            public string name;
            public string description;
        }

        [Serializable]
        public class PantryInfo {
            public string name;
            public string description;
            public List<object> errors;
            public bool notifications;
            public int percentFull;
            public List<BasketInfo> baskets;
        }
    
        [Serializable]
        public class BasketInfo {
            public string name;
            public string ttl;
        }
    }
}
