using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public int  weaponIndex = 0;
    InputManager input;
    [SerializeField] Recoil_Sway_ADS recoilScript;
    Sniper sniperScript;

    [Header("Throwable config")]
    GameObject heldGrenade = null;
    [SerializeField] GameObject primeThrowable;
    [SerializeField] GameObject secondThrowable;
    [SerializeField] Transform holdPoint;
    [SerializeField] float throwForce = 15;

    int secondCounter = 0;
    int primCounter = 0;
    int maxThrowables = 2;
    bool isHolding = false;
    GunScript gunScript;
    private void Start()
    {
        input = GetComponent<InputManager>();
        sniperScript = FindAnyObjectByType<Sniper>();
        SelectWeapon(weaponIndex);
    }

    private void Update()
    {
        if (input.onFoot.SwitchNextWeapon.triggered)
        {
            weaponIndex++;

            if(weaponIndex >= weaponHolder.childCount)
            {
                weaponIndex = 0;
            }

            SelectWeapon(weaponIndex);
        }
        else if (input.onFoot.SwitchLastWeapon.triggered)
        {
            weaponIndex--;

            if (weaponIndex < 0)
            {
                weaponIndex = weaponHolder.childCount -1;
            }
            SelectWeapon(weaponIndex);
        }

        //checks if player has mouse
        if(Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if(scroll > 0)
            {
                // Scroll up = next weapon
                weaponIndex++;
                if (weaponIndex >= weaponHolder.childCount)
                {
                    weaponIndex = 0;
                }
                SelectWeapon(weaponIndex);
            }
            else if(scroll < 0)
            {
                // Scroll down = previous weapon
                weaponIndex--;
                if (weaponIndex < 0)
                {
                    weaponIndex = weaponHolder.childCount - 1;
                }
                SelectWeapon(weaponIndex);
            }
        }

        //check if player grabs 1st throwable
        if (input.onFoot.PrimairyThrowAble.IsPressed()&& !isHolding)
        {
            if(primCounter < maxThrowables)
            {
                primCounter++;
                isHolding = true;
                StartHoldingGrenade(primeThrowable);
            }
        }
        if (input.onFoot.PrimairyThrowAble.WasReleasedThisFrame())
        {
            ThrowGrenade();
            isHolding = false;
        }

        //check if player grabs 2nd throwable
        if (input.onFoot.SecondairyThrowAble.IsPressed() && !isHolding)
        {
            if (secondCounter < maxThrowables)
            {
                secondCounter++;
                isHolding = true;
                StartHoldingGrenade(secondThrowable);
            }
        }
        if (input.onFoot.SecondairyThrowAble.WasReleasedThisFrame())
        {
            ThrowGrenade();
            isHolding = false;
        }

        //when holding throwable move gun down
        if (isHolding)
        {
            //loops over weapons
            for (int i = 0; i < weaponHolder.childCount; i++)
            {
                gunScript = weaponHolder.GetChild(i).GetComponent<GunScript>();
                gunScript.MoveToReloadPos();
            }
        }
    }


    void SelectWeapon(int index)
    {
        //loops over weapons
        for(int i =0; i < weaponHolder.childCount; i++)
        {
            bool active = (i == index);
            //enables the currently selected gun
            weaponHolder.GetChild(i).gameObject.SetActive(active);

            //resets gun position to hip when switching
            GunScript gunScript = weaponHolder.GetChild(i).GetComponent<GunScript>();
            gunScript.ResetGunPos();

            //checks if sniper is active and disables scope
            if (sniperScript != null)
            {
                sniperScript.SetScopeAlpha(0);
                sniperScript.DisablePostEffects();
                sniperScript.ShowGun();
                sniperScript.ResetGunPos();
            }

            if (active)
            {
                //reset postition/ rotation
                weaponHolder.transform.localPosition = Vector3.zero;
                weaponHolder.transform.localRotation = Quaternion.identity;
            }
        }

        weaponIndex = index;
    }

    void StartHoldingGrenade(GameObject prefab)
    {
        if (heldGrenade == null)
        {
            heldGrenade = Instantiate(prefab, holdPoint.position, holdPoint.rotation);
            heldGrenade.transform.SetParent(holdPoint);
            // Optional: disable physics so it doesn’t fall while held
            Rigidbody rb = heldGrenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    void ThrowGrenade()
    {
        if (heldGrenade != null)
        {
            // Unparent so it’s free
            heldGrenade.transform.SetParent(null);

            Rigidbody rb = heldGrenade.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                // Apply force forward relative to player or holdPoint
                rb.AddForce(holdPoint.forward * throwForce, ForceMode.VelocityChange);
            }

            heldGrenade = null;
            gunScript.ResetGunPos();
        }
    }
}
