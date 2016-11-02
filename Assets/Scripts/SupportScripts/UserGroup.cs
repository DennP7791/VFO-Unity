using UnityEngine;
using System.Collections;

public class UserGroup {

    public int Id;
    public string GroupName;
    public int? CustomerId;

    public UserGroup(int id, string groupName, int? customerId)
    {
        Id = id;
        GroupName = groupName;
        CustomerId = customerId;
    }
}
