using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokeServices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeApp.ControllerUtils
{
    public static class ControllerUtilExtensions
    {
        public static async Task<IActionResult> Post<U>(this Controller controller,
            ModelStateDictionary modelState, Func<Task<U>> fnInsert)
        {
            try
            {
                if (modelState.IsValid)
                {
                    var result = await fnInsert();
                    return controller.Ok(result);
                }
                else
                {
                    return controller.BadRequest(modelState);
                }
            }
            catch (OperationFailedException ex) when (ex.Status == OperationErrorStatus.MalformedInput)
            {
                return controller.BadRequest(ex.Message);
            }
            catch (OperationFailedException ex) when (ex.Status == OperationErrorStatus.ResourceNotFound)
            {
                return controller.NotFound(ex.Message);
            }
            catch (Exception err)
            {
                //Log error.
                return controller.StatusCode(500, err);
            }
        }
    }
}
