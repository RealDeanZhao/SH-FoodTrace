﻿using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTrace.IDBAccess
{
    public interface IUserRoleAccess
    {
        MessageModel InsertSingleEntity(UserRoleModel model);

        List<UserRoleModel> GetUserRefRoleByUid(int id);

    }
}
