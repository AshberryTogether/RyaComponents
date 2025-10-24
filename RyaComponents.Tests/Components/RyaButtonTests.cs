using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Tests.Components {
    internal class RyaButtonTests : BunitTestContext {
        [Test]
        public void RyaButton_DefaultConfiguration_DefaultRenderingCorrect() {
            var cut = RenderComponent<RyaButton>();
            var button = cut.Find("button");

            button.MarkupMatches("""
                <button class="rya-btn" type="button">
                    <span class="rya-btn-icon"></span>
                    <span class="rya-btn-txt"></span>          
                </button>
                """);
            Assert.AreEqual(cut.Instance.Visible, true);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("Test")]
        [TestCase(" Test ")]
        [TestCase("  ")]
        [TestCase("汉语")]
        [TestCase("Тест")]
        public void Text_SetText_TextRenderedCorrectly(string text) {
            var cut = RenderComponent<RyaButton>(parameters => parameters.Add(p => p.Text, text));
            var buttonText = cut.Find(".rya-btn-txt");
            buttonText.MarkupMatches($$"""
                <span class="rya-btn-txt">{{text}}</span>
                """);
        }

        [TestCaseSource(nameof(GetAttributeTestCases))]
        public void Attributes_SetAdditionalAttributes_AttributesRenderedCorrectly(
            Dictionary<string, object> attributes) {
            var cut = RenderComponent<RyaButton>(parameters
                => {
                    foreach(var attr in attributes) {
                        parameters.AddUnmatched(attr.Key, attr.Value);
                    }
                });
            var button = cut.Find("button");
            foreach(var attr in attributes) {
                Assert.AreEqual(attr.Value.ToString(), button.GetAttribute(attr.Key));
            }
        }

        private static IEnumerable<TestCaseData> GetAttributeTestCases() {
            yield return new TestCaseData(new Dictionary<string, object> {
                { "data-test", "test-value" }
            });
            yield return new TestCaseData(new Dictionary<string, object> {
                { "my-param", 3 },
                { "my-param-1", "hello" },
                { "test", 2.5d },
            });
            yield return new TestCaseData(new Dictionary<string, object> {
            });
        }

        [Test]
        public void Visible_SetVisibleProperty_RendersCorrectly() {
            var cutVisible = RenderComponent<RyaButton>(parameters => parameters.Add(p => p.Visible, true));
            var cutNotVisible = RenderComponent<RyaButton>(parameters => parameters.Add(p => p.Visible, false));

            Assert.IsNotNull(cutVisible.Find("button"));
            Assert.Throws<Bunit.ElementNotFoundException>(() => cutNotVisible.Find("button"));
            cutNotVisible.MarkupMatches(string.Empty);
        }

        [Test]
        public void Visible_SetVisibleProperty_UpdatesCorrectly() {
            var cut = RenderComponent<RyaButton>(parameters => parameters.Add(p => p.Visible, true));
            Assert.IsNotNull(cut.Find("button"));
            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Visible, false));
            Assert.Throws<Bunit.ElementNotFoundException>(() => cut.Find("button"));
            cut.MarkupMatches(string.Empty);
            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Visible, true));
            Assert.IsNotNull(cut.Find("button"));
        }

        [Test]
        public void SubmitFormOnClick_SetToTrue_RaisesFormSubmit() {
            
        }
    }
}
