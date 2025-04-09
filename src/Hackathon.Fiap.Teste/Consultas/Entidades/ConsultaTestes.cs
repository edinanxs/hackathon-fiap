using FluentAssertions;
using Hackathon.Fiap.DataTransfer.Consultas.Enumeradores;
using Hackathon.Fiap.Domain.Consultas.Entidades;
using Hackathon.Fiap.Domain.HorariosDisponiveis.Entidades;
using Hackathon.Fiap.Domain.Medicos.Entidades;
using Hackathon.Fiap.Domain.Pacientes.Entidades;

namespace Hackathon.Fiap.Teste.Consultas.Entidades;

public class ConsultaTestes
{
    [Fact]
    public void Quando_CriarConsulta_ComMedicoEPaciente_DeveInicializarPropriedadesCorretamente()
    {
        // ARRANGE
        int id = 1;
        double valor = 150.00;
        StatusConsultaEnum status = StatusConsultaEnum.Pendente;
        string justificativaCancelamento = "";
        DateTime criadoEm = DateTime.Now;
        
        // Criando a especialidade
        var especialidade = new Especialidade { EspecialidadeId = 1, NomeEspecialidade = "Cardiologia", DescricaoEspecialidade = "Especialidade de coração" };
        
        // Criando o médico com a especialidade e CRM
        var medico = new Medico();
        medico.SetCrm("123456");
        medico.SetEspecialidade(especialidade);

        // Criando o paciente
        var paciente = new Paciente();

        // Criando o horario disponivel
        HorarioDisponivel horarioDisponivel = new ();

        // ACT
        Consulta consulta = new(id, valor, status, medico, horarioDisponivel, paciente, justificativaCancelamento);
        consulta.SetMedico(medico);
        consulta.SetPaciente(paciente);
        consulta.SetHorarioDisponivel(horarioDisponivel);

        // ASSERT
        consulta.Medico.Should().Be(medico);
        consulta.Medico.Crm.Should().Be("123456");
        consulta.Medico.Especialidade.Should().Be(especialidade);
        consulta.Paciente.Should().Be(paciente);
        consulta.Valor.Should().Be(valor);
        consulta.Status.Should().Be(status);
        consulta.JustificativaCancelamento.Should().Be(justificativaCancelamento);
        consulta.CriadoEm.Should().BeCloseTo(criadoEm , TimeSpan.FromSeconds(5000));
        consulta.HorarioDisponivel.Should().Be(horarioDisponivel);
    }

    [Fact]
    public void Quando_CriarConsulta_ComStatusCancelada_DeveInicializarComJustificativa()
    {
        // ARRANGE
        int id = 1;
        double valor = 150.00;
        StatusConsultaEnum status = StatusConsultaEnum.Cancelada;
        string justificativaCancelamento = "Paciente solicitou cancelamento";

        // Criando a especialidade
        var especialidade = new Especialidade { EspecialidadeId = 1, NomeEspecialidade = "Cardiologia", DescricaoEspecialidade = "Especialidade de coração" };

        // Criando o médico com a especialidade e CRM
        var medico = new Medico();
        medico.SetCrm("123456");
        medico.SetEspecialidade(especialidade);

        // Criando o paciente
        var paciente = new Paciente();

        // Criando o horario disponivel
        HorarioDisponivel horarioDisponivel = new();

        // ACT
        Consulta consulta = new(id, valor, status, medico, horarioDisponivel, paciente, justificativaCancelamento);

        // ASSERT
        consulta.Status.Should().Be(StatusConsultaEnum.Cancelada);
        consulta.JustificativaCancelamento.Should().Be(justificativaCancelamento);
    }

    [Fact]
    public void Quando_SetMedicoEPaciente_DeveAtualizarPropriedadesCorretamente()
    {
        // ARRANGE
        var consulta = new Consulta();
        var medico = new Medico();
        var paciente = new Paciente();

        // ACT
        consulta.SetMedico(medico);
        consulta.SetPaciente(paciente);

        // ASSERT
        consulta.Medico.Should().Be(medico);
        consulta.Paciente.Should().Be(paciente);
    }
} 