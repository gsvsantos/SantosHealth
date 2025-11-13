using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.ModuloPaciente.Commands.Excluir;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloPaciente;

namespace OrganizaMed.Testes.Unidade.ModuloPaciente.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class ExcluirPacienteRequesHandlerTests
{
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IRepositorioPaciente> _repositorioPacienteMock;

    private ExcluirPacienteRequestHandler _handler;

    [TestInitialize]
    public void Inicializar()
    {
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._repositorioPacienteMock = new Mock<IRepositorioPaciente>();

        this._handler = new ExcluirPacienteRequestHandler(
            this._repositorioPacienteMock.Object,
            this._contextoMock.Object
        );
    }

    [TestMethod]
    public async Task Deve_Excluir_Paciente_Com_Sucesso()
    {
        // Arrange
        ExcluirPacienteRequest request = new(Guid.NewGuid());
        Paciente medicoSelecionado = new("João da Silva", "000.000.000-00", "joao@email.com", "(00) 00000-0000")
        {
            Id = request.Id
        };

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(medicoSelecionado);

        this._repositorioPacienteMock
            .Setup(r => r.ExcluirAsync(It.IsAny<Paciente>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<ExcluirPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(r => r.ExcluirAsync(It.IsAny<Paciente>()), Times.Once);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public async Task Nao_Deve_Excluir_Paciente_Se_Nao_Encontrado()
    {
        // Arrange
        ExcluirPacienteRequest request = new(Guid.NewGuid());

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync((Paciente)null);

        // Act
        FluentResults.Result<ExcluirPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(r => r.ExcluirAsync(It.IsAny<Paciente>()), Times.Never);
        this._contextoMock.Verify(c => c.GravarAsync(), Times.Never);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.NotFoundError(request.Id).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Deve_Retornar_Erro_Se_Ocorreu_Excecao_Ao_Excluir()
    {
        // Arrange
        ExcluirPacienteRequest request = new(Guid.NewGuid());

        this._repositorioPacienteMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(new Paciente(
                "João da Silva",
                "000.000.000-00",
                "joao@email.com",
                "(00) 00000-0000")
            { Id = request.Id }
            );

        this._repositorioPacienteMock
            .Setup(r => r.ExcluirAsync(It.IsAny<Paciente>()))
            .ThrowsAsync(new Exception("Erro ao excluir médico"));

        this._contextoMock
            .Setup(c => c.RollbackAsync())
            .Returns(Task.CompletedTask);

        // Act
        FluentResults.Result<ExcluirPacienteResponse> result = await this._handler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioPacienteMock.Verify(r => r.ExcluirAsync(It.IsAny<Paciente>()), Times.Once);
        this._contextoMock.Verify(c => c.RollbackAsync(), Times.Once);

        Assert.IsFalse(result.IsSuccess);

        string mensagemErroEsperada = ErrorResults.InternalServerError(new Exception()).Message;
        Assert.AreEqual(mensagemErroEsperada, result.Errors.First().Message);
    }
}