﻿using Utils;
using Usuarios.Request;
using Usuarios.Response;

namespace Usuarios.Interfaces
{
    public interface IUsuariosAppServico
    {
        PaginacaoConsulta<UsuarioResponse> ListarPacientes(PacienteListarRequest request);
    }
}
