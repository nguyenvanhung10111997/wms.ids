using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using wms.infrastructure.Enums;
using wms.infrastructure.Models;

namespace wms.infrastructure.Configurations
{
    [ApiController]
    [AllowAnonymous]
    public abstract class BaseController : ControllerBase
    {
        private UserPrincipal _currentUser;

        protected UserPrincipal CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = (UserPrincipal)HttpContext.User;
                return _currentUser;
            }
        }

        protected IActionResult ApiOK<T>(CRUDResult<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)//0 = 200            
                return Ok(obj.Data);
            else if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)//7 = 201
                return Created(string.Empty, obj.Data);
            else if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)//1 = 204            
                return NoContent();
            else if (obj.StatusCode == CRUDStatusCodeRes.InvalidData
                || obj.StatusCode == CRUDStatusCodeRes.ResetContent)//6 = 406 || 8 = 205
                return StatusCode((int)HttpStatusCode.NotAcceptable, obj.ErrorMessage);
            else//500            
                return StatusCode((int)HttpStatusCode.InternalServerError, obj.ErrorMessage);
        }

        protected IActionResult ApiOK<T>(PagingResponse<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)//0 = 200            
                return Ok(obj);
            else if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)//7 = 201
                return Created(string.Empty, obj);
            else if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)//1 = 204            
                return NoContent();
            else if (obj.StatusCode == CRUDStatusCodeRes.InvalidData
                || obj.StatusCode == CRUDStatusCodeRes.ResetContent)//6 = 406 || 8 = 205
                return StatusCode((int)HttpStatusCode.NotAcceptable, obj.ErrorMessage);
            else//500            
                return StatusCode((int)HttpStatusCode.InternalServerError, obj.ErrorMessage);
        }

        protected IActionResult CreateResponse<T>(CRUDResult<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)//0 = 200            
                return Ok(obj.Data);
            else if (obj.StatusCode == CRUDStatusCodeRes.ReturnWithData)//7 = 201
                return Created(string.Empty, obj.Data);
            else if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)//1 = 204            
                return NoContent();
            else if (obj.StatusCode == CRUDStatusCodeRes.InvalidData
                || obj.StatusCode == CRUDStatusCodeRes.ResetContent)//6 = 406 || 8 = 205
                return StatusCode((int)HttpStatusCode.NotAcceptable, obj.ErrorMessage);
            else//500            
                return StatusCode((int)HttpStatusCode.InternalServerError, obj.ErrorMessage);
        }

        [NonAction]
        protected IActionResult CreateResponse<T>(PagingResponse<T> obj)
        {
            if (obj.StatusCode == CRUDStatusCodeRes.Success)//0 = 200            
                return Ok(obj);
            else if (obj.StatusCode == CRUDStatusCodeRes.ResourceNotFound || obj.StatusCode == CRUDStatusCodeRes.Deny)//1 = 204            
                return NoContent();
            else if (obj.StatusCode == CRUDStatusCodeRes.InvalidData || obj.StatusCode == CRUDStatusCodeRes.ResetContent)//6 = 406 || 8 = 205
                return StatusCode((int)HttpStatusCode.NotAcceptable, obj.ErrorMessage);
            else//500            
                return StatusCode((int)HttpStatusCode.InternalServerError, obj.ErrorMessage);
        }

    }
}
