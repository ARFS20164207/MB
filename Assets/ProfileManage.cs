using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ProfileManage
{

    [SerializeField] public List<Profile> profiles { get; set; }

    public static ProfileManage instance {get; set;}

    // Start is called before the first frame update
    public void Initialize();

    public void AddProfile(Profile profile);

    public abstract void SetTurns();

    public abstract Profile GetProfile(int index);
}
