using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Cafe.Controllers.Params;
using Cafe.Controllers.DBHandlers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Cafe.Models.ReadModels;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Cafe.Controllers
{
    
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        [HttpGet]
        public async Task<IActionResult> gets()
        {
            string sql = string.Format(@"SELECT * FROM TB_USERS");
            try
            {
                var json = SqlDBContext.SelectQuery(sql);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return Ok("เกิดข้อผิดพลาด");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(int id)
        {
            string sql = string.Format(@"SELECT * FROM TB_USERS WHERE ID = '{0}'", id);
            try
            {
                var json = SqlDBContext.SelectQuery(sql);
                return Ok(json);
            }
            catch (Exception ex)
            {
                return Ok("เกิดข้อผิดพลาด");
            }
        }

        [HttpPost]
        public async Task<StatusResult> post(UserParam user)
        {
            string insert = string.Format(@"INSERT INTO TB_USERS
                (NAME,LASTNAME,EMAIL) 
                values('{0}','{1}','{2}')", user.NAME, user.LASTNAME, user.EMAIL);
            try
            {
                SqlDBContext.ExecuteNonQuery(insert);
                return StatusResult.Ok();
            }
            catch (Exception ex)
            {
                return StatusResult.Error("เกิดข้อผิดพลาด");
            }
        }

        [HttpPut("{id}")]
        public async Task<StatusResult> put(UserParam user)
        {
            string update = string.Format(@"UPDATE TB_USERS SET
                NAME = '{0}' ,
                LASTNAME = '{2}'
                EMAIL = '{3}' 
                where ID = {4}", user.NAME, user.LASTNAME, user.EMAIL,user.ID);
            try
            {
                SqlDBContext.ExecuteNonQuery(update);
                return StatusResult.Ok();
            }
            catch (Exception ex)
            {
                return StatusResult.Error("เกิดข้อผิดพลาด");
            }
        }
        [HttpDelete("{id}")]
        public async Task<StatusResult> delete(UserParam user)
        {
            string delete = string.Format(@"DELETE FROM TB_USERS SET
                where ID = {0}",user.ID);
            try
            {
                SqlDBContext.ExecuteNonQuery(delete);
                return StatusResult.Ok();
            }
            catch (Exception ex)
            {
                return StatusResult.Error("เกิดข้อผิดพลาด");
            }
        }
    }
}
