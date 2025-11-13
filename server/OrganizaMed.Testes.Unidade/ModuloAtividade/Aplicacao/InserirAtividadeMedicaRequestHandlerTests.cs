using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.ModuloAtividade;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Inserir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloAutenticacao;
using OrganizaMed.Dominio.ModuloMedico;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Testes.Unidade.ModuloAtividade.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class InserirAtividadeMedicaRequestHandlerTests
{
    private Mock<IRepositorioAtividadeMedica> _repositorioAtividadeMedicaMock;
    private Mock<IRepositorioMedico> _repositorioMedicoMock;
    private Mock<IRepositorioPaciente> _repositorioPacienteMock;
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IValidator<AtividadeMedica>> _validadorMock;
    private Mock<ITenantProvider> _tenantProviderMock;
    private Mock<EnviarEmail> _enviarEmailMock;

    private InserirAtividadeMedicaRequestHandler _requestHandler;

    [TestInitialize]
    public void Inicializar()
    {
        this._repositorioAtividadeMedicaMock = new Mock<IRepositorioAtividadeMedica>();
        this._repositorioMedicoMock = new Mock<IRepositorioMedico>();
        this._repositorioPacienteMock = new Mock<IRepositorioPaciente>();
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._tenantProviderMock = new Mock<ITenantProvider>();
        this._validadorMock = new Mock<IValidator<AtividadeMedica>>();
        this._enviarEmailMock = new Mock<EnviarEmail>();

        this._requestHandler = new InserirAtividadeMedicaRequestHandler(
            this._repositorioAtividadeMedicaMock.Object,
            this._repositorioMedicoMock.Object,
            this._repositorioPacienteMock.Object,
            this._contextoMock.Object,
            this._tenantProviderMock.Object,
            this._validadorMock.Object,
            this._enviarEmailMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Inserir_Consulta_Com_Sucesso()
    {
        // Arrange
        Paciente paciente = new("João da Silva", "000.000.000-01", "joao@gmail.com", "(00) 00000-0000");

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(paciente.Id))
            .ReturnsAsync(paciente);

        InserirAtividadeMedicaRequest request = new(
            paciente.Id,
            DateTime.Now,
            DateTime.Now.AddHours(1),
            TipoAtividadeMedica.Consulta,
            [Guid.NewGuid()]
        );

        Medico medico = new("Dr. João", "12345-SP");

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarMuitosPorId(request.Medicos))
            .ReturnsAsync([medico]);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.InserirAsync(It.IsAny<AtividadeMedica>()))
            .ReturnsAsync(Guid.NewGuid());

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<InserirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(x => x.InserirAsync(It.IsAny<AtividadeMedica>()), Times.Once);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
    }

    [TestMethod]
    public async Task Nao_Deve_Inserir_Quando_Medicos_Nao_Forem_Encontrados()
    {
        // Arrange
        Paciente paciente = new("João da Silva", "000.000.000-01", "joao@gmail.com", "(00) 00000-0000");

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(paciente.Id))
            .ReturnsAsync(paciente);

        InserirAtividadeMedicaRequest request = new(
            paciente.Id,
            DateTime.Now,
            DateTime.Now.AddHours(1),
            TipoAtividadeMedica.Consulta,
            [Guid.NewGuid()]
        );

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarMuitosPorId(request.Medicos))
            .ReturnsAsync([]);

        // Act
        FluentResults.Result<InserirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(x => x.InserirAsync(It.IsAny<AtividadeMedica>()), Times.Never);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Never);

        Assert.IsTrue(result.IsFailed);

        string mensagemErroEsperada = AtividadeMedicaErrorResults.MedicosNaoEncontradosError().Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Nao_Deve_Inserir_Atividade_Com_Erros_De_Validacao()
    {
        // Arrange
        Paciente paciente = new("João da Silva", "000.000.000-01", "joao@gmail.com", "(00) 00000-0000");

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(paciente.Id))
            .ReturnsAsync(paciente);

        InserirAtividadeMedicaRequest request = new(
            paciente.Id,
            DateTime.Now.AddDays(-2),
            DateTime.Now.AddDays(-3),
            TipoAtividadeMedica.Consulta,
            [Guid.NewGuid()]
        );

        Medico medico = new("Dr. João", "12345-SP");

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarMuitosPorId(request.Medicos))
            .ReturnsAsync([medico]);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure("Inicio", "A data de início deve ser no presente ou no futuro"),
                new ValidationFailure("Termino", "A data de término deve ser posterior à data de início")
            ]));

        // Act
        FluentResults.Result<InserirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(x => x.InserirAsync(It.IsAny<AtividadeMedica>()), Times.Never);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Never);

        Assert.IsTrue(result.IsFailed);

        string mensagemErroEsperada = ErrorResults.BadRequestError(
            result.Errors.Select(e => e.Message).ToList()
        ).Message;

        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Deve_Retornar_Erro_Interno_Em_Caso_De_Exception()
    {
        // Arrange
        Paciente paciente = new("João da Silva", "000.000.000-01", "joao@gmail.com", "(00) 00000-0000");

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(paciente.Id))
            .ReturnsAsync(paciente);

        InserirAtividadeMedicaRequest request = new(
            paciente.Id,
            DateTime.Now,
            DateTime.Now.AddHours(2),
            TipoAtividadeMedica.Cirurgia,
            [Guid.NewGuid()]
        );

        Medico medico = new("Dr. João", "12345-SP");
        this._repositorioMedicoMock
            .Setup(r => r.SelecionarMuitosPorId(request.Medicos))
            .ReturnsAsync([medico]);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.InserirAsync(It.IsAny<AtividadeMedica>()))
            .Throws(new Exception("Erro de banco de dados"));

        // Act
        FluentResults.Result<InserirAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._contextoMock.Verify(x => x.RollbackAsync(), Times.Once);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.InternalServerError(new Exception()).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}