using UnityEngine;
using System.Collections;

public class UserGroupVideoCredential {

    public int Id;
    public int VideoCategoryId;
    public int UserGroupId;
    public string Password;
    public string Salt;

    public UserGroupVideoCredential(int id, int videoCategoryId, int userGroupId, string password, string salt)
    {
        Id = id;
        VideoCategoryId = videoCategoryId;
        UserGroupId = userGroupId;
        Password = password;
        Salt = salt;
    }
}
