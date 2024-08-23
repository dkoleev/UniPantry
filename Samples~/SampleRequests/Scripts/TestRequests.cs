using UnityEngine;
using UnityEngine.UI;
using Yogi.UniPantry.Runtime;

namespace UniPantry.Samples.SampleRequests.Scripts {
    public class TestRequests : MonoBehaviour {
        [SerializeField] private string pantryId;
        [SerializeField] private string basketId;
        [Space]
        [SerializeField] private Button getPantry;
        [SerializeField] private Button updatePantryInfo;
        [SerializeField] private Button getBasket;
        [SerializeField] private Button updateBasket;
        [SerializeField] private Button createBasket;
        [SerializeField] private Button deleteBasket;

        private Pantry _pantry;
        private Data.Resources _resources;
    
        private void Awake() {
            _pantry = new Pantry();
            InitializeData();

        }

        private void InitializeData() {
            _resources = new Data.Resources();
            _resources.resources.Add("energy");
        }

        private void OnEnable() {
            getPantry.onClick.AddListener(GetPantry);
            getBasket.onClick.AddListener(GetBasket);
            updateBasket.onClick.AddListener(UpdateBasket);
            createBasket.onClick.AddListener(CreateBasket);
            deleteBasket.onClick.AddListener(DeleteBasket);
            updatePantryInfo.onClick.AddListener(UpdatePantryInfo);
        }

        private void OnDisable() {
            getPantry.onClick.RemoveListener(GetPantry);
            getBasket.onClick.RemoveListener(GetBasket);
            updateBasket.onClick.RemoveListener(UpdateBasket);
            createBasket.onClick.RemoveListener(CreateBasket);
            deleteBasket.onClick.RemoveListener(DeleteBasket);
            updatePantryInfo.onClick.RemoveListener(UpdatePantryInfo);
        }

        private void GetPantry() {
            StartCoroutine(
                _pantry.GetPantry(pantryId,
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
    
        private void UpdatePantryInfo() {
            StartCoroutine(
                _pantry.UpdatePantryInfo(pantryId, "configs", "game configuration",
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
    
        private void GetBasket() {
            StartCoroutine(_pantry.GetBasket(pantryId, basketId, Debug.Log));
        }
    
        private void UpdateBasket() {
            var content = JsonUtility.ToJson(_resources);
            StartCoroutine(_pantry.UpdateBasket(pantryId, basketId, content, Debug.Log));
        }

        private void CreateBasket() {
            var content = JsonUtility.ToJson(_resources);
            StartCoroutine(_pantry.CreateBasket(pantryId, basketId, content, Debug.Log));
        }

        private void DeleteBasket() {
            StartCoroutine(_pantry.DeleteBasket(pantryId, basketId, Debug.Log));
        }
    }
}
