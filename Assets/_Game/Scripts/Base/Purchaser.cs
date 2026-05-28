using UnityEngine.Purchasing;
public sealed class Purchaser : MonoSingleton<Purchaser>
{
    public readonly string m_pack01_unlock_slots = "pack01_unlock_slots",
                    m_pack02_unlock_base = "pack02_unlock_base",
                    normal_tickit_pack = "normal_tickit_pack",
                    big_tickit_pack = "big_tickit_pack"


        ;


    public IAPManager IAP_Manager
    {
        get
        {
            return IAPManager.Instance;
        }
        private set { }
    }

    private void Start()
    {
        IAP_Manager.Initialize(
            new ProductDefinition(m_pack01_unlock_slots, ProductType.NonConsumable),
              new ProductDefinition(normal_tickit_pack, ProductType.Consumable),
                new ProductDefinition(big_tickit_pack, ProductType.Consumable)
        );


    }

}