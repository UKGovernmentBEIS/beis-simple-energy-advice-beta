using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using NUnit.Framework;

namespace Tests.Localisation;

[TestFixture]
public class WelshApostropheLintTests
{
    [Test]
    public void WelshResxValues_ShouldNotContainStraightApostrophes()
    {
        var root = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", ".."));
        var filesWithStraightApostrophes = Directory.GetFiles(root, "*.cy.resx", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}") &&
                           !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
            .Where(path => Regex.Matches(File.ReadAllText(path), @"<value[^>]*>(.*?)</value>", RegexOptions.Singleline)
                .Any(match => match.Groups[1].Value.Contains('\'')))
            .Select(path => Path.GetRelativePath(root, path))
            .ToList();

        // Only <value> text is checked — English name="..." keys may still use straight apostrophes.
        filesWithStraightApostrophes.Should().BeEmpty(
            "Welsh *.cy.resx <value> text should use curly apostrophes (’) instead of straight ones (')");
    }
}
