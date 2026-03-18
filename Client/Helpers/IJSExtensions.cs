using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FUTBOLERO.Client.Helpers
{
    public static class IJSExtensions
    {
        //public static async Task<object> MostrarMensaje(this IJSRuntime js, string mensaje)
        //{
        //    return  await js.InvokeAsync<object>("Swal.fire", mensaje).ConfigureAwait(false);
        //}

        //public static Task MostrarMensaje(this IJSRuntime js, string mensaje)
        //{
        //    return js.InvokeAsync<object>("Swal.fire", mensaje);
        //}

    }

    public enum TipoMensajeSweetAlert
    {
        question, warning, error, success, info
    }

}
