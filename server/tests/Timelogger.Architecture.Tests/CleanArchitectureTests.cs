using FluentAssertions;
using Timelogger.Api.Assembly;
using Timelogger.Application.Assembly;
using Timelogger.Bootstrap.Assembly;
using Timelogger.Domain.Assembly;
using Timelogger.Infrastructure.Assembly;
using Xunit;

namespace Timelogger.Architecture.Tests;

public class CleanArchitectureTests
{
    [Fact]
    public void Domain_ShouldNotDependOnAnyProject()
    {
        // Arrange
        var domainAssembly = typeof(IDomainAssemblyMarker).Assembly;
    
        /* Forbidden References */
        var applicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        var infrastructureAssembly = typeof(IInfrastructureAssemblyMarker).Assembly;
        var apiAssembly = typeof(IApiAssemblyMarker).Assembly;
        var bootstrapAssembly = typeof(IBootstrapAssemblyMarker).Assembly;
        
        // Act
        // Assert
        domainAssembly
            .Should()
            .NotReference(applicationAssembly).And
            .NotReference(infrastructureAssembly).And
            .NotReference(apiAssembly).And
            .NotReference(bootstrapAssembly);
    }
    
    [Fact]
    public void Api_ShouldNotReference_Forbidden_References()
    {
        // Arrange
        var apiAssembly = typeof(IApiAssemblyMarker).Assembly;
        
        /* Forbidden References */
        var domainAssembly = typeof(IDomainAssemblyMarker).Assembly;
        var infrastructureAssembly = typeof(IInfrastructureAssemblyMarker).Assembly;
        var bootstrapAssembly = typeof(IBootstrapAssemblyMarker).Assembly;
    
        // Act
        // Assert
        apiAssembly
            .Should()
            .NotReference(domainAssembly).And
            .NotReference(infrastructureAssembly).And
            .NotReference(bootstrapAssembly);
    }
    
    [Fact]
    public void Application_ShouldNotReference_Forbidden_References()
    {
        // Arrange
        var applicationAssembly = typeof(IApplicationAssemblyMarker).Assembly;
        
        /* Forbidden References */
        var infrastructureAssembly = typeof(IInfrastructureAssemblyMarker).Assembly;
        var apiAssembly = typeof(IApiAssemblyMarker).Assembly;
        var bootstrapAssembly = typeof(IBootstrapAssemblyMarker).Assembly;
        
        // Act
        // Assert
        applicationAssembly
            .Should()
            .NotReference(bootstrapAssembly).And
            .NotReference(apiAssembly).And
            .NotReference(infrastructureAssembly);
    }
    
    [Fact]
    public void Infrastructure_ShouldNotReference_Forbidden_References()
    {
        // Arrange
        var infrastructureAssembly = typeof(IInfrastructureAssemblyMarker).Assembly;
        
        /* Forbidden References */
        var apiAssembly = typeof(IApiAssemblyMarker).Assembly;
        var bootstrapAssembly = typeof(IBootstrapAssemblyMarker).Assembly;
    
        // Act
        // Assert
        infrastructureAssembly
            .Should()
            .NotReference(bootstrapAssembly).And
            .NotReference(apiAssembly);
    }
}