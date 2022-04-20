using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Authorization
{
    public class Policies
    {
        ///<summary>Policy to allow viewing all user records.</summary>
        public const string ViewAllUsersPolicy = "View All Users";

        ///<summary>Policy to allow adding, removing and updating all user records.</summary>
        public const string ManageAllUsersPolicy = "Manage All Users";

        /// <summary>Policy to allow viewing details of all roles.</summary>
        public const string ViewAllRolesPolicy = "View All Roles";

        /// <summary>Policy to allow viewing details of all or specific roles (Requires roleName as parameter).</summary>
        public const string ViewRoleByRoleNamePolicy = "View Role by RoleName";

        /// <summary>Policy to allow adding, removing and updating all roles.</summary>
        public const string ManageAllRolesPolicy = "Manage All Roles";
         
        /// <summary>  Policy to view  all  geo locations.  </summary>
        public const string ViewGeoLocationsPolicy = "View All Geo Locations";

        /// <summary>  Policy to manage all  geo locations.  </summary>
        public const string ManageGeoLocationsPolicy = "Manage All Geo Locations";

  
    }


 
   
}