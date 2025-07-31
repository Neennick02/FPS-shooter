using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public int  weaponIndex = 0;
    InputManager input;
    [SerializeField] Recoil_Sway_ADS recoilScript;
    Sniper sniperScript;
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
}
