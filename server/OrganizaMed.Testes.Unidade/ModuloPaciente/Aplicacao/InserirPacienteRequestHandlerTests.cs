using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Inserir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAutenticacao;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Testes.Unidade.ModuloPaciente.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class InserirPacienteRequestHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioPaciente> _repositorioPacienteMock;
    private Mock<IValidator<Paciente>> _validadorMock;
    private Mock<ITenantProvider> _tenantProviderMock;

    private InserirPacienteRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioPacienteMock = new Mock<IRepositorioPaciente>();
        this._validadorMock = new Mock<IValidator<Paciente>>();
        this._tenantProviderMock = new Mock<ITenantProvider>();

        this._handler = new InserirPacienteRequestHandler(
            this._contextoMock.Object,
            this._repositorioPacienteMock.Object,
            this._tenantProviderMock.Object,
            this._validadorMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Inserir_Paciente()
    {
        // Arrange
        InserirPacienteRequest request = new(
            "João da Silva",
            "000.000.000-00",
            "joao@email.com",
            "(00) 90000-0000"
        );

        this._validadorMock.Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarTodosAsync())
            .ReturnsAsync([]);

        this._repositorioPacienteMock
            .Setup(r => r.InserirAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(Guid.NewGuid());

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<InserirPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(x => x.InserirAsync(It.IsAny<Paciente>()), Times.Once);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }
}