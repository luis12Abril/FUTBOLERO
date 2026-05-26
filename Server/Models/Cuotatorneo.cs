using System;
using System.Collections.Generic;

namespace FUTBOLERO.Server.Models;

public partial class Cuotatorneo
{
    public int Idcuotatorneo { get; set; }

    public int Idtorneo { get; set; }

    public decimal Montobase { get; set; }

    public string Concepto { get; set; }

    public DateTime Fechalimite { get; set; }

    public bool Activo { get; set; }
}
