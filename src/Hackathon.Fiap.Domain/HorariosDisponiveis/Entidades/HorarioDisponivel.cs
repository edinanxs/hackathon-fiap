using Hackathon.Fiap.DataTransfer.HorariosDisponiveis.Enumeradores;
using Hackathon.Fiap.Domain.Medicos.Entidades;
using Hackathon.Fiap.Domain.Pacientes.Entidades;

namespace Hackathon.Fiap.Domain.HorariosDisponiveis.Entidades
{
    public class HorarioDisponivel
    {
        public int IdHorarioDisponivel {  get; set; }
        public Medico Medico { get; set; } = new Medico();
        public Paciente? Paciente { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraFim { get; set; }
        public StatusHorarioDisponivelEnum Status { get; set; }

        public HorarioDisponivel()
        {
            
        }

        public HorarioDisponivel(int idHorarioDisponivel, DateTime dataHoraInicio, DateTime dataHoraFim, StatusHorarioDisponivelEnum status)
        {
            IdHorarioDisponivel = idHorarioDisponivel;
            DataHoraInicio = dataHoraInicio;
            DataHoraFim = dataHoraFim;
            Status = status;
        }

        public void SetMedico(Medico medico)
        {
            Medico = medico;   
        }

        public void SetPaciente(Paciente? paciente)
        {
            Paciente = paciente;
        }
    }
}