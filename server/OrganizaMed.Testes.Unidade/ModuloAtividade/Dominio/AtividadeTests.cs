using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloAtividade.Dominio;

[TestClass]
[TestCategory("Testes de Unidade")]
public class AtividadeTests
{
    [TestMethod]
    public void Deve_Registrar_Consulta_Com_Periodo_Descanso_Valido()
    {
        // Arrange
        Medico medico = new("João da Silva", "00000-SC");

        DateTime dataInicio = DateTime.Today + new TimeSpan(14, 0, 0);
        DateTime dataTermino = DateTime.Today + new TimeSpan(16, 0, 0);

        Consulta consulta = new(dataInicio, dataTermino, medico);

        DateTime dataInicioSegundaConsulta = DateTime.Today + new TimeSpan(16, 15, 0);
        DateTime dataTerminoSegundaConsulta = DateTime.Today + new TimeSpan(18, 15, 0);

        // Act
        Consulta segundaConsulta = new(dataInicioSegundaConsulta, dataTerminoSegundaConsulta, medico);

        // Assert
        FluentValidation.Results.ValidationResult errosValidacao = new ValidadorAtividadeMedica().Validate(segundaConsulta);
        Assert.AreEqual(0, errosValidacao.Errors.Count);
    }

    [TestMethod]
    public void Nao_Deve_Registrar_Consulta_Com_Periodo_Descanso_Invalido()
    {
        // Arrange
        Medico medico = new("João da Silva", "00000-SC");

        DateTime dataInicio = DateTime.Today + new TimeSpan(14, 0, 0);
        DateTime dataTermino = DateTime.Today + new TimeSpan(16, 0, 0);
        Consulta consulta = new(dataInicio, dataTermino, medico);

        DateTime dataInicioSegundaConsulta = DateTime.Today + new TimeSpan(16, 10, 0);
        DateTime dataTerminoSegundaConsulta = DateTime.Today + new TimeSpan(18, 10, 0);

        Consulta segundaConsulta = new(dataInicioSegundaConsulta, dataTerminoSegundaConsulta, medico);

        // Act
        FluentValidation.Results.ValidationResult errosValidacao = new ValidadorAtividadeMedica().Validate(segundaConsulta);

        // Assert
        string mensagenErroEsperada = "O médico 'João da Silva' está em período de descanso obrigatório";

        string mensagemErroRecebida = errosValidacao.Errors.First().ErrorMessage;

        Assert.AreEqual(1, errosValidacao.Errors.Count);
        Assert.AreEqual(mensagenErroEsperada, mensagemErroRecebida);
    }

    [TestMethod]
    public void Nao_Deve_Registrar_Consulta_Com_Mais_De_Um_Medico()
    {
        // Arrange
        Medico medico = new("João da Silva", "00000-SC");
        Medico segundoMedico = new("Julia Santos", "10002-SP");

        DateTime dataInicio = DateTime.Today + new TimeSpan(14, 0, 0);
        DateTime dataTermino = DateTime.Today + new TimeSpan(16, 0, 0);

        Consulta consulta = new(dataInicio, dataTermino, medico);

        consulta.AdicionarMedico(segundoMedico);
        // Act
        FluentValidation.Results.ValidationResult errosValidacao = new ValidadorAtividadeMedica().Validate(consulta);

        // Assert
        string mensagemEsperada = "Consultas só podem ser realizadas por um médico";

        Assert.AreEqual(1, errosValidacao.Errors.Count);
        Assert.AreEqual(mensagemEsperada, errosValidacao.Errors.First().ErrorMessage);
    }

    [TestMethod]
    public void Deve_Registrar_Cirurgia_Com_Periodo_Descanso_Valido()
    {
        // Arrange

        List<Medico> medicos = [
             new Medico("João da Silva", "00000-SC"),
             new Medico("Julia Santos", "10002-SP")
        ];

        DateTime dataInicio = DateTime.Today + new TimeSpan(14, 0, 0);
        DateTime dataTermino = DateTime.Today + new TimeSpan(16, 0, 0);

        Cirurgia cirurgia = new(dataInicio, dataTermino, medicos);

        DateTime dataInicioSegundaCirurgia = DateTime.Today + new TimeSpan(20, 01, 0);
        DateTime dataTerminoSegundaCirurgia = DateTime.Today + new TimeSpan(21, 01, 0);

        // Act
        Cirurgia segundaCirurgia = new(
            dataInicioSegundaCirurgia,
            dataTerminoSegundaCirurgia,
            medicos
        );

        // Assert
        FluentValidation.Results.ValidationResult errosValidacao = new ValidadorAtividadeMedica().Validate(segundaCirurgia);
        Assert.AreEqual(0, errosValidacao.Errors.Count);
    }

    [TestMethod]
    public void Nao_Deve_Registrar_Cirurgia_Com_Periodo_Descanso_Invalido()
    {
        // Arrange

        List<Medico> medicos = [
             new Medico("João da Silva", "00000-SC"),
             new Medico("Júlia Santos", "10002-SP")
        ];

        DateTime dataInicio = DateTime.Today + new TimeSpan(14, 0, 0);
        DateTime dataTermino = DateTime.Today + new TimeSpan(16, 0, 0);

        Cirurgia cirurgia = new(dataInicio, dataTermino, medicos);

        DateTime dataInicioSegundaCirurgia = DateTime.Today + new TimeSpan(20, 00, 0);
        DateTime dataTerminoSegundaCirurgia = DateTime.Today + new TimeSpan(21, 01, 0);

        // Act
        Cirurgia segundaCirurgia = new(
            dataInicioSegundaCirurgia,
            dataTerminoSegundaCirurgia,
            medicos
        );

        // Assert
        FluentValidation.Results.ValidationResult errosValidacao = new ValidadorAtividadeMedica().Validate(segundaCirurgia);

        List<string> mensagensErroEsperadas =
        [
            "O médico 'João da Silva' está em período de descanso obrigatório",
            "O médico 'Júlia Santos' está em período de descanso obrigatório"
        ];

        List<string> mensagensErroRecebidas = errosValidacao.Errors
            .Select(e => e.ErrorMessage).ToList();

        Assert.AreEqual(2, errosValidacao.Errors.Count);
        CollectionAssert.AreEqual(mensagensErroEsperadas, mensagensErroRecebidas);
    }
}