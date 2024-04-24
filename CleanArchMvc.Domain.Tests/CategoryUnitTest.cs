using CleanArchMvc.Domain.Entities;
using CleanArchMvc.Domain.Validations;
using FluentAssertions;
using System;
using Xunit;

namespace CleanArchMvc.Domain.Tests
{
    public class CategoryUnitTest
    {
        #region  Teste sem o uso de Fluent Assertion

        [Fact(DisplayName = "Update category with short name value (without Fluent Assertions)")]
        public void CreateCategoryWithoutFluentAssertions_ShortNameValue_ResultDomainExceptionValidationShortName()
        {
            Category category = new Category(1, "Category Name");

            Action act = () => category.Update("Ca");

            Assert.Throws<DomainExceptionValidation>(act);
        }

        #endregion

        #region Testes utilizando o Fluent Assertion

        [Fact(DisplayName = "Create category with valid state")]
        public void CreateCategory_WithValidParameters_ResultObjectValidState()
        {
            Action action = () => new Category(1, "Category Name"); ;

            action
                .Should()
                .NotThrow<DomainExceptionValidation>();
        }

        [Fact(DisplayName = "Create category with negative Id value")]
        public void CreateCategory_WithNegatviveIdValue_ResultDomainExceptionValidationInvalidId()
        {
            Action action = () => new Category(-1, "Category Name"); ;

            action
                .Should()
                .Throw<DomainExceptionValidation>()
                .WithMessage("Invalid Id value."); // A mensagem da exceção deve ser exatamente igual, caso contrário o teste falha.
        }

        [Fact(DisplayName = "Create category with short name value")]
        public void CreateCategory_ShortNameValue_ResultDomainExceptionValidationShortName()
        {
            Action action = () => new Category(1, "Ca"); ;

            action
                .Should()
                .Throw<DomainExceptionValidation>()
                .WithMessage("Invalid name, too short, minimum 3 characters");
        }

        [Fact(DisplayName = "Create category with missing name value")]
        public void CreateCategory_MissingNameValue_ResultDomainExceptionValidationMissingName()
        {
            Action action = () => new Category(1, "");

            action
                .Should()
                .Throw<DomainExceptionValidation>()
                .WithMessage("Invalid name. Name is required");
        }

        [Fact(DisplayName = "Create category with null name value")]
        public void CreateCategory_WithNullNameValue_ResultDomainExceptionValidationNullName()
        {
            Action action = () => new Category(1, null);

            action
                .Should()
                .Throw<DomainExceptionValidation>()
                .WithMessage("Invalid name. Name is required");
        }

        #endregion
    }
}
