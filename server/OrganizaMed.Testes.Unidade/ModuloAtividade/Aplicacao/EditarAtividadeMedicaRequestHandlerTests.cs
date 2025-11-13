using FluentValidation;
using FluentValidation.Results;
using Moq;
using OrganizaMed.Aplicacao.Compartilhado;
using OrganizaMed.Aplicacao.EmailSender.Commands;
using OrganizaMed.Aplicacao.ModuloAtividade.Commands.Editar;
using OrganizaMed.Dominio.Compartilhado;
using OrganizaMed.Dominio.ModuloAtividade;
using OrganizaMed.Dominio.ModuloMedico;

namespace OrganizaMed.Testes.Unidade.ModuloAtividade.Aplicacao;

[TestClass]
[TestCategory("Testes de Unidade")]
public class EditarAtividadeMedicaRequestHandlerTests
{
    private Mock<IRepositorioAtividadeMedica> _repositorioAtividadeMedicaMock;
    private Mock<IRepositorioMedico> _repositorioMedicoMock;
    private Mock<IContextoPersistencia> _contextoMock;
    private Mock<IValidator<AtividadeMedica>> _validadorMock;
    private Mock<EnviarEmail> _enviarEmailMock;

    private EditarAtividadeMedicaRequestHandler _requestHandler;

    [TestInitialize]
    public void Inicializar()
    {
        this._repositorioAtividadeMedicaMock = new Mock<IRepositorioAtividadeMedica>();
        this._repositorioMedicoMock = new Mock<IRepositorioMedico>();
        this._contextoMock = new Mock<IContextoPersistencia>();
        this._validadorMock = new Mock<IValidator<AtividadeMedica>>();
        this._enviarEmailMock = new Mock<EnviarEmail>();

        this._requestHandler = new EditarAtividadeMedicaRequestHandler(
            this._repositorioAtividadeMedicaMock.Object,
            this._repositorioMedicoMock.Object,
            this._contextoMock.Object,
            this._validadorMock.Object,
            this._enviarEmailMock.Object

        );
    }

    [TestMethod]
    public async Task Deve_Editar_Atividade_Com_Sucesso()
    {
        // Arrange
        EditarAtividadeMedicaRequest request = new(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddHours(1),
            [Guid.NewGuid()]
        );

        Consulta atividade = new(DateTime.Now, DateTime.Now.AddHours(2), new Medico("Dr. João", "12345-SP"));
        Medico medicoAdicionado = new("Dr. Maria", "67890-SP");
        Medico medicoRemovido = atividade.Medicos.First();

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(atividade);

        this._repositorioMedicoMock
            .Setup(r => r.SelecionarMuitosPorId(request.Medicos))
            .ReturnsAsync([medicoAdicionado]);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.EditarAsync(It.IsAny<AtividadeMedica>()))
            .ReturnsAsync(true);

        this._contextoMock
            .Setup(c => c.GravarAsync())
            .ReturnsAsync(1);

        // Act
        FluentResults.Result<EditarAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._repositorioAtividadeMedicaMock.Verify(x => x.EditarAsync(It.IsAny<AtividadeMedica>()), Times.Once);
        this._contextoMock.Verify(x => x.GravarAsync(), Times.Once);

        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(atividade.Id, result.Value.Id);
    }

    [TestMethod]
    public async Task Nao_Deve_Editar_Atividade_Nao_Encontrada()
    {
        // Arrange
        EditarAtividadeMedicaRequest request = new(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddHours(1),
            null
        );

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync((AtividadeMedica)null);

        // Act
        FluentResults.Result<EditarAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(ErrorResults.NotFoundError(request.Id).Message, result.Errors.First().Message);
    }

    [TestMethod]
    public async Task Nao_Deve_Editar_Atividade_Com_Erros_De_Validacao()
    {
        // Arrange
        EditarAtividadeMedicaRequest request = new(
            Guid.NewGuid(),
            DateTime.Now.AddDays(-1),
            DateTime.Now.AddDays(-2),
            null
        );

        Consulta atividade = new(DateTime.Now, DateTime.Now.AddHours(2), new Medico("Dr. João", "12345-SP"));

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(atividade);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(
            [
                new ValidationFailure("Termino", "A data de término deve ser posterior à data de início")
            ]));

        // Act
        FluentResults.Result<EditarAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual(1, result.Errors.Count);
    }

    [TestMethod]
    public async Task Deve_Retornar_Erro_Interno_Em_Caso_De_Exception()
    {
        // Arrange
        EditarAtividadeMedicaRequest request = new(
            Guid.NewGuid(),
            DateTime.Now,
            DateTime.Now.AddHours(2),
            null
        );

        Consulta atividade = new(DateTime.Now, DateTime.Now.AddHours(2), new Medico("Dr. João", "12345-SP"));

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.SelecionarPorIdAsync(request.Id))
            .ReturnsAsync(atividade);

        this._validadorMock
            .Setup(v => v.ValidateAsync(It.IsAny<AtividadeMedica>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        this._repositorioAtividadeMedicaMock
            .Setup(r => r.EditarAsync(It.IsAny<AtividadeMedica>()))
            .Throws(new Exception("Erro de banco de dados"));

        // Act
        FluentResults.Result<EditarAtividadeMedicaResponse> result = await this._requestHandler.Handle(request, It.IsAny<CancellationToken>());

        // Assert
        this._contextoMock.Verify(x => x.RollbackAsync(), Times.Once);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(ErrorResults.InternalServerError(new Exception()).Message, result.Errors.First().Message);
    }
}
