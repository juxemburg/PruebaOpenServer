using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PokeServices.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeServer.ControllerUtils
{
    public static class ControllerUtilExtensions
    {
        /// <summary>
        /// Función genérica para obtener un recurso
        /// </summary>
        /// <typeparam name="T">Tipo de dato del recurso a obtener</typeparam>
        /// <param name="fnGetModel">Función que obtiene el modelo</param>
        /// <returns></returns>
        public static async Task<IActionResult> Get<T>(this Controller controller, Func<Task<T>> fnGetModel)
        {
            try
            {
                return controller.Ok(await fnGetModel());
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
