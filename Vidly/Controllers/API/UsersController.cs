using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;
using System.Data.Entity;
using System.Web;
using NLog;

namespace Vidly.Controllers.API
{
    public class UsersController : ApiController
    {
        private readonly ApplicationDbContext _dbContext;
        private static Logger logger = null;
        public UsersController()
        {
            logger = LogManager.GetCurrentClassLogger();
            this._dbContext = new ApplicationDbContext();
        }

        #region Get All Users
        // Get Users
        [HttpGet]
        [Route("api/Users/GetAllUsers")]
        public IHttpActionResult GetUsers()
        {
            IEnumerable<User> UserList = null;
            if (null != _dbContext && null != _dbContext.User)
            {
                logger.Info("GetUsers-> _dbContext or _dbContext.User is null" + Environment.NewLine + DateTime.Now);
                return BadRequest();
            }
            try
            {
                logger.Info("GetUsers-> Success" + Environment.NewLine + DateTime.Now);
                UserList = _dbContext.User.Include(x => x.AdminUser).ToList();
                return Ok(UserList);
            }
            catch(Exception ex)
            {
                logger.Error("GetUsers-> Exception Message: "+ex.Message + Environment.NewLine + DateTime.Now);
            }
            return BadRequest();
        }
        #endregion

        #region Get All LogInfo
        // Get Users
        [HttpGet]
        [Route("api/Logs/GetAllLogs")]
        public IHttpActionResult GetAllLogs()
        {
            IEnumerable<OperationLog> OperationLogList = null;
            if (null == _dbContext && null == _dbContext.User)
            {
                logger.Info("GetAllLogs-> _dbContext or _dbContext.User is null" + Environment.NewLine + DateTime.Now);
                return BadRequest();
            }
            try
            {
                logger.Info("GetAllLogs-> toList From Context Success" + Environment.NewLine + DateTime.Now);
                OperationLogList = _dbContext.OperationLog.OrderByDescending(x => x.timestamp).Take(200);
                return Ok(OperationLogList);
            }
            catch (Exception ex)
            {
                logger.Error("GetAllLogs->  toList From Context Exception Message: " + ex.Message + Environment.NewLine + DateTime.Now);
            }
            return BadRequest();
        }
        #endregion

