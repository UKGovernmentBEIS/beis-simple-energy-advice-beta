using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SeaPublicWebsite.Services;

namespace Tests;

public class QuestionFlowServiceTests
{
    private IQuestionFlowService QuestionFlowService;

    public QuestionFlowServiceTests()
    { }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RequestsMeasures()
    {
        //Arrange
        //Act
        //Assert
    }
}