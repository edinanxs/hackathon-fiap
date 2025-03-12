﻿using Contatos.Entidades;
using Contatos.Repositorios.Filtros;
using Dapper;
using System.Text;
using Medicos.Entidades;
using Medicos.Repositorios;
using Regioes.Entidades;
using Utils;
using Utils.DBContext;
using Medicos.Repositorios.Filtros;

namespace Medicos
{
    public class MedicosRepositorio(DapperContext dapperContext) : RepositorioDapper<Medico>(dapperContext), IMedicosRepositorio
    {
        public async Task<PaginacaoConsulta<Medico>> ListarMedicosPaginadosAsync(MedicosPaginacaoFiltro filtro)
        {
            StringBuilder sql = new(
                @"SELECT
	                    u.id as Id,
	                    u.cpf as Cpf,
	                    u.email as Email,
	                    u.nome as Nome,
	                    u.tipo as TipoUsuario,
                        m.crm as Crm,
	                    u.criado_em as CriadoEm,
	                    e.id as IdEspecialidade,
	                    e.nome as NomeEspecialidade,
	                    e.descricao as DescricaoEspecialidade
                    FROM
	                    techchallenge.Usuarios u
                    INNER JOIN techchallenge.Medicos m 
                    ON m.id = u.id 
                    INNER JOIN techchallenge.Medico_Especialidades me 
                    ON me.medico_id = u.id 
                    INNER JOIN techchallenge.Especialidades e 
                    ON e.id = me.especialidade_id 
                  WHERE 1 = 1");

            if (!filtro.Email.InvalidOrEmpty())
                sql.AppendLine($" AND u.email = '{filtro.Email}' ");

            if (!filtro.Nome.InvalidOrEmpty())
                sql.AppendLine($" AND u.nome like '%{filtro.Nome}%' ");

            string sqlPaginado = GerarQueryPaginacao(sql.ToString(), filtro.Pg, filtro.Qt, filtro.CpOrd, filtro.TpOrd.ToString());

            var registros = new Dictionary<int, Medico>();

            var queryResult = await session.QueryAsync<Medico, Especialidade, Medico>(sqlPaginado, (medico, especialidade) =>
            {
                if (!registros.TryGetValue(medico.Id, out var existingMedico))
                {
                    existingMedico = medico;
                    registros[medico.Id] = existingMedico;
                }
                existingMedico.SetEspecialidade(especialidade);
                return existingMedico;
            }, splitOn: "IdEspecialidade");

            PaginacaoConsulta<Medico> response = new()
            {
                Registros = registros.Values,
                Total = RecuperarTotalLinhas(sql.ToString())
            };

            return response;
        }
    }
}