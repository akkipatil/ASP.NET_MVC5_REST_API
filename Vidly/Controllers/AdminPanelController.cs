using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class AdminPanelController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public AdminPanelController()
        {
            //AdminUserDetail = new AdminUser();
        }
        // GET: AdminPanel
        // GET: Employee  
        public ActionResult Index()
        {
            logger.Info("Index: Called" + Environment.NewLine + DateTime.Now);
            return View();
        }

        #region Get Admin Detail For LoggedIn Admin
        public ActionResult AdminDetails(AdminPanelViewModel viewModel)
        {
            logger.Info("AdminDetails: Called" + Environment.NewLine + DateTime.Now);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49333/api/");
                var responseTask = client.GetAsync("Users/GetAdmin/" + viewModel .AdminUser.AdminUserName+ "/" + viewModel.AdminUser.AdminPassword + "");    // Http Get

                logger.Info("AdminDetails: Users/GetAdmin/User/Pass :Called" + Environment.NewLine + DateTime.Now);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info("AdminDetails: Users/GetAdmin/User/Pass :Success" + Environment.NewLine + DateTime.Now);

                    string readTask = result.Content.ReadAsStringAsync().Result;

                    try
                    {
                        AdminUser l_cAdminInfo = JsonConvert.DeserializeObject<AdminUser>(readTask);
                        if (null != (l_cAdminInfo))
                        {
                            //Session["AdminPassword"] = l_cAdminInfo.AdminPassword;
                            Session["AdminUserId"] = l_cAdminInfo.AdminUserId;
                            Session["AdminUserName"] = l_cAdminInfo.AdminUserName;

                            return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.Error("AdminDetails: Users/GetAdmin/User/Pass: Json DeserializeObject Error: "+ex.Message + Environment.NewLine + DateTime.Now);
                    }

                }
            }
            return View("Index");
        }
        #endregion

        #region Redirect to UserDashboard
        public ActionResult UserDashBoard()
        {
            if (Session["AdminUserName"] != null)
            {
                logger.Info("UserDashBoard: Redirect GetAllUsersFroAdmin" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
            }
            else
            {
                logger.Info("UserDashBoard: Redirect Index" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Get All User For Specific Admin
        public ActionResult GetAllUsersFroAdmin()
        {
            if (null == Session["AdminUserId"])
            {
                logger.Error("GetAllUsersFroAdmin: AdminUserId(Sesion Invalid)" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:49333/api/");
                var responseTask = client.GetAsync("Users/GetAllUsersForAdmin/" + Session["AdminUserId"]);    // Http Get

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                logger.Info("GetAllUsersFroAdmin: GetAllUsersForAdmin/Id :Called" + Environment.NewLine + DateTime.Now);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    logger.Info("GetAllUsersFroAdmin: GetAllUsersForAdmin/Id :Success" + Environment.NewLine + DateTime.Now);
                    string readTask = result.Content.ReadAsStringAsync().Result;
                    try
                    {
                        List<User> l_cUserUnderAdmin = JsonConvert.DeserializeObject<List<User>>(readTask);
                        if (null != (l_cUserUnderAdmin))
                        {
                            logger.Info("GetAllUsersFroAdmin: Passing UserData To UserDashBoard" + Environment.NewLine + DateTime.Now);
                            AdminPanelViewModel AdminPanelVM = new AdminPanelViewModel() { UserListForAdmin = l_cUserUnderAdmin };
                            return View("UserDashBoard", AdminPanelVM);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error("GetAllUsersFroAdmin: Exception Ocured While JsonConvert.DeserializeObject" + ex.Message + Environment.NewLine + DateTime.Now);
                    }
                }
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit User
        public ActionResult EditUser(int id)
        {
            if (Session["AdminUserName"] != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49333/api/");
                    var responseTask = client.GetAsync("Users/GetUser/" + id);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    logger.Info("EditUser: Api called Users/GetUser/id for user edit" + Environment.NewLine + DateTime.Now);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        logger.Info("EditUser: Api called Users/GetUser/id for user edit: Success" + Environment.NewLine + DateTime.Now);
                        string readTask = result.Content.ReadAsStringAsync().Result;
                        try
                        {
                            User l_cUserUnderAdmin = JsonConvert.DeserializeObject<User>(readTask);
                            if (null != (l_cUserUnderAdmin))
                            {
                                logger.Info("EditUser:Passing UserObject to edit to NewUserForm" + Environment.NewLine + DateTime.Now);
                                return View("NewUserForm", new AdminPanelViewModel() { SelectedUser = l_cUserUnderAdmin });
                                //AdminPanelViewModel AdminPanelVM = new AdminPanelViewModel() { UserListForAdmin = l_cUserUnderAdmin };
                                // return View("UserDashBoard", AdminPanelVM);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Info("EditUser:Exception while JsonConvert.DeserializeObject"+ ex.Message + Environment.NewLine + DateTime.Now);
                        }
                    }
                }
                return View("NewUserForm", new AdminPanelViewModel() { SelectedUser = new Models.User() { UserId = id} });
            }
            else
            {
                logger.Info("EditUser: AdminUserId(Sesion Invalid)" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Delete User
        public ActionResult DeleteUser(int id)
        {
            logger.Info("DeleteUser Action Called" + Environment.NewLine + DateTime.Now);
            if (Session["AdminUserName"] != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49333/api/");
                    var responseTask = client.DeleteAsync("Users/DeleteUser/" + id); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    logger.Info("DeleteUser: delete user api called Users/DeleteUser/" + id + Environment.NewLine + DateTime.Now);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        logger.Info("DeleteUser: Delection Success for UserUsers/DeleteUser/" + id + Environment.NewLine + DateTime.Now);
                        return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
                    }
                    logger.Info("DeleteUser: Delection Failed  for UserUsers/DeleteUser/" + id + Environment.NewLine + DateTime.Now);
                }

                return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
            }
            else
            {
                logger.Info("Session invalid redirecting to Index" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Open Add_New_User / Ass New User Operation
        public ActionResult OpenNewUserFroAdmin()
        {
            logger.Info("OpenNewUserFroAdmin: Action called" + Environment.NewLine + DateTime.Now);

            if (Session["AdminUserName"] != null)
            {
                logger.Info("OpenNewUserFroAdmin: Redirecting to NewUserForm" + Environment.NewLine + DateTime.Now);
                return View("NewUserForm");
            }
            else
            {
                logger.Info("OpenNewUserFroAdmin: Redirecting to Index as User Session Invalid" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index", "AdminPanel");
            }
        }
        public ActionResult AddNewUser(AdminPanelViewModel a_cAdminPanelViewModel)
        {
            logger.Info("AddNewUser: Action Called" + Environment.NewLine + DateTime.Now);
            if (Session["AdminUserName"] != null)
            {
                if(null != a_cAdminPanelViewModel && null != a_cAdminPanelViewModel.SelectedUser)
                {
                    if(a_cAdminPanelViewModel.SelectedUser.UserId == 0)
                    {
                        a_cAdminPanelViewModel.SelectedUser.AdminUserId = System.Convert.ToInt32(Session["AdminUserId"]);

                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri("http://localhost:49333/api/");
                            //var responseTask = client.DeleteAsync("Users/CreateUser");    // Http Get
                            var postTask = client.PostAsJsonAsync<User>("Users/CreateUser", a_cAdminPanelViewModel.SelectedUser);

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            logger.Info("AddNewUser: Api called Users/CreateUser" + Environment.NewLine + DateTime.Now);

                            postTask.Wait();
                            var result = postTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                logger.Info("AddNewUser: Api called Users/CreateUser : Success" + Environment.NewLine + DateTime.Now);
                                return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
                            }
                            logger.Info("AddNewUser: Api called Users/CreateUser : Failed" + Environment.NewLine + DateTime.Now);
                        }
                    }
                    else
                    {
                        a_cAdminPanelViewModel.SelectedUser.AdminUserId = System.Convert.ToInt32(Session["AdminUserId"]);
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri("http://localhost:49333/api/");
                            //var responseTask = client.DeleteAsync("Users/CreateUser");    // Http Get
                            var putTask = client.PutAsJsonAsync<User>("Users/UpdateUser/"+ a_cAdminPanelViewModel.SelectedUser.UserId, a_cAdminPanelViewModel.SelectedUser);

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            logger.Info("AddNewUser: Api called Users/UpdateUser : Called" + Environment.NewLine + DateTime.Now);

                            putTask.Wait();
                            var result = putTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                logger.Info("AddNewUser: Api called Users/UpdateUser : Success" + Environment.NewLine + DateTime.Now);
                                return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
                            }
                            logger.Info("AddNewUser: Api called Users/UpdateUser : Failed" + Environment.NewLine + DateTime.Now);
                        }
                    }
                }
                return RedirectToAction("GetAllUsersFroAdmin", "AdminPanel");
            }
            else
            {
                return RedirectToAction("Index", "AdminPanel");
            }
        }
        #endregion

        #region Logout Admin
        public ActionResult LogOutAdmin()
        {
            logger.Info("LogOutAdmin: Action Called" + Environment.NewLine + DateTime.Now);

            if (Session["AdminUserName"] != null)
            {
                logger.Info("LogOutAdmin: cleaning session data" + Environment.NewLine + DateTime.Now);

                Session["AdminPassword"] = null;
                Session["AdminUserId"] = null;
                Session["AdminUserName"] = null;

                logger.Info("LogOutAdmin: redirect to Index" + Environment.NewLine + DateTime.Now);

                return RedirectToAction("Index", "AdminPanel");
            }
            return RedirectToAction("Index", "AdminPanel");
        }
        #endregion

        #region Redirect to UserDashboard
        public ActionResult LogView()
        {
            logger.Info("LogView: Action Called" + Environment.NewLine + DateTime.Now);

            if (Session["AdminUserName"] != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:49333/api/");
                    var responseTask = client.GetAsync("Logs/GetAllLogs");    // Http Get

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    logger.Info("LogView: Logs/GetAllLogs API :Called" + Environment.NewLine + DateTime.Now);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        logger.Info("LogView: Logs/GetAllLogs API :Success" + Environment.NewLine + DateTime.Now);
                        string readTask = result.Content.ReadAsStringAsync().Result;
                        try
                        {
                            List<OperationLog> l_cUserUnderAdmin = JsonConvert.DeserializeObject<List<OperationLog>>(readTask);
                            if (null != (l_cUserUnderAdmin))
                            {
                                logger.Info("LogView: Passing UserData To UserDashBoard" + Environment.NewLine + DateTime.Now);
                                AdminPanelViewModel AdminPanelVM = new AdminPanelViewModel() { OperationLogs = l_cUserUnderAdmin };
                                return View("LogView", AdminPanelVM);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("LogView: Exception Ocured While JsonConvert.DeserializeObject" + ex.Message + Environment.NewLine + DateTime.Now);
                        }
                    }
                }

                return RedirectToAction("Index", "AdminPanel");
            }
            else
            {
                logger.Info("LogView: Session Null Redirect Index" + Environment.NewLine + DateTime.Now);
                return RedirectToAction("Index");
            }
        }
        #endregion
    }
}