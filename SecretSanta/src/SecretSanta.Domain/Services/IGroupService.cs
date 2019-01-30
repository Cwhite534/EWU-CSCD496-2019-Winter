using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {

        Group AddGroup(Group group);

        Group UpdateGroup(Group group);

        Group RemoveGroup(Group group);

        User AddUserToGroup(int groupId, User user);

        User RemoveUserFromGroup(int groupId, User user);

        List<User> FetchAllUsersInGroup(int groupId);

        List<Group> FetchAllGroups();
    }
}