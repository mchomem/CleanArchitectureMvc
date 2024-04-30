namespace CleanArchMvc.Domain.Tests;

public class ProductUnitTest
{
    [Fact(DisplayName = "Create product with valid parameters")]
    public void CreateProduct_WithValidParameters_ResultObjectValidState()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.9m, 99, "Product Image");

        action
            .Should()
            .NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create product with negative id value")]
    public void CreateProduct_WithNegativeIdValue_ResultDomainExceptionValidate()
    {
        Action action = () => new Product(-1, "Product Name", "Product Description", 9.9m, 99, "Product Image");

        action
            .Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid Id value.");
    }

    [Fact(DisplayName = "Create product with short name value")]
    public void CreateProduct_WithShortNameValue_ResultDomainExceptionValidate()
    {
        Action action = () => new Product(1, "Pr", "Product Description", 9.9m, 99, "Product Image");

        action
            .Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid name, too short, minimum 3 characters");
    }

    [Fact(DisplayName = "Create product with long image name value")]
    public void CreateProduct_WithLongImageNameValue_ResultDomainExceptionValidate()
    {
        Action action = () => new Product(1, "Product Description", "Product Description", 9.9m, 99, "Producttttttttttttttttttttttttttttttttt Imageeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee tooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo largestttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt");

        action
            .Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid image name, too long, maximum 250 characters");
    }

    [Fact(DisplayName = "Create product with null image name value")]
    public void CreateProduct_WithNullmageNameValue_ResultNoDomainExceptionValidate()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.9m, 99, null);

        action
            .Should()
            .NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create product with null imagem name value (NullReferenceException)")]
    public void CreateProduct_WithNullImageName_NoNullRefeferenceException()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.9m, 99, null);

        action
            .Should()
            .NotThrow<NullReferenceException>();
    }

    [Fact(DisplayName = "Create product with empty image name value")]
    public void CreateProduct_WithEmptyImageNameValue_ResultNoDomainExceptionValidate()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.9m, 99, "");

        action
            .Should()
            .NotThrow<DomainExceptionValidation>();
    }

    [Fact(DisplayName = "Create product with negative price value")]
    public void CreateProduct_WithNegativePriceValue_ResultNoDomainExceptionValidate()
    {
        Action action = () => new Product(1, "Product Name", "Product Description", -9.9m, 99, "Product Image");

        action
            .Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid price value");
    }

    [Theory(DisplayName = "Create product with negative stock value")]
    [InlineData(-5)]
    public void CreateProduct_WithInvalidStockValue_ExceptionDomainNegativeValue(int value)
    {
        Action action = () => new Product(1, "Product Name", "Product Description", 9.9m, value, "Product Image");

        action
            .Should()
            .Throw<DomainExceptionValidation>()
            .WithMessage("Invalid stock value");
    }
}
