using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;

namespace HwaTec.Blog.Controllers
{
    [Route("api/[controller]")] //配置路由
    public class UEditorController : Controller
    {
        private UEditorService  _uEditorService;
        public UEditorController(UEditorService uEditorService)
        {
            this._uEditorService = uEditorService;
        }

        public void Do()
        {
            _uEditorService.DoAction(HttpContext);
        }
    }
}