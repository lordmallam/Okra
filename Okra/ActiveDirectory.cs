using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace Okra.ActiveDirectory
{
    public class ActiveDirectory
    {
        private String _path;
        private String _filterAttribute;
        private String _container;
        private String _ldapContainer;
        private String _adminuser;
        private String _password;
        private String _server;
        private String _domain;
        private String _userPath;

        public ActiveDirectory(String path, String ldapContainer, String AdminUser, String Password, String Server,String Domain)
        {
            _path = path;
            _ldapContainer =ldapContainer;
            _adminuser = AdminUser ;
            _password =Password;
            _server = Server;
            _domain = Domain;
        }
        public ActiveDirectory(String path, String ldapContainer, String Server, String Domain)
        {
            _path = path;
            _ldapContainer = ldapContainer;
            _server = Server;
            _domain = Domain;
        }
        public ActiveDirectory(String path, String Domain)
        {
            _path = path;
            _domain = Domain;
        }
        public ActiveDirectory()
        {

        }

        public bool IsAuthenticated (String username, String pwd)
        {
            String domainAndUsername = _domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {	//Bind to the native AdsObject to force authentication.			
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }

                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (String)result.Properties["cn"][0];
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            return true;
        }

        public String GetGroups(String username, String pwd)
        {
            String domainAndUsername = _domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();

                int propertyCount = result.Properties["memberOf"].Count;

                String dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (String)result.Properties["memberOf"][propertyCounter];

                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }

                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");

                    DirectoryEntry gentry = new DirectoryEntry(_path, domainAndUsername, pwd);
                    DirectorySearcher gsearch = new DirectorySearcher(gentry);

                    gsearch.Filter = "(cn=" + dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1) + ")";
                    gsearch.PropertiesToLoad.Add("memberOf");

                    try
                    {
                    SearchResult gresult = gsearch.FindOne();

                    int gpropertyCount = gresult.Properties["memberOf"].Count;

                    for (int gcounter = 0; gcounter < gpropertyCount; gcounter++) {
                        dn = (String)gresult.Properties["memberOf"][gcounter];

                        equalsIndex = dn.IndexOf("=", 1);
                        commaIndex = dn.IndexOf(",", 1);
                        if (-1 == equalsIndex)
                        {
                            return null;
                        }

                        groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                        groupNames.Append("|");
                    }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }

        public Principal GetUserInfo(string username, string password){
            Principal newuser = null;
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server ,username,password);

            // define a "query-by-example" principal - here, we search for a UserPrincipal 


            //UserPrincipal qbeUserCT = new UserPrincipal(ctx, username, password, true);

            UserPrincipal qbeUser = new UserPrincipal(ctx);
            qbeUser.SamAccountName = username;
            PrincipalSearcher ps = new PrincipalSearcher(qbeUser);
            PrincipalSearchResult<Principal> psr = ps.FindAll();


                // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....          


            if (psr != null)
                {

                    foreach (var item in psr)
                    {
                        newuser = item;
                    }
                   
                }
           
            return newuser;

        }

        public List<String> GetAllGroupsFromAD(string username, string password) {
            

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server,_ldapContainer ,username,password);

            // define a "query-by-example" principal - here, we search for a GroupPrincipal 
            GroupPrincipal qbeGroup = new GroupPrincipal(ctx);

            // create your principal searcher passing in the QBE principal    
            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            // find all matches

            List<String> result = new List<String>();
            foreach (var found in srch.FindAll())
            {
                // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....   
                result.Add(((GroupPrincipal)found).Name);
            }
            
            result.Sort();

            return result;
        }

        public List<ADGroups> GetGroupsByOrg(string username, string password, string Org)
        {List<ADGroups> result = new List<ADGroups>();

            try
            {
PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server, "OU=" + Org +","+ _ldapContainer, username, password);

            // define a "query-by-example" principal - here, we search for a GroupPrincipal
            GroupPrincipal qbeGroup = new GroupPrincipal(ctx);

            // create your principal searcher passing in the QBE principal    
            PrincipalSearcher srch = new PrincipalSearcher(qbeGroup);

            // find all matches

            
            foreach (var found in srch.FindAll())
            {
                // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....
                ADGroups nGroup = new ADGroups();
                nGroup.Group = ((GroupPrincipal)found).Name;
                nGroup.Description = ((GroupPrincipal)found).Description;

                result.Add(nGroup);
            }
            }
            catch (Exception)
            {
                throw;
            }
            

            

            return result;
        }

        public void CreateOU(string path, string ou, string des) { 
        
                //Create OU on Active Directory
            try
            {
                DirectoryEntry objADAM;  // Binding object.
                DirectoryEntry objOU;    // Organizational unit.
                string strDescription;   // Description of OU.
                string strOU;            // Organiztional unit.
                string strPath;          // Binding path.

                // Construct the binding string.
                strPath = path;

                // Get AD LDS object.

                objADAM = new DirectoryEntry(strPath, _adminuser, _password);
                objADAM.RefreshCache();


                // Specify Organizational Unit.
                strOU = "OU=" + ou;
                strDescription = des;

                // Create Organizational Unit.

                objOU = objADAM.Children.Add(strOU,
                    "OrganizationalUnit");
                if (strDescription != "")
                {
                    objOU.Properties["description"].Add(strDescription);
                }

                objOU.CommitChanges();
            }
            catch (Exception ex1) {
                throw ex1;
            }
        
        }

        public void CreateGroup(string path, string gp, string des)
        {

            //Create OU on Active Directory
            try
            {
                DirectoryEntry objADAM;  // Binding object.
                DirectoryEntry objOU;    // Organizational unit.
                string strDescription;   // Description of OU.
                string strOU;            // Organiztional unit.
                string strPath;          // Binding path.

                // Construct the binding string.
                strPath = path;

                // Get AD LDS object.

                objADAM = new DirectoryEntry(strPath, _adminuser, _password);
                objADAM.RefreshCache();


                strOU = "CN=" + gp;
                strDescription = des;

                // Create Group Unit.

                objOU = objADAM.Children.Add(strOU,
                    "Group");

                if (strDescription != "")
                {
                    objOU.Properties["description"].Add(strDescription);
                }

                objOU.CommitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool IsUsernameAvailable(String username)
        {
            
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server, _ldapContainer,_adminuser, _password);

            // define a "query-by-example" principal - here, we search for a UserPrincipal 



            UserPrincipal qbeUser = new UserPrincipal(ctx);
            qbeUser.SamAccountName = username;
            PrincipalSearcher ps = new PrincipalSearcher(qbeUser);
            PrincipalSearchResult<Principal> psr = ps.FindAll();


            // do whatever here - "found" is of type "Principal" - it could be user, group, computer.....          


            if (psr.Count() != 0)
            {

                return false;
            }
            else
            {
                return true;
            }


            
        }

        public PrincipalSearchResult<Principal> FindUser(String username)
        {

            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server, _ldapContainer, _adminuser, _password);

            // define a "query-by-example" principal - here, we search for a UserPrincipal 



            UserPrincipal qbeUser = new UserPrincipal(ctx);
            qbeUser.SamAccountName = username;
            
            PrincipalSearcher ps = new PrincipalSearcher(qbeUser);
            PrincipalSearchResult<Principal> psr = ps.FindAll();
            return psr;
        }

        public bool SetPassword(string username, string password)
        {
            DirectoryEntry DE = new DirectoryEntry(_path, _adminuser, _password);
            DirectorySearcher search = new DirectorySearcher(DE);

            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();
            
                try
                {
                    DirectoryEntry directoryEntry = new DirectoryEntry(result.Path, _adminuser, _password);


                    directoryEntry.Invoke("SetPassword", new object[] { password });
                    directoryEntry.Properties["LockOutTime"].Value = 0;

                    directoryEntry.Close();
                    return true;
                }
                catch (Exception e)
                {
                Console.Write(e.Message);
                    return false;
                }
            
        }
        public bool SetPassword(string username, string password, string adminuser, string adminpassword)
        {
            DirectoryEntry DE = new DirectoryEntry(_path, _adminuser, _password);
            DirectorySearcher search = new DirectorySearcher(DE);

            search.Filter = "(SAMAccountName=" + username + ")";
            search.PropertiesToLoad.Add("cn");
            SearchResult result = search.FindOne();

            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(result.Path, adminuser, adminpassword);


                directoryEntry.Invoke("SetPassword", new object[] { password });
                directoryEntry.Properties["LockOutTime"].Value = 0;

                directoryEntry.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }

        }


        public List<Principal> GetGroupMembers(String GroupName, String username, String password)
        {
            // set up domain context
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, _server, _ldapContainer, username, password);

                            // find the group in question
                            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, GroupName);
                             List<Principal> members = null;
                            // if found....
                            if (group != null) {
                                members = group.GetMembers().ToList();
                
                            }
                            return members;
            }
            catch (Exception)
            {
                
                throw;
            }
            
                
               
        }




    }

}



