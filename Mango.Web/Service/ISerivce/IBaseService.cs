using System;
using Mango.Web.Models;
using Mango.Web.Models;

namespace Mango.Web.Service.ISerivce
{
	public interface IBaseService
	{
		Task<ResponseDto?>  SendAsync(RequestDto requestDto);
	}
}

