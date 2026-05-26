using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models;

public partial class Pagoequipo
{
    public int Idpago { get; set; }

    public int Idcuotaequipo { get; set; }

    public int Idtorneo { get; set; }

    public decimal Montopagado { get; set; }

    public DateTime Fechapago { get; set; }

    public string Referencia { get; set; }

    public string Metodopago { get; set; }

    public string Observaciones { get; set; }

    public string Usuarioregistro { get; set; }
}
