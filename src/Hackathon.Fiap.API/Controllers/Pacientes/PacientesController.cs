﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Fiap.DataTransfer.Utils;
using Hackathon.Fiap.DataTransfer.Pacientes.Responses;
using Hackathon.Fiap.DataTransfer.Usuarios.Request;
using Hackathon.Fiap.Application.Pacientes.Interfaces;
using Hackathon.Fiap.Domain.Usuarios.Entidades;

namespace Hackathon.Fiap.API.Controllers.Pacientes
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController(IPacientesAppServico pacientesAppServico) : ControllerBase
    {
        /// <summary>
        /// Recuperar pacientes com paginação.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("paginados")]
        [Authorize(Roles = Roles.Medico)]
        public async Task<ActionResult<PaginacaoConsulta<PacienteResponse>>> RecuperarPacientesAsync([FromQuery] UsuarioListarRequest request, CancellationToken ct)
        {
            PaginacaoConsulta<PacienteResponse> usuarios = await pacientesAppServico.ListarPacientesAsync(request, ct);
            return Ok(usuarios);
        }
    }
}