using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPIT.RentHouse.Common
{
    public enum ResultState
    {
        Error = -1,
        Success = 1
    }

    public class AjaxResult
    {
        public ResultState State { get; set; }
        public string ErrorMessage { get; set; }

        public object Data { get; set; }

        public AjaxResult(ResultState state, string errorMessage)
        {
            this.State = state;
            this.ErrorMessage = errorMessage;
        }
        public AjaxResult(ResultState state, string errorMessage,object data)
        {
            this.State = state;
            this.ErrorMessage = errorMessage;
            this.Data = data;
        }
    }
}
