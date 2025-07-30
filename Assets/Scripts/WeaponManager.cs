using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponHolder;
    public int  weaponIndex = 0;
    InputManager input;
    [SerializeField] Recoil_Sway_ADS recoilScript;

    private void Start()
    {
        input = GetComponent<InputManager>();
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
    }

    void SelectWeapon(int index)
    {
        //loops over weapons
        for(int i =0; i < weaponHolder.childCount; i++)
        {
            bool active = (i == index);
            //enables the currently selected gun
            weaponHolder.GetChild(i).gameObject.SetActive(active);

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
