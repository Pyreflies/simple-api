using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Cafe.Controllers.Params;
using Cafe.Controllers.DBHandlers;
using Microsoft.AspNetCore.Authorization;
using Cafe.Models.ReadModels;
using System.Linq;

namespace Cafe.Controllers
{
    
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> gets()
        {
            string sql = string.Format(@"SELECT * FROM TB_USERS");
            try
            {
                var json = SqlDBContext.SelectQuery<UserParam>(sql).ToList();
                return Ok(new StatusResult<UserParam>( "isSuccess", 200,true,json));
            }
            catch (Exception ex)
            {
                return Ok(new StatusResult("unSuccessful", 500, false));

            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getById(int id)
        {
            string sql = string.Format(@"SELECT * FROM TB_USERS WHERE ID = '{0}'", id);
            try
            {
                var json = SqlDBContext.SelectQuery<UserParam>(sql).ToList();
                return Ok(new StatusResult<UserParam>("isSuccess", 200, true, json));
            }
            catch (Exception ex)
            {
                return Ok(new StatusResult("unSuccessful", 500, false));

            }
        }

        [HttpPost]
        public async Task<IActionResult> post(UserParam user)
        {
            string insert = string.Format(@"INSERT INTO TB_USERS
                (NAME,LASTNAME,EMAIL) 
                values('{0}','{1}','{2}')", user.NAME, user.LASTNAME, user.EMAIL);
            try
            {
                SqlDBContext.ExecuteNonQuery(insert);
                return Ok(new StatusResult("บันทึกสำเร็จ", 200, true));

            }
            catch (Exception ex)
            {
                return Ok(new StatusResult("เกิดข้อผิดพลาด : " + ex, 500, false));

            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> put(UserParam user)
        {
            string update = string.Format(@"UPDATE TB_USERS SET
                NAME = '{0}' ,
                LASTNAME = '{2}'
                EMAIL = '{3}' 
                where ID = {4}", user.NAME, user.LASTNAME, user.EMAIL,user.ID);
            try
            {
                SqlDBContext.ExecuteNonQuery(update);
                return Ok(new StatusResult("อัพเดตสำเร็จ", 200, true));
            }
            catch (Exception ex)
            {
                return Ok(new StatusResult("เกิดข้อผิดพลาด : " + ex, 500, false));
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(UserParam user)
        {
            string delete = string.Format(@"DELETE FROM TB_USERS SET
                where ID = {0}",user.ID);
            try
            {
                SqlDBContext.ExecuteNonQuery(delete);
                return Ok(new StatusResult("ลบสำเร็จ", 200, true));
            }
            catch (Exception ex)
            {
                return Ok(new StatusResult("เกิดข้อผิดพลาด : " + ex, 500, false));
            }
        }
    }
}