        #region Get All Admin
        [HttpGet]
        [Route("api/Users/GetAllAdmins")]
        public IHttpActionResult GetAdmin()
        {
            IEnumerable<AdminUser> AdminLIist = null;
            if (null == _dbContext && null == _dbContext.AdminUser)
            {
                logger.Info("GetUsers-> _dbContext or _dbContext.AdminUser is null" + Environment.NewLine + DateTime.Now);
                return BadRequest();
            }
            try
            {
                AdminLIist = _dbContext.AdminUser.ToList();
                logger.Info("GetAdmin-> While Getting All Admin Success" + Environment.NewLine + DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error("GetAdmin-> While Getting All Admin Exception Message: " + ex.Message + Environment.NewLine + DateTime.Now);
            }
            return Ok(AdminLIist);
        }
        #endregion

        #region Get All User For AdminId Passes
        [HttpGet]
        [Route("api/Users/GetAllUsersForAdmin/{id}")]
        public IHttpActionResult GetUsersForAdmin(int id)
        {
            IEnumerable<User> UserList = null;
            try
            {
                logger.Info("GetUsersForAdmin-> Getting UserList Success" + Environment.NewLine + DateTime.Now);
                UserList = _dbContext.User.Include(x => x.AdminUser).Where(x => x.AdminUser.AdminUserId.Equals(id)).ToList();
                logger.Info("GetUsersForAdmin-> Getting UserList Success" + Environment.NewLine + DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error("GetUsersForAdmin->While searching admin Exception Message: " + ex.Message + Environment.NewLine + DateTime.Now);
            }

            if (null == UserList)
            {
                logger.Info("GetUsersForAdmin-> users not present for admin" + Environment.NewLine + DateTime.Now);
                return NotFound();
            }

            logger.Info("GetUsersForAdmin-> Users passed for Admin" + Environment.NewLine + DateTime.Now);
            return Ok(UserList);
        }
        #endregion

        #region Get User Based On Id
        [HttpGet]
        [Route("api/Users/GetUser/{id}")]
        public IHttpActionResult GetUsersForIdPassed(int id)
        {
            User l_cUser = null;
            try
            {
                l_cUser = _dbContext.User.SingleOrDefault(x => x.UserId.Equals(id));
                logger.Info("GetUsersForIdPassed-> Get user for Id passed Success" + Environment.NewLine + DateTime.Now);
            }
            catch (Exception ex)
            {
                logger.Error("GetUsersForIdPassed->While searching user based on id passed Exception Message: " + ex.Message + Environment.NewLine + DateTime.Now);
            }

            if (null == l_cUser)
            {
                logger.Info("GetUsersForIdPassed->No user found for ID passed" + Environment.NewLine + DateTime.Now);
                return NotFound();
            }

            logger.Info("GetUsersForIdPassed->User Found and returned" + Environment.NewLine + DateTime.Now);
            return Ok(l_cUser);
        }
        #endregion

        #region Verify Admin Details Imputted On Login
        [HttpGet]
        [Route("api/Users/GetAdmin/{username}/{password}")]
        public IHttpActionResult GetAdmin(string username, string password)
        {
            AdminUser l_cAdminUser = null;
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                l_cAdminUser = _dbContext.AdminUser.SingleOrDefault(x => x.AdminUserName.ToLower().Equals(username.ToLower()));
                if (!System.Web.Helpers.Crypto.VerifyHashedPassword(l_cAdminUser.AdminPassword, password))
                    l_cAdminUser = null;

                logger.Info("GetAdmin->Admin found for pass and username passed" + Environment.NewLine + DateTime.Now);
            }

            if (null == l_cAdminUser)
            {
                logger.Info("GetAdmin->Admin not found for pass and username passed" + Environment.NewLine + DateTime.Now);
                return NotFound();
            }

            logger.Info("GetAdmin->Admin User returned" + Environment.NewLine + DateTime.Now);
            return Ok(l_cAdminUser);
        }
        #endregion

        #region Create New User
        [HttpPost]
        [Route("api/Users/CreateUser")]
        public IHttpActionResult CreateUser(User NewUserEntry)
        {
            if (!ModelState.IsValid)
            {
                logger.Info("CreateUser->New user stste invalid" + Environment.NewLine + DateTime.Now);
                return BadRequest();
            }
            try
            {
                logger.Info("CreateUser->Adding user in DB + hashing password" + Environment.NewLine + DateTime.Now);
                NewUserEntry.Password = System.Web.Helpers.Crypto.HashPassword(NewUserEntry.Password);
                _dbContext.User.Add(NewUserEntry);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("CreateUser->Exception occured while adding new user:  " + ex.Message + Environment.NewLine + DateTime.Now);
            }
            return Created(new Uri(Request.RequestUri + "/" + NewUserEntry.UserId), NewUserEntry);
        }
        #endregion

        #region Update User Based On ID Passed
        [HttpPut]
        [Route("api/Users/UpdateUser/{id}")]
        public IHttpActionResult UpdateUser(int id, User NewUserEntry)
        {
            if (!ModelState.IsValid)
            {
                logger.Info("UpdateUser->Invalid model state" + Environment.NewLine + DateTime.Now);
                return BadRequest();
            }

            User userInDb = null;
            try
            {
                logger.Info("UpdateUser->Searching user in db based on id passed Id: "+id + Environment.NewLine + DateTime.Now);
                userInDb = _dbContext.User.SingleOrDefault(x => x.UserId == id);
            }
            catch (Exception ex)
            {
                logger.Error("CreateUser->Exception occured while searching userin db Success:  " + ex.Message + Environment.NewLine + DateTime.Now);
            }

            if (null == userInDb)
            {
                logger.Info("UpdateUser->User Not FOund in DB" + Environment.NewLine + DateTime.Now);
                return NotFound();
            }

            try
            {
                logger.Info("UpdateUser->Updating user information" + Environment.NewLine + DateTime.Now);
                userInDb.First_Name = NewUserEntry.First_Name;
                userInDb.Last_Name = NewUserEntry.Last_Name;
                userInDb.Password = System.Web.Helpers.Crypto.HashPassword(NewUserEntry.Password);
                userInDb.StatusId = NewUserEntry.StatusId;
                userInDb.Email_Address = NewUserEntry.Email_Address;
                userInDb.AdminUserId = NewUserEntry.AdminUserId;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("CreateUser->Exception occured while updating User in db Message:  " + ex.Message + Environment.NewLine + DateTime.Now);
            }
            return Ok();
        }
        #endregion

        #region Delete User
        [HttpDelete]
        [Route("api/Users/DeleteUser/{id}")]
        public IHttpActionResult DeleteUser(int id)
        {
            User userInDb = null;
            try
            {
                logger.Info("DeleteUser->Deleting User FromDB ID: " + id + Environment.NewLine + DateTime.Now);
                userInDb = _dbContext.User.SingleOrDefault(x => x.UserId == id);
                if (null == userInDb)
                {
                    logger.Info("DeleteUser->User not found in db ID:" + id + Environment.NewLine + DateTime.Now);
                    return NotFound();
                }
                _dbContext.User.Remove(userInDb);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error("DeleteUser->Exception occured while Deleting User Message: " + ex.Message + Environment.NewLine + DateTime.Now);
            }
            return Ok();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            this._dbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
