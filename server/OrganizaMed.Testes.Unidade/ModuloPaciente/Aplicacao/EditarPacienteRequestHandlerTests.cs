using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Editar;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Testes.Unidade.ModuloPaciente.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class EditarPacienteRequestHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioPaciente> _repositorioPacienteMock;
    private Mock<IValidator<Paciente>> _validador;

    private EditarPacienteRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioPacienteMock = new Mock<IRepositorioPaciente>();
        this._validador = new Mock<IValidator<Paciente>>();

        this._handler = new EditarPacienteRequestHandler(
            this._repositorioPacienteMock.Object,
            this._contextoMock.Object,
            this._validador.Object
        );
    }

    [TestMethod]
    public async Task Deve_Editar_Paciente_Com_Sucesso()
    {
        // Arrange
        EditarPacienteRequest request = new(Guid.NewGuid(), "João da Silva",
            "000.000.000-00",
            "joao@email.com",
            "(00) 90000-0000"
        );

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(new Paciente(
                "Antônio Carlos",
                "100.002.000-00",
                "joao@email.com",
                "(00) 90000-0000")
            { Id = request.Id }
            );

        this._validador
            .Setup(v => v.ValidateAsync(It.IsAny<Paciente>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarTodosAsync())
            .ReturnsAsync([]);

        this._repositorioPacienteMock
            .Setup(r => r.EditarAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<EditarPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(r => r.EditarAsync(It.IsAny<Paciente>()), Times.Once);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Nao_Deve_Editar_Paciente_Se_Nao_Encontrado()
    {
        // Arrange
        EditarPacienteRequest request = new(Guid.NewGuid(), "João da Silva",
            "000.000.000-00",
            "joao@email.com",
            "(00) 90000-0000"
        );

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync((Paciente)null);

        // Act
        FluentResults.Result<EditarPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(r => r.EditarAsync(It.IsAny<Paciente>()), Times.Never);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.NotFoundError(request.Id).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}