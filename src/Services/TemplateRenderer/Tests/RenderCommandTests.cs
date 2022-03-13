using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using HelpLine.Services.TemplateRenderer.Application.Commands.Render;
using HelpLine.Services.TemplateRenderer.Application.Commands.Save;
using HelpLine.Services.TemplateRenderer.Models;
using HelpLine.Services.TemplateRenderer.Tests.SeedWork;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace HelpLine.Services.TemplateRenderer.Tests
{
    public class RenderCommandTests : TemplateRendererTestsBase
    {
        protected override string DbName => nameof(RenderCommandTests);

        [Test]
        public async Task RenderWithoutCtxAndComponents_IsSuccessful()
        {
            var template = new Template()
            {
                Id = "test",
                Group = "test",
                Props = new JObject()
                {
                    {"test", "test"}
                },
                Contexts = new string[] { },
                Content = "template {{props.test}}"
            };
            await Service.ExecuteAsync(new SaveTemplateCommand(template));
            var renderResult = await Service.ExecuteAsync(new RenderCommand(new []{template.Id}, new object()));
            var result = renderResult[template.Id];
            Assert.That(result.Contains("test"), Is.True);
            Assert.That(result.Contains("template"), Is.True);
        }

        [Test]
        public async Task RenderWithCtxAndComponents_IsSuccessful()
        {
            var template = new Template()
            {
                Id = "test",
                Group = "test",
                Props = new JObject()
                {
                    {"test", "test"}
                },
                Contexts = new string[] {"test" },
                Content = "template {{>test}}"
            };
            await Service.ExecuteAsync(new SaveTemplateCommand(template));

            await Service.ExecuteAsync(new SaveContextCommand(new Context
            {
                Id = "test",
                Group = "test",
                Data = new JObject()
                {
                    {"data", "ctx"}
                },
            }));

            await Service.ExecuteAsync(new SaveComponentCommand(new Component()
            {
                Id = "test",
                Group = "test",
                Content = "component {{ctx.test.data}}",
            }));
            var renderResult = await Service.ExecuteAsync(new RenderCommand(new []{template.Id}, new object()));
            var result = renderResult[template.Id];
            Assert.That(result.Contains("component"), Is.True);
            Assert.That(result.Contains("ctx"), Is.True);
            Assert.That(result.Contains("template"), Is.True);
        }

        [Test]
        public async Task RenderWithCtxAlias_IsSuccessful()
        {

            var ctx = new Context
            {
                Id = "test",
                Group = "test",
                Data = new JObject()
                {
                    {"data", "ctx"}
                },
                Alias = "alias"
            };
            var template1 = new Template()
            {
                Id = "test1",
                Group = "test",
                Props = new JObject(),
                Contexts = new string[] {ctx.Id },
                Content = "{{ctx.test.data}}"
            };
            var template2 = new Template()
            {
                Id = "test2",
                Group = "test",
                Props = new JObject(),
                Contexts = new string[] {ctx.Id },
                Content = "{{ctx.alias.data}}"
            };
            await Service.ExecuteAsync(new SaveContextCommand(ctx));
            await Service.ExecuteAsync(new SaveTemplateCommand(template1));
            await Service.ExecuteAsync(new SaveTemplateCommand(template2));



            var renderResult = await Service.ExecuteAsync(new RenderCommand(new []{template1.Id, template2.Id}, new object()));
            var result1 = renderResult[template1.Id];
            var result2 = renderResult[template2.Id];
            Assert.That(result1.Contains("ctx"), Is.True);
            Assert.That(result2.Contains("ctx"), Is.True);
        }

        [Test]
        public async Task RenderWithDuplicateCtxAlias_IsSuccessful()
        {

            var ctx1 = new Context
            {
                Id = "test1",
                Group = "test",
                Data = new JObject()
                {
                    {"data", "ctx1"}
                },
                Alias = "alias"
            };
            var ctx2 = new Context
            {
                Id = "test2",
                Group = "test",
                Data = new JObject()
                {
                    {"data", "ctx2"}
                },
                Alias = "alias"
            };

            var template1 = new Template()
            {
                Id = "test",
                Group = "test",
                Props = new JObject(),
                Contexts = new string[] {ctx1.Id, ctx2.Id },
                Content = "{{ctx.alias.data}}"
            };
            await Service.ExecuteAsync(new SaveContextCommand(ctx1));
            await Service.ExecuteAsync(new SaveContextCommand(ctx2));
            await Service.ExecuteAsync(new SaveTemplateCommand(template1));


            var renderResult = await Service.ExecuteAsync(new RenderCommand(new []{template1.Id}, new object()));
            var result = renderResult[template1.Id];
            Assert.That(result.Contains("ctx1"), Is.False);
            Assert.That(result.Contains("ctx2"), Is.True);
        }

        [Test]
        public async Task RenderWithExtendCtxAlias_IsSuccessful()
        {

            var ctx1 = new Context
            {
                Id = "test1",
                Group = "test",
                Data = new JObject()
                {
                    {"str1", "str1"},
                    {"array", new JArray("arr1", "arr2")},
                    {"obj1", new JObject()
                    {
                        {"obj11", "obj11"},
                        {"objOverMe", "objOverMe1"},
                    }}
                },
            };
            var ctx2 = new Context
            {
                Id = "test2",
                Group = "test",
                Data = new JObject()
                {
                    {"str2", "str2"},
                    {"array", new JArray("arr3")},
                    {"obj1", new JObject()
                    {
                        {"objOverMe", "objOverMe2"},
                        {"obj12", "obj12"},
                    }}
                },
                Extend = ctx1.Id
            };

            var template1 = new Template()
            {
                Id = "test",
                Group = "test",
                Props = new JObject(),
                Contexts = new string[] {ctx2.Id},
                Content = "{{ctx.test2.str1}} {{ctx.test2.str2}} {{ctx.test2.obj1.obj11}} {{ctx.test2.obj1.objOverMe}} {{ctx.test2.obj1.obj12}}"
            };
            await Service.ExecuteAsync(new SaveContextCommand(ctx1));
            await Service.ExecuteAsync(new SaveContextCommand(ctx2));
            await Service.ExecuteAsync(new SaveTemplateCommand(template1));


            var renderResult = await Service.ExecuteAsync(new RenderCommand(new []{template1.Id}, new object()));
            var result = renderResult[template1.Id];
            Assert.That(result.Contains("str1"), Is.True);
            Assert.That(result.Contains("str2"), Is.True);
            Assert.That(result.Contains("obj11"), Is.True);
            Assert.That(result.Contains("objOverMe2"), Is.True);
            Assert.That(result.Contains("obj12"), Is.True);
        }
    }
}
