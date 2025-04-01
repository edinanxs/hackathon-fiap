﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Azure;
using FluentAssertions;
using Hackathon.Fiap.DataTransfer.Consultas.Enumeradores;
using Hackathon.Fiap.DataTransfer.Consultas.Requests;
using Hackathon.Fiap.DataTransfer.Consultas.Responses;
using Hackathon.Fiap.DataTransfer.Utils;
using Hackathon.Fiap.Domain.Usuarios.Entidades;
using Hackathon.Fiap.Teste.Integracao.ClassesHelper;
using MySqlX.XDevAPI;
using Newtonsoft.Json;

namespace Hackathon.Fiap.Teste.Integracao
{
    public class ConsultasIntegracaoTestes(HackatonApiFactory hackatonApi) : IClassFixture<HackatonApiFactory>
    {
        private readonly HttpClient apiFactoryClient = hackatonApi.CreateClient();
        private string tokenJwt = string.Empty;

        private async Task AutenticarAplicacao(string role)
        {
            string usuario = $"SIS@{role.ToUpper()}.com";
            string senha = "123";
            HttpResponseMessage result = await apiFactoryClient.PostAsync($"auth?identificador={usuario}&senha={senha}", null);
            Assert.True(result.IsSuccessStatusCode, "Autenticação falhou.");
            tokenJwt = await result.Content.ReadAsStringAsync();
            apiFactoryClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenJwt);
        }

        [Fact]
        public async Task Listar_Consultas_Paginas_Corretamente()
        {
            await AutenticarAplicacao(Roles.Medico);
            HttpResponseMessage result = await apiFactoryClient.GetAsync("api/consultas/paginados");
            PaginacaoConsulta <ConsultaResponse>? consultasPaginadas = JsonConvert.DeserializeObject<PaginacaoConsulta<ConsultaResponse>> (await result.Content.ReadAsStringAsync());
           
            Assert.NotNull(consultasPaginadas);
            Assert.NotEmpty(consultasPaginadas.Registros);
        }

        [Fact]
        public async Task Listar_Consultas_Autorizacao_ErroSemAutorizacao()
        {
            await AutenticarAplicacao(Roles.Administrador);

            HttpResponseMessage result = await apiFactoryClient.GetAsync("api/consultas/paginados");

            Assert.Equal(System.Net.HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task AlterarSituacaoConsulta_Autorizacao_SemAutorizacao()
        {
            await AutenticarAplicacao(Roles.Administrador);
            HttpResponseMessage result = await apiFactoryClient.PutAsync("api/consultas/situacoes", null);
            Assert.Equal(System.Net.HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task AlterarSituacaoConsulta_Status_Invalido_Espero_Erro()
        {
            await AutenticarAplicacao(Roles.Medico);

            HttpResponseMessage resultConsulta = await apiFactoryClient.GetAsync("api/consultas/paginados");
            PaginacaoConsulta<ConsultaResponse>? consultasPaginadas = JsonConvert.DeserializeObject<PaginacaoConsulta<ConsultaResponse>>(await resultConsulta.Content.ReadAsStringAsync());
            
            Assert.NotNull(consultasPaginadas);
            Assert.NotEmpty(consultasPaginadas.Registros);

            ConsultaResponse consultaResponse = consultasPaginadas.Registros.First();
            ConsultaStatusRequest consultaStatusRequest = new()
            {
                IdConsulta = consultaResponse.IdConsulta,
                Justificativa = "Justificativa",
                Status = StatusConsultaEnum.Aceita
            };

            string jsonContent = JsonConvert.SerializeObject(consultaStatusRequest);
            HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            HttpResponseMessage resultAlteracao = 
                await apiFactoryClient.PutAsync($"api/consultas/situacoes?IdConsulta={consultaResponse.IdConsulta}&Status=Aceita", httpContent);

            ErroResponse? consultaAlterada = JsonConvert.DeserializeObject<ErroResponse>(await resultAlteracao.Content.ReadAsStringAsync());

            Assert.NotNull(consultaAlterada);
            Assert.Equal(Convert.ToInt32(HttpStatusCode.BadRequest), consultaAlterada.Erro.StatusCode);
            Assert.Equal("A consulta está cancelada.", consultaAlterada.Erro.Mensagem);
            Assert.Equal("RegraDeNegocioExcecao", consultaAlterada.Erro.Tipo);
        }

    }
}
